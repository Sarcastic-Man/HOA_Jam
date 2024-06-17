using System;
using HYPLAY.Runtime;
using TMPro;
using UnityEngine;

namespace HYPLAY.Demo
{
    public class DemoGetUser : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private void Start()
        {
            if (HyplayBridge.IsLoggedIn)
                text.text = "Logged in!";
        }

        public async void Login()
        {
            text.text = "Logging in...";
            await HyplayBridge.LoginAsync();
            text.text = "Logged in!";
        }

        public async void GetUser()
        {
            text.text = "Getting user...";
            var res = await HyplayBridge.GetUserAsync();
            if (res.Success)
                text.text = $"Successfully got user {res.Data.Username}";
            else
                text.text = $"Failed to get user: {res.Error}";
        }

        public async void DeleteSession()
        {
            text.text = "Logging out...";
            await HyplayBridge.LogoutAsync();
            text.text = "Logged out";
        }
    }
}