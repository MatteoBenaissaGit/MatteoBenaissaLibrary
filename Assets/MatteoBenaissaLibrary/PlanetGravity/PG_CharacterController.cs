using System;
using System.Collections.Generic;
using Data.Character;
using Gravity;
using MatteoBenaissaLibrary.SingletonClassBase;
using UnityEngine;


namespace MatteoBenaissaLibrary.PlanetGravity
{
    public enum CharacterGameplayAction
    {
        Walk = 0,
        StopWalk = 1,
        Jump = 2,
        Fall = 3,
        Land = 4,
    }
    
    public class PG_CharacterGameplayData
    {
        public bool IsGrounded
        {
            get => _isGrounded;
            set
            {
                _isGrounded = value;
                OnGrounded?.Invoke(_isGrounded);
            } 
        }
        private bool _isGrounded;
        public Action<bool> OnGrounded { get; set; }
        public bool IsMeshFollowingInputs { get; set; } = true;
        public bool IsLookingTowardCameraAim { get; set; } = false;
    }
    
    public class PG_CharacterController : Singleton<PG_CharacterController>
    {
        [field:SerializeField] public CharacterControllerData Data { get; private set; }
        [field:SerializeField] public GravityBody GravityBody { get; private set; }
        [field:SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field:SerializeField] public Collider Collider { get; private set; }
        [field:SerializeField] public Transform Mesh { get; private set; }
        [field:SerializeField] public PG_CameraController PgCameraController { get; private set; }  
        
        public PG_CharacterStateManager StateManager { get; private set; } 
        public PG_InputManager Input { get; private set; }
        public PG_CharacterGameplayData GameplayData { get; private set; }
        public Action<CharacterGameplayAction> OnCharacterAction { get; set; }
        
        
        #region MonoBehaviour methods

        protected override void InternalAwake()
        {
            GameplayData = new PG_CharacterGameplayData();
            
            Input = new PG_InputManager();
            Input.Initialize();
            
            StateManager = new PG_CharacterStateManager();
            StateManager.Initialize(this);
        }

        private void Update()
        {
            Input.Update();
            StateManager.UpdateState();
            
            SetMeshRotationToFollowInputs(GameplayData.IsLookingTowardCameraAim);
        }

        private void FixedUpdate()
        {
            StateManager.FixedUpdateState();
        }

        private void OnCollisionEnter(Collision collision)
        {
            StateManager.CurrentState.OnColliderEnter(collision);
        }

        private void OnDisable()
        {
            Input.Disable();
        }

        #endregion

