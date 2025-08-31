using Asteroids.Input;
using System;
using UnityEngine;
using VContainer;

namespace Asteroids.Entities
{
    /// <summary>
    /// Component handling player related logic.
    /// </summary>
    public class Player : MonoBehaviour, IGameEntity, IDisposable
    {
        private IGameController gameController;
        private IPlayerInput playerInput;

        private PlayerRotateDirection rotationDirection = PlayerRotateDirection.None;
        private readonly float rotationSpeed = 180.0f; // degrees per second

        private bool thrusting = false;
        private readonly float thrustPower = 2.5f; // units per second
        private readonly float drag = 0.33f; // velocity retention per second

        private readonly float bulletOffset = 0.5f; // distance from player center to bullet spawn position

        public GameEntityType EntityType => GameEntityType.Player;

        public float Rotation
        {
            get { return transform.rotation.eulerAngles.z; }
        }

        public Vector2 Position
        {
            get { return (Vector2)transform.position; }
        }

        public Vector2 Speed { get; private set; }

        [Inject]
        private void Construct(IGameController gameController, IPlayerInput playerInput)
        {
            this.gameController = gameController
                ?? throw new System.ArgumentNullException(nameof(gameController), $"{nameof(Player)} requires reference to {nameof(IGameController)}.");

            this.playerInput = playerInput
                ?? throw new System.ArgumentNullException(nameof(playerInput), $"{nameof(Player)} requires reference to {nameof(IPlayerInput)}.");

            playerInput.OnRotate += Rotate;
            playerInput.OnThrust += Thrust;
            playerInput.OnFire += Fire;
        }

        public void Rotate(PlayerRotateDirection direction)
        {
            rotationDirection = direction;
        }
        
        public void Thrust(bool thrust)
        {
            thrusting = thrust;
        }
        
        public void Fire()
        {
            var direction = GetDirection(Rotation);
            gameController.OnPlayerFire(Position + bulletOffset * direction, direction);
        }

        public void Update()
        {
            var rotation = GetUpdatedRotation(rotationDirection);
            var speed = GetUpdatedSpeed(rotation, thrusting);
            var position = GetUpdatedPosition(speed);

            Speed = speed;
            transform.SetPositionAndRotation(position, Quaternion.Euler(Vector3.forward * rotation));
        }

        private float GetUpdatedRotation(PlayerRotateDirection rotationDirection)
        {
            var rotation = transform.rotation.eulerAngles.z;

            if (rotationDirection != PlayerRotateDirection.None)
            {
                var rotationDirectionValue = rotationDirection == PlayerRotateDirection.Left ? 1.0f : -1.0f;
                rotation += (float)rotationDirectionValue * rotationSpeed * Time.deltaTime;
                rotation = rotation % 360.0f;
            }

            return rotation;
        }

        private Vector2 GetUpdatedSpeed(float rotation, bool thrusting)
        {
            var speed = Speed;
            if (thrusting)
            {
                var thrustVector = thrustPower * GetDirection(rotation);
                speed += thrustVector * Time.deltaTime;

                if (speed.magnitude > thrustPower)
                {
                    speed = speed.normalized * thrustPower;
                }
            }
            else
            {
                speed *= Mathf.Pow(drag, Time.deltaTime);

                if (speed.magnitude < 0.01f)
                {
                    speed = Vector2.zero;
                }
            }

            return speed;
        }

        private Vector2 GetUpdatedPosition(Vector2 speed)
        {
            var position = (Vector2)transform.position;
            if (speed != Vector2.zero)
            {
                position += Speed * Time.deltaTime;
            }
            return position;
        }

        private Vector2 GetDirection(float rotation)
        {
            float radians = rotation * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        public void Dispose()
        {
            if (playerInput != null)
            {
                playerInput.OnRotate -= Rotate;
                playerInput.OnThrust -= Thrust;
                playerInput.OnFire -= Fire;
            }
        }
    }
}
