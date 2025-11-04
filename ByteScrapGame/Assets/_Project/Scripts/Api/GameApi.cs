using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using _Project.Scripts.ElectricitySystem.Systems.Responses;
using _Project.Scripts.GameRoot;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;




namespace _Project.Scripts.ElectricitySystem.Systems
{

    public class LoginCreds
    {
        public LoginCreds(string username, string password)
        {
            this.name = username;
            this.password = password;
        }

        public string name;
        public string password;
    }
    
    public class GameApi
    {
        private const string BASE_URL = "https://bysc.zizazr.art/";//"http://localhost:8000/";
        
        #region Utils
        
        public string GetUrl() => PlayerPrefs.GetString("ApiURL",BASE_URL); 
        public void SetUrl(string url) => PlayerPrefs.SetString("ApiURL", url);
        
        public void SaveToken(string token) => PlayerPrefs.SetString("Token", token);
        public string GetToken() => PlayerPrefs.GetString("Token");


        public IEnumerator Post<T>(string endpoint, T data, UnityAction<long, string> onSuccess = null, UnityAction<long, string> onError = null, Dictionary<string, string> headers = null)
        {
            var json = JsonConvert.SerializeObject(data);
            var req = UnityWebRequest.Post(Path.Combine(GetUrl(), endpoint), json, "application/json");
            
            if (headers != null)
                foreach (var header in headers)
                    req.SetRequestHeader(header.Key, header.Value);

            yield return req.SendWebRequest();
            
            if (req.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(req.responseCode, req.error);
                Debug.LogError($"[HTTP API POST] ({endpoint}) - Error {req.responseCode}: {req.error}");
            }
            else
            {
                onSuccess?.Invoke(req.responseCode, req.downloadHandler.text);
                Debug.Log($"[HTTP API POST] ({endpoint}) - {req.responseCode}");
            }
    
        }

        public IEnumerator AuthorizedPost<T>(string endpoint, T data, UnityAction<long, string> onSuccess = null, 
            UnityAction<long, string> onError = null)
        {
            var headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + GetToken() }
            };
            return Post(endpoint, data, onSuccess, onError, headers);
        }

        public IEnumerator Get(string endpoint, UnityAction<long, string> onSuccess = null,
            UnityAction<long, string> onError = null, Dictionary<string, string> headers = null)
        {
            var req = UnityWebRequest.Get(Path.Combine(GetUrl(), endpoint));
            
            if (headers != null)
                foreach (var header in headers)
                    req.SetRequestHeader(header.Key, header.Value);
            
            yield return req.SendWebRequest();
            
            if (req.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(req.responseCode, req.error);
                Debug.LogError($"[HTTP API GET] ({endpoint}) - Error {req.responseCode}: {req.error} \nResponse: {req.downloadHandler.text}");
            }
            else
            {
                onSuccess?.Invoke(req.responseCode, req.downloadHandler.text);
                Debug.Log($"[HTTP API GET] ({endpoint}) - {req.responseCode} \nResponse: {req.downloadHandler.text}");
            }
        }

        public IEnumerator AuthorizedGet(string endpoint, UnityAction<long, string> onSuccess = null, 
            UnityAction<long, string> onError = null)
        {
            var headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer " + GetToken() }
            };
            return Get(endpoint, onSuccess, onError, headers);
        }

        private void ProcessAuth(string token)
        {
            Debug.Log("[HTTP API] Token saved \n" + token);
            SaveToken(token);
        }
        #endregion
        
        //-----Impl----
        public void Login(string username, string password, UnityAction<long, string> onSuccess = null, 
            UnityAction<long, string> onError = null)
        {
            var data = new LoginCreds(username, password);
            Bootstrap.Instance.StartCoroutine(
                Post("users/login", data, (c,token)=>
                {
                    ProcessAuth(token);
                    onSuccess?.Invoke(c,token);
                }, onError)
            );
        }

        public void Register(string username, string password, UnityAction<long, string> onSuccess = null, 
            UnityAction<long, string> onError = null)
        {
            var data = new LoginCreds(username, password);
            Bootstrap.Instance.StartCoroutine(
                Post("users/register", data, (c,token)=>
                {
                    ProcessAuth(token);
                    onSuccess?.Invoke(c,token);
                }, onError)
            );
        }
        
        public void GetCurrentUser(UnityAction<long, string> onSuccess = null, UnityAction<long, string> onError = null)
        {
            Bootstrap.Instance.StartCoroutine(
                AuthorizedGet("users/current", onSuccess, onError)
            );
        }

        public void Logout()
        {
            SaveToken(null);
        }

        public Coroutine UploadSave(string id, string levelData, UnityAction<long, string> onSuccess = null, UnityAction<long, string> onError = null)
        {
            var payload = new SaveUpload(id, levelData);
            return Bootstrap.Instance.StartCoroutine(
                AuthorizedPost($"saves/{id}", payload, onSuccess, onError)
            );
        }

        public void GetSaves(UnityAction<long, string> onSuccess = null, UnityAction<long, string> onError = null)
        {
            Bootstrap.Instance.StartCoroutine(
                AuthorizedGet("saves/", onSuccess, onError)
            );
        }

        public Coroutine GetSave(string id, UnityAction<long, SaveDataResponse> onSuccess = null, UnityAction<long, string> onError = null)
        {
            return Bootstrap.Instance.StartCoroutine(
            AuthorizedGet("saves/" + id, 
                (code, json) => 
                    onSuccess?.Invoke(code, new SaveDataResponse().FromJson(json)),
                onError)
            );
        }
    }
}