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
            _context.MoveDirection = new Vector2(context.ReadValue<float>(), 0);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
