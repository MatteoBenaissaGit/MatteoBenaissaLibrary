using UnityEngine;
using UnityEngine.UI;

namespace MatteoBenaissaLibrary.AudioManager
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private void Start()
        {
            _slider.value = 1f;
            AudioManager.SoundManager.Instance.ChangeMasterVolume(_slider.value);
            _slider.onValueChanged.AddListener(value => AudioManager.SoundManager.Instance.ChangeMasterVolume(value));
        }
    }
}
