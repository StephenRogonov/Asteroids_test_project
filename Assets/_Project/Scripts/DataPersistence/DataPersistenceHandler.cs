using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.Common;
using _Project.Scripts.SceneInitializers;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.DataPersistence
{
    public class DataPersistenceHandler
    {
        private CloudDataHandler _cloudDataHandler;
        private FileDataHandler _fileDataHandler;
        private BootstrapSceneEntryPoint _bootstrapSceneEntryPoint;
        private SceneSwitcher _sceneSwitcher;
        private List<IDataPersistence> _dataPersistenceObjects = new();
        private PlayerData _cloudData;
        private PlayerData _fileData;

        public GameConfig GameConfig { get; private set; }
        public PlayerData PlayerData { get; private set; }

        public event Action PlayerDataChanged;

        public DataPersistenceHandler(
            FileDataHandler dataHandler,
            CloudDataHandler cloudDataHandler,
            SceneSwitcher sceneSwitcher
            )
        {
            _fileDataHandler = dataHandler;
            _cloudDataHandler = cloudDataHandler;
            _sceneSwitcher = sceneSwitcher;
        }

        public void SetBootstrapEntryPoint(BootstrapSceneEntryPoint bootstrapSceneEntryPoint)
        {
            _bootstrapSceneEntryPoint = bootstrapSceneEntryPoint;
        }

        public void AddDataObject(IDataPersistence dataPersistence) => _dataPersistenceObjects.Add(dataPersistence);

        public void RemoveDataObject(IDataPersistence dataPersistence) => _dataPersistenceObjects.Remove(dataPersistence);

        private async void StartNewGame()
        {
            PlayerData = new PlayerData();
            await SavePlayerData();
        }

        public void SetRemoteGameConfig(string config)
        {
            GameConfig = JsonConvert.DeserializeObject<GameConfig>(config);
        }

        public async UniTask LoadPlayerData()
        {
            Debug.Log("Load triggered.");

            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
                Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                _cloudData = await _cloudDataHandler.LoadData();
                _fileData = await _fileDataHandler.LoadData();

                if (_cloudData != null && _fileData != null)
                {
                    if (_cloudData.SaveDateTime == _fileData.SaveDateTime)
                    {
                        PlayerData = _cloudData;
                    }
                    else
                    {
                        await _bootstrapSceneEntryPoint.LoadSaveChoiseMenu(_fileData.SaveDateTime, _cloudData.SaveDateTime);
                        return;
                    }
                }
                else if (_cloudData == null && _fileData != null)
                {
                    PlayerData = _fileData;
                    await SavePlayerData();
                }
                else if (_cloudData != null && _fileData == null)
                {
                    PlayerData = _cloudData;
                    await SavePlayerData();
                }
                else
                {
                    PlayerData = null;
                }
            }
            else
            {
                PlayerData = await _fileDataHandler.LoadData();
            }


            if (PlayerData == null)
            {
                Debug.LogWarning("No game data found. New game data will be created.");
                StartNewGame();
            }

            _sceneSwitcher.LoadMenu();
        }

        public async void UseCloudData(bool useCloud)
        {
            if (useCloud == true)
            {
                PlayerData = _cloudData;
            }
            else
            {
                PlayerData = _fileData;
            }

            await SavePlayerData();
            _sceneSwitcher.LoadMenu();
        }

        public async UniTask SavePlayerData()
        {
            Debug.Log("Save triggered.");

            if (PlayerData == null)
            {
                Debug.LogError("No player data found. A New Game must be started before data can be loaded.");
                return;
            }

            if (_dataPersistenceObjects.Count > 0)
            {
                foreach (IDataPersistence obj in _dataPersistenceObjects)
                {
                    obj.SaveData(PlayerData);
                }
            }

            PlayerData.SaveDateTime = DateTime.Now;
            string dataToStore = JsonConvert.SerializeObject(PlayerData, Formatting.Indented);

            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
                Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                await _cloudDataHandler.SaveData(dataToStore);
            }

            await _fileDataHandler.SaveData(dataToStore);
            PlayerDataChanged?.Invoke();
        }
    }
}

