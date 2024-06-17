using System.Collections.Generic;
using System.Text;
using HYPLAY.Runtime;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace UnityEditor.Hyplay
{
    public class HyplaySettingsProvider
    {
        private static HyplaySettings _settings;
        private const string SettingsPath = "Assets/HYPLAY/Resources/Settings.asset";

        private static VisualElement _appList;
        private static VisualElement _currentApp;
        private static VisualElement _newApp;
        private static Button _createApp;
        
        private static SerializedProperty _appIcon;
        private static SerializedProperty _appBackground;
        private static SerializedProperty _appName;
        private static SerializedProperty _appDescription;
        private static SerializedProperty _appUrl;
        private static Label _createAppStatus;

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            // First parameter is the path in the Settings window.
            // Second parameter is the scope of this setting: it only appears in the Settings window for the Project scope.
            var provider = new SettingsProvider("Project/HYPLAY", SettingsScope.Project)
            {
                label = "HYPLAY Settings & App Creation",
                // activateHandler is called when the user clicks on the Settings item in the Settings window.
                activateHandler = CreateUI,

                // Populate the search keywords to enable smart search filtering and label highlighting:
                keywords = new HashSet<string>(new[] { "HYPLAYSettings" })
            };

            return provider;
        }

        private static void CreateUI(string searchContext, VisualElement rootElement)
        {
            if (_settings == null)
            {
                _settings = AssetDatabase.LoadAssetAtPath<HyplaySettings>(SettingsPath);
                if (_settings == null)
                {
                    _settings = ScriptableObject.CreateInstance<HyplaySettings>();
                    AssetDatabase.CreateAsset(_settings, SettingsPath);
                }
            }

            var settings = new SerializedObject(_settings);
            
            // rootElement is a VisualElement. If you add any children to it, the OnGUI function
            // isn't called because the SettingsProvider uses the UIElements drawing framework.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/HYPLAY/Editor/HyplayEditorStyles.uss");
            rootElement.styleSheets.Add(styleSheet);
            rootElement.AddToClassList("body");
            var title = new Label { text = "HYPLAY Settings" };
            title.AddToClassList("title");
            rootElement.Add(title);

            //var properties = new VisualElement { style = { flexDirection = FlexDirection.Column } };
            //properties.AddToClassList("property-list");
            //rootElement.Add(properties);

            var openProject = new Button
            {
                text = "Open HYPLAY Account page\nGo here to get your access token"
            };

            openProject.clicked += OpenAccountSettings;

            _currentApp = new VisualElement();
            _currentApp.Add(new PropertyField(settings.FindProperty("currentApp")));
            
            _newApp = new VisualElement();
            _createApp = new Button
            {
                text = "Create New App"
            };
            _createApp.clicked += CreateApp;
            BuildNewApp(settings);
            
            _appList = new VisualElement();

            var updateCurrent = new Button
            {
                text = "Update Current App"
            };
            updateCurrent.clicked += _settings.UpdateCurrent;
            
            var findApps = new Button()
            {
                text = "Get My Apps"
            };
            findApps.clicked += FindApps;
            _appList.Add(findApps);
            
            rootElement.Add(openProject);
            var accessToken = new PropertyField(settings.FindProperty("accessToken"));
            accessToken.RegisterValueChangeCallback(GetAccessToken);
            rootElement.Add(accessToken);
            
            _currentApp.Add(updateCurrent);
            rootElement.Add(_currentApp);
            rootElement.Add(_appList);
            rootElement.Add(_newApp);
            rootElement.Add(_createApp);
            _createAppStatus = new Label("");
            rootElement.Add(_createAppStatus);
            
            rootElement.Bind(settings);
        }
        
        private static async void FindApps()
        {
            var res = await _settings.GetApps();
            foreach (var app in res)
            {
                var btn = new Button()
                {
                    text = app.name
                };
                
                btn.clicked += () =>
                {
                    ShowApp(app);
                };
                
                _appList.Add(btn);
            }
        }

        private static void BuildNewApp(SerializedObject settings)
        {
            _newApp.style.display = DisplayStyle.None;
            
            _appIcon = settings.FindProperty("appIcon");
            BuildUIForProperty(_appIcon);
            
            _appBackground = settings.FindProperty("appBackground");
            BuildUIForProperty(_appBackground);
            
            _appName = settings.FindProperty("appName");
            BuildUIForProperty(_appName);
            
            _appDescription = settings.FindProperty("appDescription");
            BuildUIForProperty(_appDescription);
            
            _appUrl = settings.FindProperty("appUrl");
            BuildUIForProperty(_appUrl);
            _createApp.style.opacity = 1;
        }

        private static void BuildUIForProperty(SerializedProperty prop)
        {
            var field = new PropertyField(prop);
            field.RegisterValueChangeCallback(AppSettingsChanged);
            _newApp.Add(field);
        }
        
        private static async void CreateApp()
        {
            if (_createApp.text == "Create New App")
            {
                _createApp.text = "Submit";
                _newApp.style.display = DisplayStyle.Flex;
                SetButtonActive();
                //_createApp.
            } else if (CanSubmitNewApp())
            {
                _newApp.style.display = DisplayStyle.None;
                _createApp.text = "Create New App";
                _createAppStatus.text = "Uploading app icon";
                var appIconAsset = await _settings.CreateAsset(((Texture2D)_appIcon.objectReferenceValue).EncodeToPNG());
                _createAppStatus.text = "Uploading app background";
                var appBackgroundAsset = await _settings.CreateAsset(((Texture2D)_appBackground.objectReferenceValue).EncodeToPNG());
                _createAppStatus.text = "Creating app on HYPLAY backend";
                var redirect = _appUrl.stringValue;
                if (!redirect.EndsWith("/"))
                    redirect += "/";
                redirect += "redirect.html";

                var body = new Dictionary<string, object>()
                {
                    { "iconImageAssetId", appIconAsset.id },
                    { "backgroundImageAssetId", appBackgroundAsset.id },
                    { "name", _appName.stringValue },
                    { "description", _appDescription.stringValue },
                    {
                        "redirectUris", new []
                        {
                            redirect,
                            "myapp://token"
                        }
                    },
                    { "url", _appUrl.stringValue }
                };

                #if UNITY_2022_1_OR_NEWER
                var req = UnityWebRequest.Post("https://api.hyplay.com/v1/apps", HyplayJSON.Serialize(body), "application/json");
                #else
                var req = UnityWebRequest.Post("https://api.hyplay.com/v1/apps", "");
                HyplayJSON.SetData(ref req, HyplayJSON.Serialize(body));
                #endif

                    
                
                req.SetRequestHeader("x-authorization", _settings.AccessToken);
                await req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
                    var settings = new SerializedObject(_settings);
                    _createAppStatus.text = "Created app :)";
                    _settings.SetCurrent(HyplayJSON.Deserialize<HyplayApp>(req.downloadHandler.text));
                    
                    settings.Update();
                    settings.ApplyModifiedProperties();
                }
                else
                {
                    _createAppStatus.text = $"Failed to create app! {req.responseCode}";
                    Debug.LogError(req.error);
                }
                

            }
        }

        private static void AppSettingsChanged(SerializedPropertyChangeEvent evt)
        {
            SetButtonActive();
        }

        private static void SetButtonActive()
        {
            var canSubmit = CanSubmitNewApp();
            if (!canSubmit && _appDescription.stringValue.Length < 20)
                _createAppStatus.text = "Description must be >= 20 characters long";
            else
                _createAppStatus.text = "";
            _createApp.style.opacity = canSubmit ? 1 : 0.5f;
        }

        private static bool CanSubmitNewApp()
        {
            return
                _appIcon.objectReferenceValue != null
                && _appBackground.objectReferenceValue != null
                && !string.IsNullOrWhiteSpace(_appName.stringValue)
                && !string.IsNullOrWhiteSpace(_appDescription.stringValue)
                && !string.IsNullOrWhiteSpace(_appUrl.stringValue)
                && _appDescription.stringValue.Length >= 20;
        }

        private static void ShowApp(HyplayApp app)
        {
            var settings = new SerializedObject(_settings);
            _appList.style.display = DisplayStyle.None;
            _currentApp.style.display = DisplayStyle.Flex;
            //settings.FindProperty("currentApp").boxedValue = app;
            _settings.SetCurrent(app);
            settings.Update();
            settings.ApplyModifiedProperties();
        }

        private static void GetAccessToken(SerializedPropertyChangeEvent evt)
        {
            var newAT = evt.changedProperty.stringValue;
            if (string.IsNullOrWhiteSpace(newAT))
            {
                _appList.style.display = DisplayStyle.None;
                _currentApp.style.display = DisplayStyle.None;
                return;
            }
        }


        private static void OpenAccountSettings()
        {
            Application.OpenURL("https://hyplay.com/account/settings");
        }
    }
}