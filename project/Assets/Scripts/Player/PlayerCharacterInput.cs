namespace Game
{
    using Character;

    using UnityEngine;
    using UnityEngine.InputSystem;

    using VContainer.Unity;

    public sealed class PlayerCharacterInput : IStartable, TwoButtonsInputAction.IGameplayActions
    {
        private readonly TwoButtonsInputAction _inputAction;
        
        private readonly CharacterContext _context;
        private readonly Transform _characterTransform;
        private readonly Interactable[] _interactableArray;

        private float _moveDirection;
        
        public PlayerCharacterInput(TwoButtonsInputAction inputAction, CharacterContext context, Transform characterTransform, Interactable[] interactableArray)
        {
            _inputAction = inputAction;
            _context = context;
            _characterTransform = characterTransform;
            _interactableArray = interactableArray;
            _inputAction.Gameplay.SetCallbacks(this);
        }

        public void Start()
        {
            _inputAction.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if(context.canceled && !context.control.IsPressed())
                _moveDirection = 0f;
            
            if(context.performed)
                _moveDirection = context.ReadValue<float>();

            _context.MoveDirection = new Vector2(_moveDirection, 0);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            _context.JumpPressed = context.ReadValue<float>() > 0;

            if (context.performed)
            {
                _context.JumpInputElapsed = 0f;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            foreach (Interactable interactable in _interactableArray)
            {
                if (interactable.TryToInteract(_characterTransform))
                    break;
            }
        }
    }
}
