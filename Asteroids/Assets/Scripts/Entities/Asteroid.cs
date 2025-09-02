using Asteroids.Configurations;
using Asteroids.Utilities;
using UnityEngine;
using VContainer;

namespace Asteroids.Entities
{
    /// <summary>
    /// Sizes of asteroids.
    /// </summary>
    public enum AsteroidSize
    {
        Undefined = -1,
        Large = 0,
        Medium = 1,
        Small = 2,
    }

    /// <summary>
    /// Component handling asteroid related logic.
    /// </summary>
    public class Asteroid : MonoBehaviour, IAsteroid
    {
        private ITimeProvider timeProvider;
        private IScreenEdgeHelper screenEdgeHelper;
        private IGameSettings gameSettings;

        private RotationDirection rotationDirection = RotationDirection.None;

        public GameEntityType EntityType => GameEntityType.Asteroid;

        public float Rotation
        {
            get { return transform.rotation.z; }
        }

        public Vector2 Position
        {
            get { return (Vector2)transform.position; }
        }

        public Vector2 Speed { get; private set; }

        public AsteroidSize Size { get; set; } = AsteroidSize.Undefined;

        [Inject]
        private void Construct(ITimeProvider timeProvider, IScreenEdgeHelper screenEdgeHelper,
                               IGameSettings gameSettings)
        {
            this.timeProvider = timeProvider
                ?? throw new System.ArgumentNullException(nameof(timeProvider), $"{nameof(Asteroid)} requires reference to {nameof(ITimeProvider)}.");

            this.screenEdgeHelper = screenEdgeHelper
                ?? throw new System.ArgumentNullException(nameof(screenEdgeHelper), $"{nameof(Asteroid)} requires reference to {nameof(ScreenEdgeHelper)}.");

            this.gameSettings = gameSettings;
            if (gameSettings == null)
            {
                throw new System.ArgumentNullException(nameof(gameSettings), $"{nameof(Asteroid)} requires reference to {nameof(IGameSettings)}.");
            }
        }

        public void SetSizePositionAndDirection(AsteroidSize size, Vector2 position, Vector2 direction)
        {
            Size = size;
            transform.SetPositionAndRotation(position, transform.rotation);
            Speed = gameSettings.AsteroidSettings.Speed * direction;
            rotationDirection = (Random.value > 0.5f) ? RotationDirection.Left : RotationDirection.Right;
        }

        public void Update()
        {
            var rotation = GetUpdatedRotation(rotationDirection);
            var position = GetUpdatedPosition(Speed);

            transform.SetPositionAndRotation(position, Quaternion.Euler(Vector3.forward * rotation));
        }

        private float GetUpdatedRotation(RotationDirection rotationDirection)
        {
            var rotation = transform.rotation.eulerAngles.z;

            if (rotationDirection != RotationDirection.None)
            {
                var rotationDirectionValue = rotationDirection == RotationDirection.Left ? 1.0f : -1.0f;
                rotation += (float)rotationDirectionValue * gameSettings.AsteroidSettings.RotationSpeed * timeProvider.DeltaTime;
                rotation = rotation % 360.0f;
            }

            return rotation;
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
    }
}