        /// <summary>
        /// This method rotate the mesh toward where he is moving depending on inputs
        /// </summary>
        private void SetMeshRotationToFollowInputs(bool lookAtCameraAim = false)
        {
            if (GameplayData.IsMeshFollowingInputs == false)
            {
                return;
            }
            
            if (Input.CharacterControllerInput.IsMovingHorizontalOrVertical() == false && lookAtCameraAim == false)
            {
                return;
            }

            //get base local rotation
            Quaternion baseLocalRotation = Mesh.localRotation;
            
            //change it toward the desired world position
            Vector3 worldPositionToLook;
            if (lookAtCameraAim)
            {
                worldPositionToLook = Rigidbody.transform.position + PgCameraController.CameraTarget.transform.forward;
            }
            else
            {
                worldPositionToLook = Rigidbody.transform.position + GetCameraRelativeInputDirectionWorld();
            }
            Mesh.LookAt(worldPositionToLook);
            
            //nullify the x and z rotation to only rotate the character's y local rotation
            Vector3 lookAtRotation = Mesh.localRotation.eulerAngles;
            lookAtRotation.x = 0;
            lookAtRotation.z = 0;
            
            //set it back to base and lerp it to the new desired one
            Mesh.localRotation = baseLocalRotation;
            Quaternion desiredRotation = Quaternion.Euler(lookAtRotation);
            Mesh.localRotation = Quaternion.Slerp(baseLocalRotation, desiredRotation, Data.WalkRotationSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Create and get the result of a raycast hit toward the ground from the character's feet
        /// </summary>
        /// <returns>The RaycastHit of the raycast</returns>
        public RaycastHit GetRaycastTowardGround(float distanceCheckMultiplier = 1f, float upOffset = 0)
        {
            float raycastDistance = Data.RaycastTowardGroundToDetectFallDistance;
            Physics.Raycast(transform.position + transform.up * upOffset, -transform.up, out RaycastHit hit, raycastDistance * distanceCheckMultiplier);
            return hit;
        }

        /// <summary>
        /// This method return the input direction relative to the camera in the world space 
        /// </summary>
        /// <returns></returns>
        public Vector3 GetCameraRelativeInputDirectionWorld(bool normalized = true)
        {
            CharacterControllerInput input = Input.CharacterControllerInput;
            Vector2 movementInput = new Vector2(input.HorizontalMovement, input.VerticalMovement).normalized;
            Vector3 inputDirection = new Vector3(movementInput.x, 0f, movementInput.y);
            
            if (normalized)
            {
                inputDirection = inputDirection.normalized;
            }

            Vector3 localDirection = PgCameraController.CameraTargetMovementRelative.TransformDirection(inputDirection);

            return localDirection;
        }

#if UNITY_EDITOR

        private Dictionary<Vector3, JumpState> _jumpGizmos = new Dictionary<Vector3, JumpState>();
        private void OnDrawGizmos()
        {
            //ground detection
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (-transform.up * Data.RaycastTowardGroundToDetectFallDistance));
            
            if (Application.isPlaying == false)
            {
                return;
            }
            
            //gravity direction
            if (GravityBody != null && GravityBody.GravityDirection != Vector3.zero)
            {
                Gizmos.color = new Color(0f, 0.86f, 1f);
                Gizmos.DrawLine(transform.position, transform.position + (-GravityBody.GravityDirection * 5));
            }

            //jump
            PG_CharacterJumpState jump = StateManager.JumpState;
            PG_CharacterFallState fall = StateManager.FallState;
            if (StateManager.CurrentState == jump || StateManager.CurrentState == fall)
            {
                Vector3 position = Rigidbody.transform.position;
                if (_jumpGizmos.ContainsKey(position) == false)
                {
                    _jumpGizmos.Add(Rigidbody.transform.position, StateManager.CurrentState == jump ? jump.CurrentJumpState : JumpState.Down);
                }
            }
            else if (_jumpGizmos.Count > 200)
            {
                _jumpGizmos.Clear();
            }
            foreach (var jumpPosition in _jumpGizmos)
            {
                Gizmos.color = jumpPosition.Value switch
                {
                    JumpState.Up => new Color(0f, 1f, 0f, 0.74f),
                    JumpState.Apex => new Color(1f, 0.92f, 0.02f, 0.56f),
                    JumpState.Down => new Color(1f, 0f, 0f, 0.33f),
                    _ => throw new Exception()
                };
                Gizmos.DrawSphere(jumpPosition.Key,0.25f);
            }
            
            //movement direction
            Vector3 direction = GetCameraRelativeInputDirectionWorld();
            Gizmos.color = new Color(1f, 0.45f, 0f);
            Gizmos.DrawRay(transform.position,direction * 4);
            Gizmos.DrawSphere(transform.position + direction * 4, 0.2f);
            Gizmos.DrawRay(transform.position,PgCameraController.CameraTargetMovementRelative.up * 2);
            Gizmos.DrawSphere(transform.position + PgCameraController.CameraTargetMovementRelative.up * 2, 0.2f);
            
            //mesh up
            Vector3 meshUpDirection = Mesh.up;
            Gizmos.color = new Color(1f, 0.45f, 0f);
            Gizmos.DrawRay(transform.position,meshUpDirection * 4);
            Gizmos.DrawSphere(transform.position + meshUpDirection * 4, 0.2f);
        }

#endif
    }
}
