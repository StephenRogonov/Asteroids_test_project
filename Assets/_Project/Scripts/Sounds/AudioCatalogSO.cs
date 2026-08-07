using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Sounds
{
    [CreateAssetMenu(fileName = "AudioCatalogSO", menuName = "Scriptable Objects/AudioCatalogSO")]
    public class AudioCatalogSO : ScriptableObject
    {
        public List<AssetReferenceT<AudioClipDataSO>> ClipData;
    }
}