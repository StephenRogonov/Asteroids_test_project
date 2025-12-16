using Cysharp.Threading.Tasks;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace _Project.Scripts.Bootstrap.Authentication
{
    public class AuthInitialization
    {
        public async UniTask InitializeAuthentication()
        {
            try
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Authentication service successfully initialized.");
                await SignUpAnonymously();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async UniTask SignUpAnonymously()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Player successfully signed in.");
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}