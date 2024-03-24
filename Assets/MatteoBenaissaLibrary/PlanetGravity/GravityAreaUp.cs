using UnityEngine;

namespace Gravity
{
    /// <summary>
    /// This class handle a gravity area that pull or push a body toward a floor
    /// </summary>
    public class GravityAreaUp : GravityArea
    {
        [SerializeField] private bool _inverseGravity;
        [SerializeField] private BoxCollider _boxCollider;

        protected override void Start()
        {
            base.Start();

            _boxCollider = GetComponent<BoxCollider>();
        }

        public override Vector3 GetGravityDirection(GravityBody gravityBody)
        {
            return _inverseGravity ? transform.up : -transform.up;
        }
    }
}