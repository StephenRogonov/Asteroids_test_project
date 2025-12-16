using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace _Project.Scripts.DataPersistence
{
    public class FileDataHandler : IDataHandler
    {
        private readonly string _dataDirPath = Application.persistentDataPath;
        private readonly string _playerDataFileName = "player_data.game";

        public async UniTask SaveData(PlayerData playerData)
        {
            string fullPath = Path.Combine(_dataDirPath, _playerDataFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);

                string dataToStore = JsonConvert.SerializeObject(playerData, Formatting.Indented);

                using FileStream stream = new FileStream(fullPath, FileMode.Create);
                using StreamWriter writer = new StreamWriter(stream);
                writer.Write(dataToStore);
            }
            catch (Exception e)
            {
                Debug.LogError("Can't save data to " + fullPath + "\n" + e.Message);
            }
        }

        public async UniTask<PlayerData> LoadData()
        {
            string fullPath = Path.Combine(_dataDirPath, _playerDataFileName);
            PlayerData loadedData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad;
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using StreamReader reader = new StreamReader(stream);
                        dataToLoad = reader.ReadToEnd();
                    }

                    loadedData = JsonConvert.DeserializeObject<PlayerData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Can't load data from " + fullPath + "\n" + e.Message);
                }
            }

            return loadedData;
        }
    }
}

