using System;
using UnityEngine;

namespace Gravity
{
    /// <summary>
    /// This class handle a gravity area that pull a body toward or away from a specific point
    /// </summary>
    public class GravityAreaPoint : GravityArea
    {
        [SerializeField] private Vector3 _center;
        [SerializeField] private bool _inverse;

        public override Vector3 GetGravityDirection(GravityBody gravityBody)
        {
            return _inverse ? ( gravityBody.transform.position - _center).normalized : (_center - gravityBody.transform.position).normalized;
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 1f, 1f, GizmoTransparency);
            Gizmos.DrawSphere(_center, 1f);
        }

#endif
    }
}