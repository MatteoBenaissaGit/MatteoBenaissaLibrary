using UnityEngine;

namespace MatteoBenaissaLibrary.AudioManager
{
    public class ToggleAudio : MonoBehaviour
    {
        [SerializeField] private SoundType _toggleType;

        private bool _isActive = true;

        public void Toggle()
        {
            _isActive = _isActive == false;
            SoundManager.Instance.Toggle(_toggleType, _isActive);
        }
    }
}
