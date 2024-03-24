namespace Gravity
{
    using UnityEngine;

    /// <summary>
    /// This class handle a gravity area that pull toward its center, usually it's a sphere
    /// </summary>
    public class GravityAreaCenter : GravityArea
    {
        [SerializeField] private SphereCollider _sphereCollider;

        public override Vector3 GetGravityDirection(GravityBody gravityBody)
        {
            return (transform.position - gravityBody.transform.position).normalized;
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_sphereCollider == null)
            {
                return;
            }
            Gizmos.color = new Color(1f, 0f, 0f, GizmoTransparency);
            Gizmos.DrawSphere(transform.position, _sphereCollider.radius * transform.root.localScale.x);
        }

#endif
    }
}