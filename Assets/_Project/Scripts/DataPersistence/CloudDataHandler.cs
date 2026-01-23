using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.Services.CloudSave;

namespace _Project.Scripts.DataPersistence
{
    public class CloudDataHandler : IDataHandler
    {
        private Dictionary<string, object> _playerData = new();
        private readonly string _playerDataName = "player_data";

        public async UniTask SaveData(string playerData)
        {
            _playerData.Add(_playerDataName, playerData);

            await CloudSaveService.Instance.Data.Player.SaveAsync(_playerData);
            _playerData.Clear();
        }

        public async UniTask<PlayerData> LoadData()
        {
            PlayerData loadedData = new();

            var rawLoadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>{_playerDataName});

            if (rawLoadedData.Count > 0)
            {
                if (rawLoadedData.TryGetValue(_playerDataName, out var firstProp))
                {
                    loadedData = JsonConvert.DeserializeObject<PlayerData>(firstProp.Value.GetAs<string>());
                }

                return loadedData;
            }
            else
            {
                return null;
            }
        }
    }
}

