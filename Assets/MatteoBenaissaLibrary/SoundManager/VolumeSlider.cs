using UnityEngine;
using UnityEngine.UI;

namespace MatteoBenaissaLibrary.SoundManaging
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private void Start()
        {
            SoundManager.Instance.ChangeMasterVolume(_slider.value);
            _slider.onValueChanged.AddListener(value => SoundManager.Instance.ChangeMasterVolume(value));
        }
    }
}