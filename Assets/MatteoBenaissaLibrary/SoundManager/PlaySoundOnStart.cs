using UnityEngine;

namespace MatteoBenaissaLibrary.SoundManaging
{
    public class PlaySoundOnStart : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;

        private void Start()
        {
            global::MatteoBenaissaLibrary.SoundManaging.SoundManager.Instance.PlaySound(_clip);
        }
    }
}
