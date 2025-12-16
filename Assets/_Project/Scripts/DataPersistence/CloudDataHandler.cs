using _Project.Scripts.Obstacles.Score;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Unity.Services.CloudSave;

namespace _Project.Scripts.DataPersistence
{
    public class CloudDataHandler : IDataHandler
    {
        private Dictionary<string, object> _playerData = new();

        public async UniTask SaveData(PlayerData playerData)
        {
            string data = JsonConvert.SerializeObject(playerData, Formatting.Indented);
            _playerData = new Dictionary<string, object>(JsonConvert.DeserializeObject<Dictionary<string, object>>(data));
            await CloudSaveService.Instance.Data.Player.SaveAsync(_playerData);
            _playerData.Clear();
        }

        public async UniTask<PlayerData> LoadData()
        {
            PlayerData loadedData = new();

            var rawLoadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {
                nameof(PlayerData.SaveDateTime),
                nameof(PlayerData.NoAdsPurchased),
                nameof(PlayerData.Leaderboard)
            });

            if (rawLoadedData.Count > 0)
            {
                if (rawLoadedData.TryGetValue(nameof(PlayerData.SaveDateTime), out var firstProp))
                {
                    loadedData.SaveDateTime = firstProp.Value.GetAs<DateTime>();
                }

                if (rawLoadedData.TryGetValue(nameof(PlayerData.NoAdsPurchased), out var secondProp))
                {
                    loadedData.NoAdsPurchased = secondProp.Value.GetAs<bool>();
                }

                if (rawLoadedData.TryGetValue(nameof(PlayerData.Leaderboard), out var thirdProp))
                {
                    loadedData.Leaderboard = thirdProp.Value.GetAs<List<ScoreEntry>>();
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

