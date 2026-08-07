using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Sounds
{
    [CreateAssetMenu(fileName = "AudioClipDataSO", menuName = "Scriptable Objects/AudioClipDataSO")]
    public class AudioClipDataSO : ScriptableObject
    {
        public AssetReferenceT<AudioClip> ClipReference;
        public AudioID NameID;

        public AudioClip LoadedClip;
        [Range(0f, 1f)]
        public float Volume;
        public bool Loop;
        [Range(-3f, 3f)]
        public float Pitch;
        public bool Pausable;
    }
}