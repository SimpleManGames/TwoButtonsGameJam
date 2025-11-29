namespace Game
{
    using Character;

    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UIElements;

    using VContainer.Unity;

    public sealed class PlayerCharacterInput : IStartable, TwoButtonsInputAction.IGameplayActions
    {
        private readonly TwoButtonsInputAction _inputAction;
        private readonly CharacterContext _context;

        private float _moveDirection = 0f;
        
        public PlayerCharacterInput(TwoButtonsInputAction inputAction, CharacterContext context)
        {
            _inputAction = inputAction;
            _context = context;
            _inputAction.Gameplay.SetCallbacks(this);
        }

        public void Start()
        {
            _inputAction.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.canceled && !context.control.IsPressed())
                _moveDirection = context.ReadValue<float>();
            
            if(context.performed)
                _moveDirection = context.ReadValue<float>();
            
            _context.MoveDirection = new Vector2(_moveDirection, 0);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            _context.JumpPressed = context.ReadValue<float>() > 0;
            if (_context.JumpPressed)
                _context.JumpInputElapsed = 0f;
        }
    }
}
