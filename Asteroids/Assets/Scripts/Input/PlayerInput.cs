using Asteroids.Entities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Asteroids.Input
{
    public class PlayerInput : ITickable
    {
        private readonly IPlayerActions playerActions;

        private readonly InputAction moveInputAction;
        private readonly InputAction fireInputAction;

        public PlayerInput(IPlayerActions playerActions)
        {
            this.playerActions = playerActions
                ?? throw new ArgumentNullException(nameof(playerActions), $"{nameof(PlayerInput)} requires reference to {nameof(IPlayerActions)}");

            moveInputAction = InputSystem.actions.FindAction("Move")
                ?? throw new ArgumentNullException($"{nameof(PlayerInput)} couldn't find Move action");

            fireInputAction = InputSystem.actions.FindAction("Fire")
                ?? throw new ArgumentNullException($"{nameof(PlayerInput)} couldn't find Fire action");
        }

        public void Tick()
        {
            Vector2 moveValue = moveInputAction.ReadValue<Vector2>();
            if (moveValue.x < 0.0f)
            {
                playerActions.Rotate(PlayerRotateDirection.Left);
            }
            else if (moveValue.x > 0.0f)
            {
                playerActions.Rotate(PlayerRotateDirection.Right);
            }
            else
            {
                playerActions.Rotate(PlayerRotateDirection.None);
            }

            if (moveValue.y > 0.0f)
            {
                playerActions.Thrust(true);
            }
            else
            {
                playerActions.Thrust(false);
            }

            if (fireInputAction.IsPressed())
            {
                playerActions.Fire();
            }
        }
    }
}
