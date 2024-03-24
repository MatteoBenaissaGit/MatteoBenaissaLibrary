using System;
using Data.Gravity;
using UnityEngine;

namespace Gravity
{
    /// <summary>
    /// This class is the parent of every gravity area class and its purpose is to check if a gravity body enter/exit it
    /// and give the gravity direction to apply to it if required
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public abstract class GravityArea : MonoBehaviour
    {
        [field:SerializeField] public int Priority { get; private set; }
        [field:SerializeField] public GravityAreaData AreaData { get; private set; }
        
        [SerializeField, Range(0,1)] protected float GizmoTransparency;

        protected Collider AreaCollider;
    
        protected virtual void Start()
        {
            AreaCollider = transform.GetComponent<Collider>();
            AreaCollider.isTrigger = true;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out GravityBody gravityBody))
            {
                gravityBody.AddGravityArea(this);
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out GravityBody gravityBody))
            {
                gravityBody.RemoveGravityArea(this);
            }
        }
    
        /// <summary>
        /// This method return the direction in which the gravity should apply for this area
        /// </summary>
        /// <param name="gravityBody">The body you wish to apply the area's gravity to</param>
        /// <returns>The gravity Direction</returns>
        public abstract Vector3 GetGravityDirection(GravityBody gravityBody);
    }
}