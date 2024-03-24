using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MatteoBenaissaLibrary.PlanetGravity
{
    public enum InputDevice
    {
        Keyboard = 0,
        XboxGamepad = 1,
        PlayGamepad = 2
    }

    /// <summary>
    /// This class handle the inputs the player is making and storing them in dedicated classes
    /// </summary>
    public class PG_InputManager
    {
        public CharacterControllerInput CharacterControllerInput { get; private set; }
        public CameraMovementInput CameraMovementInput { get; private set; }

        public static InputDevice CurrentDevice { get; set; }

        private PG_PlayerInput _input;

        public void Initialize()
        {
            CharacterControllerInput = new CharacterControllerInput();
            CameraMovementInput = new CameraMovementInput();

            _input = new PG_PlayerInput();
            _input.Enable();

            //Character controller
            _input.CharacterController.MoveHorizontal.performed += CharacterControllerInput.SetHorizontalMovement;
            _input.CharacterController.MoveHorizontal.canceled += CharacterControllerInput.SetHorizontalMovement;
            _input.CharacterController.MoveVertical.performed += CharacterControllerInput.SetVerticalMovement;
            _input.CharacterController.MoveVertical.canceled += CharacterControllerInput.SetVerticalMovement;
            _input.CharacterController.Jump.performed += CharacterControllerInput.SetJump;
            _input.CharacterController.Jump.canceled += CharacterControllerInput.SetJump;

            //camera movement
            _input.CameraMovement.XMovement.performed += CameraMovementInput.SetXMovement;
            _input.CameraMovement.XMovement.canceled += CameraMovementInput.SetXMovement;
            _input.CameraMovement.YMovement.performed += CameraMovementInput.SetYMovement;
            _input.CameraMovement.YMovement.canceled += CameraMovementInput.SetYMovement;
        }

        public void Disable()
        {
            //Character controller
            _input.CharacterController.MoveHorizontal.performed -= CharacterControllerInput.SetHorizontalMovement;
            _input.CharacterController.MoveHorizontal.canceled -= CharacterControllerInput.SetHorizontalMovement;
            _input.CharacterController.MoveVertical.performed -= CharacterControllerInput.SetVerticalMovement;
            _input.CharacterController.MoveVertical.canceled -= CharacterControllerInput.SetVerticalMovement;
            _input.CharacterController.Jump.performed -= CharacterControllerInput.SetJump;
            _input.CharacterController.Jump.canceled -= CharacterControllerInput.SetJump;

            //camera movement
            _input.CameraMovement.XMovement.performed -= CameraMovementInput.SetXMovement;
            _input.CameraMovement.XMovement.canceled -= CameraMovementInput.SetXMovement;
            _input.CameraMovement.YMovement.performed -= CameraMovementInput.SetYMovement;
            _input.CameraMovement.YMovement.canceled -= CameraMovementInput.SetYMovement;
        }

        public void Update()
        {
            CharacterControllerInput.LastJumpInputTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// This class receive and store the character controller inputs
    /// </summary>
    public class CharacterControllerInput
    {
        public float HorizontalMovement { get; private set; }
        public float VerticalMovement { get; private set; }
        public Action<bool> OnJump { get; set; }
        public float LastJumpInputTime { get; internal set; }

        public bool IsMovingHorizontalOrVertical()
        {
            return HorizontalMovement != 0 || VerticalMovement != 0;
        }

        public void SetHorizontalMovement(InputAction.CallbackContext context)
        {
            if (context.performed == false)
            {
                HorizontalMovement = 0;
                return;
            }

            HorizontalMovement = context.ReadValue<float>();
        }

        public void SetVerticalMovement(InputAction.CallbackContext context)
        {
            if (context.performed == false)
            {
                VerticalMovement = 0;
                return;
            }

            VerticalMovement = context.ReadValue<float>();
        }

        public void SetJump(InputAction.CallbackContext context)
        {
            OnJump.Invoke(context.performed);

            if (context.performed)
            {
                LastJumpInputTime = 0;
            }
        }

        public Vector2 GetInputVector()
        {
            return new Vector3(HorizontalMovement, VerticalMovement).normalized;
        }
    }

    /// <summary>
    /// This class receive and store the camera control inputs
    /// </summary>
    public class CameraMovementInput
    {
        public float CameraXMovement { get; private set; }
        public float CameraYMovement { get; private set; }

        public void SetXMovement(InputAction.CallbackContext context)
        {
            if (context.performed == false)
            {
                CameraXMovement = 0;
                return;
            }

            CameraXMovement = context.ReadValue<float>();
        }

        public void SetYMovement(InputAction.CallbackContext context)
        {
            if (context.performed == false)
            {
                CameraYMovement = 0;
                return;
            }

            CameraYMovement = context.ReadValue<float>();
        }
    }
}
