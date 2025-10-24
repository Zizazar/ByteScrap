using System.ComponentModel;
using System.IO;
using _Project.Scripts.ElectricitySystem.Systems.Responses;
using _Project.Scripts.GameRoot;
using _Project.Scripts.GameRoot.States.GameStates;
using _Project.Scripts.GameRoot.States.PlayerStates;
using _Project.Scripts.LevelAndGoals;
using _Project.Scripts.UI;
using IngameDebugConsole;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Commands
{
    public class DebugCommands
    {
        public void Init()
        {
            DebugLogConsole.AddCommand("state", "Show current states", ShowStates);
            DebugLogConsole.AddCommand<string, SaveTypes>("load", "Load level by id", LoadLevel);
            DebugLogConsole.AddCommand("queue", "Show components queue", ShowComponentsQueue);
            DebugLogConsole.AddCommand("menu", "Open menu", GoToMenu);
            DebugLogConsole.AddCommand("goals", "Shows goals", ShowGoals);
            DebugLogConsole.AddCommand("opengamefolder", "Opens game folder", OpenGameFolder);
            DebugLogConsole.AddCommand("settings", "Opens settings", OpenSettings);
            DebugLogConsole.AddCommand<string, string>("login", "Login test", LoginTest);
            DebugLogConsole.AddCommand<string, string>("reg", "Login test", RegisterTest);
            DebugLogConsole.AddCommand("token", "Get token of current user", GetToken);
            DebugLogConsole.AddCommand("getcurrentuser", "Get meta of current user", GetCurrentUser);
            DebugLogConsole.AddCommand<string[]>("apiurl", "Set / get api url", ApiUrl);
            DebugLogConsole.AddCommand("logout", "Logout", Logout);
            
        }

        private void Logout()
        {
            Bootstrap.Instance.api.Logout();
        }

        private void ApiUrl(string[] args)
        {
            switch (args[0].ToLower())
            {
                case "get":
                    Debug.Log(
                    Bootstrap.Instance.api.GetUrl());
                    break;
                case "set":
                    Bootstrap.Instance.api.SetUrl(args[1]);
                    Debug.Log($"Url set to {args[1]}");
                    break;
                    
            }
        }

        private void GetCurrentUser()
        {
            Bootstrap.Instance.api.GetCurrentUser(
                    (code, msg) =>
                    {
                        var data = new UserMetaResponse().FromJson(msg);
                        Debug.Log($"Current user meta: {data}\nID: {data.id}\nName: {data.name}");
                    });
        }

        private void GetToken()
        {
            var token = Bootstrap.Instance.api.GetToken();
            Debug.Log(token);
            GUIUtility.systemCopyBuffer = token;
        }

        private void RegisterTest(string name, string password)
        {
            Bootstrap.Instance.api.Register(name, password,(code, msg) => Debug.Log($"{code}: {msg}"));
        }

        private void LoginTest(string username, string password)
        {
            Bootstrap.Instance.api.Login(username, password, (code, msg) => Debug.Log($"{code}: {msg}"));
        }

        private void OpenSettings()
        {
            Bootstrap.Instance.ui.settings.Open();
        }

        private void OpenGameFolder()
        {
            Application.OpenURL($"file://{Application.persistentDataPath}");
        }

        private void ShowComponentsQueue()
        {
            string report = "Components in update queue: \n";
            foreach (var component in CircuitManager.Instance.GetUpdateQueue())
            {
                report += component.name + "\n";
            }
            Debug.Log(report);
            
        }

        void ShowStates()
        {
            Debug.Log("Current states:" +
                      "\n Game: " + Bootstrap.Instance.sm_Game.CurrentState.GetType().Name +  
                      " | Player: " + Bootstrap.Instance.playerController?.statemachine.CurrentState.GetType().Name
                      );
            
        }

        private void LoadLevel(string id, SaveTypes saveType)
        {
            Bootstrap.Instance.sm_Game.ChangeState(new LoadingLevelState(id, saveType));
        }

        private void GoToMenu()
        {
            Bootstrap.Instance.CloseLevel();
        }

        private void ShowGoals()
        {
            var msg = "Goal List:\n";
            
            foreach (var goal in Bootstrap.Instance.goalSystem.GetGoals()) // разобраться с терянием инстанса
            {
                msg += $"{(goal.isCompleted ? "V" : "X")}| {goal.name} ({goal.type})\n";
            }
            Debug.Log(msg);
        }
    }
}