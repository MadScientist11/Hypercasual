using System;
using Hypercasual.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hypercasual.Services
{
    public interface IInputService : IService
    {
        Vector3 MousePosition { get; }
        public event Action OnLeftMouseButtonClicked;
    }

    public class InputService : IInputService, GameActions.IPlayerActions
    {
        public event Action OnLeftMouseButtonClicked;
        public Vector3 MousePosition => Mouse.current.position.ReadValue();

        private GameActions _input;

        public void Initialize()
        {
            _input = new GameActions();
            _input.Player.SetCallbacks(this);
            _input.Player.Enable();
        }

        public void DisablePlayerInput()
        {
            _input.Player.Disable();
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                OnLeftMouseButtonClicked?.Invoke();
        }
    }
}