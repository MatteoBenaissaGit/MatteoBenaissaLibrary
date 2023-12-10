using UnityEngine;

namespace MatteoBenaissaLibrary.Ragdoll
{
    public class RagdollLimb : MonoBehaviour
    {
        [SerializeField] private Transform _limbToCopy;

        public void CopyBaseLimb()
        {
            transform.position = _limbToCopy.position;
            transform.rotation = _limbToCopy.rotation;
            transform.localScale = _limbToCopy.localScale;
        }
    }
}