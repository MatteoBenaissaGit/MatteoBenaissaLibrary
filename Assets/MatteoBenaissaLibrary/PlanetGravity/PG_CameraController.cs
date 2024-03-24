using Data.Camera;
using DG.Tweening;
using UnityEngine;


namespace MatteoBenaissaLibrary.PlanetGravity
{
    public class PG_CameraController : MonoBehaviour
    {
        [field: SerializeField] public CameraData Data { get; private set; }
        [field:SerializeField] public UnityEngine.Camera Camera { get; private set; }
        [field: SerializeField] public Transform CameraTarget { get; private set; }
        [field: SerializeField] public PG_CameraFollowTransform FollowTransform { get; private set; }
        [field: SerializeField] public PG_CameraMoveWithInput MoveWithInput { get; private set; }

        public Transform CameraTargetMovementRelative { get; private set; }
        public CameraInformation CurrentCameraInformation { get; private set; }
        
        private Vector3 _baseRotationEuler;

        private void Awake()
        {
            _baseRotationEuler = Camera.transform.localRotation.eulerAngles;
            CurrentCameraInformation = Data.BaseCamera;

            CameraTargetMovementRelative = Instantiate(new GameObject("CameraTargetRelative")).transform;
            CameraTargetMovementRelative.parent = CameraTarget.parent;
            CameraTargetMovementRelative.localPosition = Vector3.zero;
        }

        /// <summary>
        /// Change the camera fov, offset and rotation
        /// </summary>
        /// <param name="cameraInformation">The camera information to apply</param>
        /// <param name="ease">The fov change ease</param>
        public void SetCamera(CameraInformation cameraInformation, Ease ease = Ease.Flash)
        {
            CurrentCameraInformation = cameraInformation;
            
            Camera.DOKill();
            Camera.transform.DOKill();
            
            SetFov(CurrentCameraInformation.Fov, CurrentCameraInformation.SpeedToChange, ease);    
            SetOffset(CurrentCameraInformation.Offset, CurrentCameraInformation.SpeedToChange, ease);
            SetRotation(CurrentCameraInformation.Rotation, CurrentCameraInformation.SpeedToChange, ease);
        }
        
        /// <summary>
        /// Change the fov of the camera
        /// </summary>
        /// <param name="fov">The fov target</param>
        /// <param name="duration">The time to change it</param>
        /// <param name="ease">The fov change ease</param>
        public void SetFov(float fov, float duration = 0.5f, Ease ease = Ease.Flash)
        {
            Camera.DOFieldOfView(fov, duration).SetEase(ease);
        }

        /// <summary>
        /// Set the camera offset 
        /// </summary>
        /// <param name="offset">the amount of offset as a Vector2</param>
        /// <param name="duration">the offset change time</param>
        /// <param name="ease">The offset change ease</param>
        public void SetOffset(Vector2 offset, float duration = 0.5f, Ease ease = Ease.Flash)
        {
            Camera.transform.DOLocalMove(offset, duration);
        }

        /// <summary>
        /// Set the camera offset 
        /// </summary>
        /// <param name="eulerAngles">the new euler Angles of the camera</param>
        /// <param name="duration">the rotation change time</param>
        /// <param name="ease">The rotation change ease</param>
        public void SetRotation(Vector3 eulerAngles, float duration = 0.5f, Ease ease = Ease.Flash)
        {
            Camera.transform.DOLocalRotate(_baseRotationEuler + eulerAngles, duration);
        }

        private CameraInformation _cameraToPutAfterCurrent;
        public void SetCameraAfterCurrent(CameraInformation cameraInformation)
        {
            _cameraToPutAfterCurrent = cameraInformation;
            if (_cameraToPutAfterCurrent.Priority >= CurrentCameraInformation.Priority)
            {
                EndCurrentCameraState();
            }
        }

        public void EndCurrentCameraState(bool forcePriority = false)
        {
            _cameraToPutAfterCurrent ??= Data.BaseCamera;
            
            if ( _cameraToPutAfterCurrent.Priority < CurrentCameraInformation.Priority && forcePriority == false)
            {
                return;
            }

            SetCamera(_cameraToPutAfterCurrent ?? Data.BaseCamera);
            _cameraToPutAfterCurrent = null;
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Vector3 origin = Camera.transform.position;
            Vector3 direction = Camera.transform.forward;
            Gizmos.color = new Color(1f, 0f, 0f, 0.34f);
            Gizmos.DrawRay(origin,direction * 60);
            
            Vector3 cameraForwardAimPosition = transform.position + transform.forward * 30f;
            Gizmos.DrawSphere(cameraForwardAimPosition,0.2f);
        }

#endif
    }
}