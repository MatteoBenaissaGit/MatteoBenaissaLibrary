using System;
using UnityEngine;


namespace MatteoBenaissaLibrary.PlanetGravity
{
    /// <summary>
    /// This state handle the character when he's moving on the ground
    /// </summary>
    public class PgPgCharacterWalkState : PG_CharacterStateBase
    {
        private float _currentAccelerationTime;
        private Vector3 _currentDirection;
        
        public PgPgCharacterWalkState(PG_CharacterController controller) : base(controller)
        {
        }
        
        public override string ToString()
        {
            return "Walk state";
        }


        public override void Enter()
        {
            Controller.GameplayData.IsGrounded = true;

            Controller.PgCameraController.SetCameraAfterCurrent(Controller.PgCameraController.Data.RunCamera);

            _currentAccelerationTime = 0;

            //if we were jumping and moving we don't reset the acceleration time of the player
            if (Controller.StateManager.PreviousState == Controller.StateManager.JumpState
                && Controller.StateManager.JumpState.IsJumpStatic == false)
            {
                _currentAccelerationTime = Controller.Data.AccelerationTime;
            }
            
            Controller.OnCharacterAction?.Invoke(CharacterGameplayAction.Walk);

            _currentDirection = Controller.GetCameraRelativeInputDirectionWorld();
        }

        public override void Update()
        {
            if (Controller.Input.CharacterControllerInput.IsMovingHorizontalOrVertical() == false)
            {
                Controller.StateManager.SwitchState(Controller.StateManager.IdleState);
            }
            
            CheckForFall();
        }

        public override void FixedUpdate()
        {
            HandleMovement();
        }

        public override void Quit()
        {
            Controller.PgCameraController.EndCurrentCameraState();

            Controller.OnCharacterAction?.Invoke(CharacterGameplayAction.StopWalk);
        }

        public override void OnColliderEnter(Collision collision)
        {
            
        }

        /// <summary>
        /// Handle the movement input and apply them to the rigidbody.
        /// Is called in FixedUpdate.
        /// </summary>
        private void HandleMovement()
        {
            _currentAccelerationTime += Time.fixedDeltaTime;
            float accelerationValue = Mathf.Clamp01(_currentAccelerationTime / Controller.Data.AccelerationTime);
            float accelerationMultiplier = Controller.Data.AccelerationCurve.Evaluate(accelerationValue);
            //Debug.Log($"{accelerationValue*100}%");
            
            Rigidbody rigidbody = Controller.Rigidbody;
            float speed = Controller.Data.WalkSpeed * accelerationMultiplier;
            
            Vector3 direction = Controller.GetCameraRelativeInputDirectionWorld();
            _currentDirection = Vector3.Lerp(_currentDirection, direction, Controller.Data.DirectionChangeLerpSpeed);
            float dotProduct = Vector3.Dot(direction, _currentDirection);
            float directionChangeSpeedMultiplier = 1f;
            if (dotProduct < 0)
            {
                float absDotProduct = Mathf.Clamp(Math.Abs(1 - dotProduct), 0.2f , 1f);
                directionChangeSpeedMultiplier = (absDotProduct * Controller.Data.DirectionChangeSpeedMultiplier);
            }

            rigidbody.MovePosition(rigidbody.position + direction * (speed * directionChangeSpeedMultiplier * Time.fixedDeltaTime));
        }
        
        /// <summary>
        /// Check if the player is falling while walking and change its state if necessary
        /// </summary>
        private void CheckForFall()
        {
            RaycastHit hit = Controller.GetRaycastTowardGround(10,1);
            if (hit.collider != null && hit.collider.gameObject != Controller.gameObject)
            {
                return;
            }

            Controller.GameplayData.IsGrounded = false;
            Controller.StateManager.SwitchState(Controller.StateManager.FallState);
        }
        
        public override void Jump(bool isPressingJump)
        {
            if (isPressingJump == false || IsActive == false)
            {
                return;
            }
            Controller.StateManager.SwitchState(Controller.StateManager.JumpState);
        }
    }
}