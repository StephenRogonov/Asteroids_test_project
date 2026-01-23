using Cysharp.Threading.Tasks;

namespace _Project.Scripts.DataPersistence
{
    public interface IDataHandler
    {
        public UniTask SaveData(string playerData);
        public UniTask<PlayerData> LoadData();
    }
}
