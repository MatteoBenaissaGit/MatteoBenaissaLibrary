using System;
using MatteoBenaissaLibrary.SingletonClassBase;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MatteoBenaissaLibrary.AudioManager
{
    public enum SoundType
    {
        None = 0,
        Effect = 1,
        Dialog = 2,
        Music = 3
    }
    
    public class SoundManager : Singleton<SoundManager>
    {
        [field:Header("Clips")] [field:SerializeField] public AudioClip BaseMusic { get; private set; }

        public float GlobalVolume
        {
            get => _globalVolume;
            set
            {
                _globalVolume = value;
                _musicSource.volume = _baseMusicVolume * _globalVolume;
            }
        }
        
        [Header("Sources")] [SerializeField] private AudioSource _musicSource;
        [SerializeField] private GameObject _effectSource, _dialogSource;
        
        private float _globalVolume;
        private float _baseMusicVolume;
        
        protected override void InternalAwake()
        {
            GlobalVolume = 1f;

            if (BaseMusic == null)
            {
                Debug.Log("No base music");
                return;
            }
            PlayMusic(BaseMusic);
        }
        
        public void ChangeMasterVolume(float value)
        {
            AudioListener.volume = value;
        }

        public void PlaySound(SoundEnum sound, float volume = 0.1f, SoundType type = SoundType.None)
        {
            if (type == SoundType.Music)
            {
                Debug.LogWarning("Can't play music with the PlaySound method, use PlayMusic instead");
                return;
            }
            
            volume *= GlobalVolume;

            AudioClip clip = SoundResourceManager.Instance.GetAudioClip(sound);

            GameObject soundObject = type switch
            {
                SoundType.None => new GameObject("sound"),
                SoundType.Dialog => _dialogSource,
                SoundType.Effect => _effectSource,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            AudioSource source = soundObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.Play();
            
            Object.Destroy(soundObject, clip.length);
        }

        public void PlayMusic(AudioClip music, float volume = 0.1f)
        {
            _baseMusicVolume = volume;

            _musicSource.clip = music;
            _musicSource.loop = true;
            _musicSource.volume = _baseMusicVolume * _globalVolume;
            _musicSource.Play();
        }

        public void Toggle(SoundType type, bool toggle)
        {
            switch (type)
            {
                case SoundType.None:
                    break;
                case SoundType.Effect:
                    _effectSource.SetActive(toggle);
                    break;
                case SoundType.Dialog:
                    _dialogSource.SetActive(toggle);
                    break;
                case SoundType.Music:
                    _musicSource.gameObject.SetActive(toggle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}