using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace HYPLAY.Runtime
{
    public class HyplaySettings : ScriptableObject
    {
        #if UNITY_EDITOR
        [SerializeField, Space] private string accessToken;
        public string AccessToken => accessToken;
        #endif
    
        [SerializeField] private Texture2D appIcon;
        [SerializeField] private Texture2D appBackground;
        [SerializeField] private string appName;
        [SerializeField] private string appDescription;
        [SerializeField] private string appUrl;

        [SerializeField] private HyplayApp currentApp;
        public HyplayApp Current => currentApp;
        
        [field: SerializeField] public string Token { get; private set; }

        #if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void DoLogin(string appid, string url);
        #endif
        
        public void SetCurrent(HyplayApp current)
        {
            currentApp = current;
        }

        public void SetToken(string token)
        {
            Token = token;
        }
        
        
        internal void DoLogin()
        {
            #if UNITY_EDITOR
            HyplayBridge.DeepLink("myapp://token#token=DEV_TOKEN");
            #elif UNITY_WEBGL
            DoLogin(Current.id, Current.redirectUris.First());
            #else
            var redirectUri = UnityWebRequest.EscapeURL("myapp://token");
            var url = "https://hyplay.com/oauth/authorize/?appId=" + Current.id + "&chain=HYCHAIN&responseType=token&redirectUri=" + redirectUri;
            Application.OpenURL(url);
            #endif
        }
        
        
        #if UNITY_EDITOR
        public async Task<List<HyplayApp>> GetApps()
        {
            var req = UnityWebRequest.Get("https://api.hyplay.com/v1/apps");
            req.SetRequestHeader("x-authorization", accessToken);
            await req.SendWebRequest();
            
            return HyplayJSON.Deserialize<List<HyplayApp>>(req.downloadHandler.text);
        }

        public async void UpdateCurrent()
        {
            #if UNITY_2022_1_OR_NEWER
            var req = UnityWebRequest.Post($"https://api.hyplay.com/v1/apps/{currentApp.id}", HyplayJSON.Serialize(currentApp), "application/json");
            #else
            var req = UnityWebRequest.Post($"https://api.hyplay.com/v1/apps/{currentApp.id}", "");
            HyplayJSON.SetData(ref req, HyplayJSON.Serialize(currentApp));
            #endif
            req.method = "PATCH";
            
            req.SetRequestHeader("x-authorization", accessToken);
            await req.SendWebRequest();
            Debug.Log(req.downloadHandler.text);
        }

        public async Task<HyplayImageAsset> CreateAsset(byte[] data)
        {
            var body = new Dictionary<string, string>
            {
                { "fileBase64", System.Convert.ToBase64String(data) }
            };
            
            #if UNITY_2022_1_OR_NEWER
            var req = UnityWebRequest.Post($"https://api.hyplay.com/v1/assets", HyplayJSON.Serialize(body), "application/json");
            #else
            var req = UnityWebRequest.Post($"https://api.hyplay.com/v1/assets", "");
            HyplayJSON.SetData(ref req, HyplayJSON.Serialize(body));
            #endif
            req.SetRequestHeader("x-authorization", accessToken);
            await req.SendWebRequest();

            return HyplayJSON.Deserialize<HyplayImageAsset>(req.downloadHandler.text);
        }
        #endif
    }
}