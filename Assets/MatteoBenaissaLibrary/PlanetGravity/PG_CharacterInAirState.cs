using UnityEngine;


namespace MatteoBenaissaLibrary.PlanetGravity
{
    public class PG_CharacterInAirState : PG_CharacterStateBase
    {
        public PG_CharacterInAirState(PG_CharacterController controller) : base(controller)
        {
        }

        public override string ToString()
        {
            return "In Air State";
        }

        public override void Enter()
        {
            Controller.GameplayData.IsGrounded = false;
            Controller.PgCameraController.SetCameraAfterCurrent(Controller.PgCameraController.Data.FallCamera);
        }

        public override void Update()
        {
            
        }

        public override void FixedUpdate()
        {
            float speed = Controller.Data.InAirMovementAmplitude;
            Vector3 forceDirection = Controller.GetCameraRelativeInputDirectionWorld();

            Vector3 force = forceDirection * (speed * Time.fixedDeltaTime);
            Controller.Rigidbody.AddForce(force, ForceMode.Impulse);
        }

        public override void Quit()
        {
            Controller.PgCameraController.EndCurrentCameraState();
        }

        public override void OnColliderEnter(Collision collision)
        {
        }

        public override void Jump(bool isPressingJump)
        {
        }
    }
}