using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Project.Scripts.DataPersistence
{
    public interface IDataHandler
    {
        public UniTask SaveData(PlayerData playerData);
        public UniTask<PlayerData> LoadData();
    }
}
