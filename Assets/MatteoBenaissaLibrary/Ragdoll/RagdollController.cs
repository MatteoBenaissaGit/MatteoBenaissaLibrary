using System.Collections.Generic;
using UnityEngine;

namespace MatteoBenaissaLibrary.Ragdoll
{
    public class RagdollController : MonoBehaviour
    {
        [SerializeField] private GameObject _characterToCopy;
        [SerializeField] private GameObject _characterRagdoll;
        [SerializeField] private List<RagdollLimb> _limbs = new List<RagdollLimb>();

        public void CopyLimbs()
        {
            _characterToCopy.SetActive(false);
            _characterRagdoll.SetActive(true);
            
            _limbs.ForEach(x => x.CopyBaseLimb());
        }
    }
}