using UnityEngine;


namespace MatteoBenaissaLibrary.PlanetGravity
{
    /// <summary>
    /// An abstract class defining the methods contained in character's states
    /// </summary>
    public abstract class PG_CharacterStateBase
    {
        public bool IsActive { get; set; }
        
        /// <summary>
        /// The CharacterController the state is attached to 
        /// </summary>
        protected PG_CharacterController Controller { get; private set; }

        public PG_CharacterStateBase(PG_CharacterController controller)
        {
            Controller = controller;
            
            Controller.Input.CharacterControllerInput.OnJump += Jump;
        }

        /// <summary>
        /// This method is called when the state is entered by the controller
        /// </summary>
        public abstract void Enter();
        
        /// <summary>
        /// This method is called every frame by the controller
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// This method is called every fixed time by the controller
        /// </summary>
        public abstract void FixedUpdate();
        
        /// <summary>
        /// This method is called when the state is exited by the controller
        /// </summary>
        public abstract void Quit();
        
        /// <summary>
        /// This method is called when the controller collide on something
        /// </summary>
        public abstract void OnColliderEnter(Collision collision);
        
        /// <summary>
        /// This method is called when the jump button is pressed
        /// </summary>
        public abstract void Jump(bool isPressingJump);

        /// <summary>
        /// This method check if the player is on the ground after being in the air and launch a new state if so.
        /// </summary>
        /// <param name="specialConditionToSwitchToJumpState">A condition to jump after landing</param>
        public void CheckForChangeStateAtLanding(bool specialConditionToSwitchToJumpState = true)
        {
            RaycastHit hit = Controller.GetRaycastTowardGround();
            if (hit.collider == null || hit.collider.gameObject == Controller.gameObject  || hit.collider.isTrigger)
            {
                return;
            }
            
            bool inputMoving = Controller.Input.CharacterControllerInput.IsMovingHorizontalOrVertical(); 
            Controller.GameplayData.IsGrounded = true;
            
            if (Controller.Data.DoJumpBuffering 
                && Controller.Input.CharacterControllerInput.LastJumpInputTime < Controller.Data.JumpBufferTimeMaxBeforeLand
                && specialConditionToSwitchToJumpState)
            {
                Controller.StateManager.SwitchState(Controller.StateManager.JumpState, true);
                return;
            }
            
            Controller.StateManager.SwitchState(inputMoving ? Controller.StateManager.WalkState : Controller.StateManager.IdleState);
        }
    }
}
