using System.Collections.Generic;
using System.Linq;
using Data.Gravity;
using UnityEngine;

namespace Gravity
{
    /// <summary>
    /// This class is attached to a rigidbody that needs to be affected by gravity area
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class GravityBody : MonoBehaviour
    {
        public bool ApplyAreaGravity { get; set; } = true;
        
        public Vector3 GravityDirection
        {
            get
            {
                if (_gravityAreas == null || _gravityAreas.Count == 0)
                {
                    return Vector3.zero;
                }
                _gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
                return _gravityAreas.Last().GetGravityDirection(this).normalized;
            }
        }
        public GravityAreaData AreaData
        {
            get
            {
                if (_gravityAreas.Count == 0)
                {
                    return null;
                }
                _gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
                return _gravityAreas.Last().AreaData;
            }
        }

        public float GravityMultiplier { get; set; } = 1f;

        [SerializeField] private Rigidbody _rigidbody;
        
        private List<GravityArea> _gravityAreas;

        private void Start()
        {
            _gravityAreas = new List<GravityArea>();
        }
    
        private void FixedUpdate()
        {
            if (AreaData == null || ApplyAreaGravity == false) 
            {
                return;
            }
            _rigidbody.AddForce(GravityDirection * (AreaData.GravityForce * GravityMultiplier * Time.fixedDeltaTime), ForceMode.Acceleration);

            Quaternion upRotation = Quaternion.FromToRotation(transform.up, -GravityDirection);
            Quaternion newRotation = Quaternion.Slerp(_rigidbody.rotation, upRotation * _rigidbody.rotation, Time.fixedDeltaTime * AreaData.RotationSpeed);;
            _rigidbody.MoveRotation(newRotation);
        }

        /// <summary>
        /// Add a gravity area to the current gravity areas affecting the body
        /// </summary>
        /// <param name="gravityArea">the area to add</param>
        public void AddGravityArea(GravityArea gravityArea)
        {
            _gravityAreas.Add(gravityArea);
        }

        /// <summary>
        /// Remove a gravity area to the current gravity areas affecting the body
        /// </summary>
        /// <param name="gravityArea">the area to remove</param>
        public void RemoveGravityArea(GravityArea gravityArea)
        {
            _gravityAreas.Remove(gravityArea);
        }

        /// <summary>
        /// Clear the current area's applied to the player
        /// </summary>
        public void ClearGravityAreas()
        {
            _gravityAreas.Clear();    
        }
    }
}