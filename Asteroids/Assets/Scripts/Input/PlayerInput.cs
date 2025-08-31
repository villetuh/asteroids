using Asteroids.Entities;
using Asteroids.Utilities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Asteroids.Input
{
    /// <summary>
    /// Component implementing player input.
    /// </summary>
    public class PlayerInput : IPlayerInput, ITickable
    {
        private readonly ITimeProvider timeProvider;
        private readonly InputAction moveInputAction;
        private readonly InputAction fireInputAction;

        private RotationDirection previousRotationDirection = RotationDirection.None;
        private bool previousThrusting = false;
        private float previousFireTime = 0.0f;

        private float fireCooldown = 0.5f; // In seconds

        /// <inheritdoc cref="IPlayerInput.OnRotate" />
        public Action<RotationDirection> OnRotate { get; set; }

        /// <inheritdoc cref="IPlayerInput.OnThrust" />
        public Action<bool> OnThrust { get; set; }

        /// <inheritdoc cref="IPlayerInput.OnFire" />
        public Action OnFire { get; set; }

        public PlayerInput(ITimeProvider timeProvider)
        {
            this.timeProvider = timeProvider
                ?? throw new ArgumentNullException(nameof(timeProvider), $"{nameof(PlayerInput)} requires reference to {nameof(ITimeProvider)}.");

            moveInputAction = InputSystem.actions.FindAction("Move")
                ?? throw new ArgumentNullException($"{nameof(PlayerInput)} couldn't find Move action");

            fireInputAction = InputSystem.actions.FindAction("Fire")
                ?? throw new ArgumentNullException($"{nameof(PlayerInput)} couldn't find Fire action");
        }

        public void Tick()
        {
            HandleRotationInput();

            HandleThrustInput();

            HandleFireInput();
        }

        private void HandleRotationInput()
        {
            Vector2 moveValue = moveInputAction.ReadValue<Vector2>();

            var rotationDirection = RotationDirection.None;
            if (moveValue.x < 0.0f)
            {
                rotationDirection = RotationDirection.Left;
            }
            else if (moveValue.x > 0.0f)
            {
                rotationDirection = RotationDirection.Right;
            }

            if (rotationDirection != previousRotationDirection)
            {
                OnRotate?.Invoke(rotationDirection);
                previousRotationDirection = rotationDirection;
            }
        }

        private void HandleThrustInput()
        {
            Vector2 moveValue = moveInputAction.ReadValue<Vector2>();

            var thrusting = moveValue.y > 0.0f;
            if (thrusting != previousThrusting)
            {
                OnThrust?.Invoke(thrusting);
                previousThrusting = thrusting;
            }
        }

        private void HandleFireInput()
        {
            if (fireInputAction.IsPressed())
            {
                var currentTime = timeProvider.Time;
                if (currentTime - previousFireTime > fireCooldown)
                {
                    OnFire?.Invoke();
                    previousFireTime = currentTime;
                }
            }
        }
    }
}
