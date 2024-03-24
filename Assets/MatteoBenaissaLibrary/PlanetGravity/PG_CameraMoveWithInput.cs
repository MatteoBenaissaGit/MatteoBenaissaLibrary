using UnityEngine;

namespace MatteoBenaissaLibrary.PlanetGravity
{
    public class PG_CameraMoveWithInput : MonoBehaviour
    {
        [SerializeField] private PG_CharacterController pgCharacterController;

        private Transform _cameraTarget;
        private Transform _cameraTargetMovementRelative;
        private float _currentYRotation;
        
        private void Awake()
        {
            Cursor.visible = false;
        }

        private void Start()
        {
            _cameraTarget = PG_CharacterController.Instance.PgCameraController.CameraTarget;
            _cameraTargetMovementRelative = PG_CharacterController.Instance.PgCameraController.CameraTargetMovementRelative;
        }

        private void Update()
        {
            Vector3 localRotation = _cameraTarget.transform.localRotation.eulerAngles;
            Vector3 localRotationMovementRelative = _cameraTargetMovementRelative.transform.localRotation.eulerAngles;
            
            //x movement
            float rotationInputX = pgCharacterController.Input.CameraMovementInput.CameraXMovement 
                                  * pgCharacterController.PgCameraController.Data.CameraXMovementSpeed 
                                  * pgCharacterController.PgCameraController.CurrentCameraInformation.RotationSpeedMultiplier.x
                                  * Time.deltaTime;
            localRotation.y += rotationInputX;
            
            //movement relative rotation
            localRotationMovementRelative.y += rotationInputX;
            _cameraTargetMovementRelative.localRotation = Quaternion.Euler(localRotationMovementRelative);
            
            //y movement
            float rotationInputY = pgCharacterController.Input.CameraMovementInput.CameraYMovement 
                                   * pgCharacterController.PgCameraController.Data.CameraYMovementSpeed 
                                   * pgCharacterController.PgCameraController.CurrentCameraInformation.RotationSpeedMultiplier.y
                                   * Time.deltaTime;
            //clamp y
            if (_currentYRotation + rotationInputY > PG_CharacterController.Instance.PgCameraController.Data.CameraYClamp.y || 
                _currentYRotation + rotationInputY < PG_CharacterController.Instance.PgCameraController.Data.CameraYClamp.x )
            {
                rotationInputY = 0;
            }
            _currentYRotation += rotationInputY;
            localRotation.x += rotationInputY;
            
            _cameraTarget.localRotation = Quaternion.Euler(localRotation);
        }
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_cameraTarget == null)
            {
                return;
            }
            Gizmos.color = Color.green;
            Vector3 upPoint = _cameraTarget.transform.position + _cameraTarget.transform.up * 2;
            Gizmos.DrawLine(_cameraTarget.transform.position, upPoint);
            Gizmos.DrawLine(_cameraTarget.transform.position, pgCharacterController.PgCameraController.Camera.transform.position);
            Gizmos.DrawLine(upPoint, pgCharacterController.PgCameraController.Camera.transform.position);
        }

#endif
    }
}