using UnityEditor;
using UnityEngine;

namespace MatteoBenaissaLibrary.PlanetGravity
{
    /// <summary>
    /// Place this script on a parent of the camera, it allows it to follow a specific transform with a base offset
    /// </summary>
    public class PG_CameraFollowTransform : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private UnityEngine.Camera _cameraChild;
        [SerializeField] private float _positionSmoothSpeed = 10f;
        [SerializeField] private float _rotationSmoothSpeed = 50f;
        [SerializeField] private bool _isFollowingAPhysicObject;

        private float _positionDistance;
        private float _height;

        private void Awake()
        {
            _positionDistance = Vector3.Distance(transform.position,_cameraTarget.position);
            _height = Mathf.Abs(transform.position.y - _cameraTarget.position.y);
        }

        private void Update()
        {
            if (_isFollowingAPhysicObject)
            {
                return;
            }
            
            transform.position = Vector3.Lerp(transform.position, GetDesiredPosition(), Time.deltaTime * _positionSmoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, _cameraTarget.rotation, _rotationSmoothSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_isFollowingAPhysicObject == false)
            {
                return;
            }
            
            transform.position = Vector3.Lerp(transform.position, GetDesiredPosition(), Time.fixedDeltaTime * _positionSmoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, _cameraTarget.rotation, _rotationSmoothSpeed * Time.fixedDeltaTime);
        }

        private Vector3 GetDesiredPosition()
        {
            return _cameraTarget.position + (-_cameraTarget.forward * _positionDistance) + _cameraTarget.up * _height;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(_cameraTarget.position, _cameraTarget.up, _positionDistance);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position - transform.up * _height);
        }

#endif
    }
}
