using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.Bootstrap.LoadOptions;
using _Project.Scripts.Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.DataPersistence
{
    public class DataPersistenceHandler
    {
        private FileDataHandler _fDataHandler;
        private CloudDataHandler _cDataHandler;
        private LoadOptionsModel _loadOptionsModel;
        private SceneSwitcher _sceneSwitcher;
        private List<IDataPersistence> _dataPersistenceObjects = new();
        private PlayerData _cData;
        private PlayerData _fData;

        public GameConfig GameConfig { get; private set; }
        public PlayerData PlayerData { get; private set; }

        public event Action PlayerDataChanged;

        public DataPersistenceHandler(
            FileDataHandler dataHandler,
            CloudDataHandler cloudDataHandler,
            SceneSwitcher sceneSwitcher
            )
        {
            _fDataHandler = dataHandler;
            _cDataHandler = cloudDataHandler;
            _sceneSwitcher = sceneSwitcher;
        }

        public void SetLoadOptionsModel(LoadOptionsModel loadOptionsModel)
        {
            _loadOptionsModel = loadOptionsModel;
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
                _cData = await _cDataHandler.LoadData();
                _fData = await _fDataHandler.LoadData();

                if (_cData != null && _fData != null)
                {
                    if (_cData.SaveDateTime == _fData.SaveDateTime)
                    {
                        PlayerData = _cData;
                    }
                    else
                    {
                        _loadOptionsModel.TriggerLoadOptions(_fData.SaveDateTime, _cData.SaveDateTime);
                        return;
                    }
                }
                else if (_cData == null && _fData != null)
                {
                    PlayerData = _fData;
                    await SavePlayerData();
                }
                else if (_cData != null && _fData == null)
                {
                    PlayerData = _cData;
                    await SavePlayerData();
                }
                else
                {
                    PlayerData = null;
                }
            }
            else
            {
                PlayerData = await _fDataHandler.LoadData();
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
                PlayerData = _cData;
            }
            else
            {
                PlayerData = _fData;
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
                await _cDataHandler.SaveData(dataToStore);
            }

            await _fDataHandler.SaveData(dataToStore);
            PlayerDataChanged?.Invoke();
        }
    }
}

