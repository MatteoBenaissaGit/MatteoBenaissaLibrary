using UnityEngine;


namespace MatteoBenaissaLibrary.PlanetGravity
{
    /// <summary>
    /// This class handle the management of the character states
    /// </summary>
    public class PG_CharacterStateManager
    {
        public PG_CharacterStateBase CurrentState { get; private set; }
        public PG_CharacterStateBase PreviousState { get; private set; }
        public PG_CharacterIdleState IdleState { get; private set; }
        public PgPgCharacterWalkState WalkState { get; private set; }
        public PG_CharacterJumpState JumpState { get; private set; }
        public PG_CharacterFallState FallState { get; private set; }
        public PG_CharacterInAirState InAirState { get; private set; }

        /// <summary>
        /// This method initialize the state classes
        /// </summary>
        /// <param name="controller">The CharacterController running the states</param>
        public void Initialize(PG_CharacterController controller)
        {
            IdleState = new PG_CharacterIdleState(controller);
            WalkState = new PgPgCharacterWalkState(controller);
            JumpState = new PG_CharacterJumpState(controller);
            FallState = new PG_CharacterFallState(controller);
            InAirState = new PG_CharacterInAirState(controller);
            
            SwitchState(IdleState);
        }

        /// <summary>
        /// This method handle the switch between two states
        /// </summary>
        /// <param name="state">the state to switch to</param>
        /// <param name="disableSecurityCheck">check if the state you want to switch to is already the current state</param>
        public void SwitchState(PG_CharacterStateBase state, bool disableSecurityCheck = false)
        {
            if (state == CurrentState && disableSecurityCheck == false)
            {
                Debug.LogWarning($"You're switching to a state you're already in. {CurrentState.ToString()} to {state.ToString()}");
                return;
            }
            

            PreviousState = CurrentState;
            if (PreviousState != null)
            {
                PreviousState.IsActive = false;
            }
            PreviousState?.Quit();
            
            //Debug.Log($"SWITCH STATE : {PreviousState?.ToString()} -> {state.ToString()}");

            CurrentState = state;
            CurrentState.IsActive = true;
            CurrentState.Enter();
        }

        /// <summary>
        /// This method update the current state every-frame
        /// </summary>
        public void UpdateState()
        {
            CurrentState?.Update();
        }

        /// <summary>
        /// This method fixed update the current state
        /// </summary>
        public void FixedUpdateState()
        {
            CurrentState?.FixedUpdate();
        }
    }
}