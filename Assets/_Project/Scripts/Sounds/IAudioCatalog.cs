using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Sounds
{
    public interface IAudioCatalog
    {
        public UniTask LoadAssets();
        public AudioClipDataSO GetAudioData(AudioID id);
        public void ReleaseLoadedAssets();
    }
}
