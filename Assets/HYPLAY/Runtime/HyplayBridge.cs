using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace HYPLAY.Runtime
{
    public class HyplayResponse<T> where T : class
    {
        public T Data;
        public string Error;
        public bool Success => string.IsNullOrWhiteSpace(Error);
    }
    public static class HyplayBridge
    {
        public static event Action LoggedIn;
        public static bool IsLoggedIn { get; private set; }
        
        private static HyplaySettings _settings;
        private static HyplayReceiveMessage _oauth;
        private static HyplayUser _currentUser;
        
        private const string CachedTokenKey = "hyplay-token";
        
        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            _currentUser = null;
            _settings = Resources.Load<HyplaySettings>("Settings");
            Application.deepLinkActivated += DeepLink;

            _oauth = new GameObject("OAuthManager").AddComponent<HyplayReceiveMessage>();
            _oauth.OnMessageReceived += GetToken;

            if (PlayerPrefs.HasKey(CachedTokenKey))
            {
                _settings.SetToken(PlayerPrefs.GetString(CachedTokenKey));
                IsLoggedIn = true;
                LoggedIn?.Invoke();
                GetUserAsync();
            }
            else
                IsLoggedIn = false;
        }

        private static void GetToken(string obj)
        {
            _settings.SetToken(obj);
            PlayerPrefs.SetString(CachedTokenKey, obj);
            IsLoggedIn = true;
            LoggedIn?.Invoke();
        }

        internal static void DeepLink(string obj)
        {
            var token = obj.Split("token=").Last();
            _settings.SetToken(token);
            PlayerPrefs.SetString(CachedTokenKey, token);
            IsLoggedIn = true;
            LoggedIn?.Invoke();
        }

        public static async void Login(Action onComplete)
        {
            await LoginAsync();
            onComplete?.Invoke();
        }

        public static async Task LoginAsync()
        {
            _settings.DoLogin();
            while (string.IsNullOrWhiteSpace(_settings.Token))
                await Task.Yield();
        }

        public static async Task LogoutAsync()
        {
            var body = new Dictionary<string, string>
            {
                { "chain", "HYCHAIN" },
                { "appId", _settings.Current.id },
                { "userId", _currentUser.Id }
            };
            var req = UnityWebRequest.Post("https://api.hyplay.com/v1/sessions", 
                HyplayJSON.Serialize(body)
                #if UNITY_2022_1_OR_NEWER
                ,"application/json");
                #else
                );
                HyplayJSON.SetData(ref req, HyplayJSON.Serialize(body));
                #endif
            req.method = UnityWebRequest.kHttpVerbDELETE;
            if (SetAuthHeader(req))
            {
                await req.SendWebRequest();
                
                _settings.SetToken(string.Empty);
                PlayerPrefs.SetString(CachedTokenKey, string.Empty);
                IsLoggedIn = false;
            }
            else
            {
                Debug.LogWarning("Tried signing out but there's no token!");
            }
        }

        public static async void Logout(Action onComplete)
        {
            await LogoutAsync();
            onComplete?.Invoke();
        }

        private static bool SetAuthHeader(UnityWebRequest req)
        {
            if (string.IsNullOrWhiteSpace(_settings.Token))
            {
                Debug.LogError("Not logged in");
                return false;
            }
            
            req.SetRequestHeader("x-session-authorization", _settings.Token);
            return true;
        }
        
        public static async Task<HyplayResponse<HyplayUser>> LoginAndGetUserAsync()
        {
            await LoginAsync();
            return await GetUserAsync();
        }

        public static async void LoginAndGetUser(Action<HyplayResponse<HyplayUser>> onComplete)
        {
            var res = await LoginAndGetUserAsync();
            onComplete(res);
        }

        public static async Task GetUser(Action<HyplayResponse<HyplayUser>> onComplete)
        {
            var res = await GetUserAsync();
            onComplete(res);
        }

        public static async Task<HyplayResponse<HyplayUser>> GetUserAsync(bool useCache = true)
        {
            if (useCache && _currentUser != null)
            {
                return new HyplayResponse<HyplayUser>
                {
                    Data = _currentUser
                };
            }
            
            var req = UnityWebRequest.Get("https://api.hyplay.com/v1/users/me");
            if (!SetAuthHeader(req))
            {
                Debug.LogError("Not logged in");
                _currentUser = null;
                IsLoggedIn = false;
                return new HyplayResponse<HyplayUser>
                {
                    Data = null,
                    Error = "Not logged in"
                };
            }
            
            await req.SendWebRequest();

            if (req.responseCode == 401)
            {
                Debug.LogError("Not logged in");
                _currentUser = null;
                IsLoggedIn = false;
                return new HyplayResponse<HyplayUser>
                {
                    Data = null,
                    Error = "Not logged in"
                };
            }

            var user = HyplayJSON.Deserialize<HyplayUser>(req.downloadHandler.text);
            var error = req.downloadHandler.error;

            _currentUser = user;
            return new HyplayResponse<HyplayUser>
            {
                Data = user,
                Error = error
            };
        }
    }
}