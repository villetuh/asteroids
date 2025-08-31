using Asteroids.Configurations;
using Asteroids.Input;
using Asteroids.Utilities;
using System;
using UnityEngine;
using VContainer;

namespace Asteroids.Entities
{
    /// <summary>
    /// Component handling player related logic.
    /// </summary>
    public class Player : MonoBehaviour, IGameEntity
    {
        private IGameController gameController;
        private ITimeProvider timeProvider;
        private IPlayerInput playerInput;
        private IScreenEdgeHelper screenEdgeHelper;
        private IGameSettings gameSettings;

        private RotationDirection rotationDirection = RotationDirection.None;

        private bool thrusting = false;

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
        private void Construct(IGameController gameController, ITimeProvider timeProvider,
            IPlayerInput playerInput, IScreenEdgeHelper screenEdgeHelper,
            IGameSettings gameSettings)
        {
            this.gameController = gameController
                ?? throw new System.ArgumentNullException(nameof(gameController), $"{nameof(Player)} requires reference to {nameof(IGameController)}.");

            this.timeProvider = timeProvider
                ?? throw new System.ArgumentNullException(nameof(timeProvider), $"{nameof(Player)} requires reference to {nameof(ITimeProvider)}.");

            this.playerInput = playerInput
                ?? throw new System.ArgumentNullException(nameof(playerInput), $"{nameof(Player)} requires reference to {nameof(IPlayerInput)}.");

            playerInput.OnRotate += Rotate;
            playerInput.OnThrust += Thrust;
            playerInput.OnFire += Fire;

            this.screenEdgeHelper = screenEdgeHelper
                ?? throw new System.ArgumentNullException(nameof(screenEdgeHelper), $"{nameof(Player)} requires reference to {nameof(ScreenEdgeHelper)}.");

            this.gameSettings = gameSettings;
            if (gameSettings == null)
            {
                throw new ArgumentNullException(nameof(gameSettings), $"{nameof(Player)} requires reference to {nameof(IGameSettings)}.");
            }
        }

        public void Rotate(RotationDirection direction)
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
            gameController.OnPlayerFire(Position + gameSettings.PlayerSettings.BulletOffset * direction, direction);
        }

        public void Update()
        {
            var rotation = GetUpdatedRotation(rotationDirection);
            var speed = GetUpdatedSpeed(rotation, thrusting);
            var position = GetUpdatedPosition(speed);

            Speed = speed;
            transform.SetPositionAndRotation(position, Quaternion.Euler(Vector3.forward * rotation));
        }

        public void OnDestroy()
        {
            if (playerInput != null)
            {
                playerInput.OnRotate -= Rotate;
                playerInput.OnThrust -= Thrust;
                playerInput.OnFire -= Fire;
            }
        }

        private float GetUpdatedRotation(RotationDirection rotationDirection)
        {
            var rotation = transform.rotation.eulerAngles.z;

            if (rotationDirection != RotationDirection.None)
            {
                var rotationDirectionValue = rotationDirection == RotationDirection.Left ? 1.0f : -1.0f;
                rotation += (float)rotationDirectionValue * gameSettings.PlayerSettings.RotationSpeed * timeProvider.DeltaTime;
                rotation = rotation % 360.0f;
            }

            return rotation;
        }

        private Vector2 GetUpdatedSpeed(float rotation, bool thrusting)
        {
            var speed = Speed;
            var thrustPower = gameSettings.PlayerSettings.ThrustPower;
            var drag = gameSettings.PlayerSettings.Drag;

            if (thrusting)
            {
                var thrustVector = thrustPower * GetDirection(rotation);
                speed += thrustVector * timeProvider.DeltaTime;

                if (speed.magnitude > thrustPower)
                {
                    speed = speed.normalized * thrustPower;
                }
            }
            else
            {
                speed *= Mathf.Pow(drag, timeProvider.DeltaTime);

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
                position += Speed * timeProvider.DeltaTime;
            }

            position = screenEdgeHelper.GetScreenWrappedPosition(position);

            return position;
        }

        private Vector2 GetDirection(float rotation)
        {
            float radians = rotation * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var gameEntity = collision.GetComponentInParent<IGameEntity>();
            if (gameEntity != null && gameEntity.EntityType == GameEntityType.Asteroid)
            {
                gameController.OnPlayerHit(gameEntity as Asteroid);
            }
        }

        
    }
}
