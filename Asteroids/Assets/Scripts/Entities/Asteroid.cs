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
    public class Asteroid : MonoBehaviour, IGameEntity
    {
        private IGameController gameController;

        private readonly float asteroidSpeed = 10.0f; // units per second

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
        private void Construct(IGameController gameController)
        {
            this.gameController = gameController
                ?? throw new System.ArgumentNullException(nameof(gameController), $"{nameof(Asteroid)} requires reference to {nameof(IGameController)}.");
        }

        public void SetSizePositionAndDirection(AsteroidSize size, Vector2 position, Vector2 direction)
        {
            Size = size;
            transform.SetPositionAndRotation(position, transform.rotation);
            Speed = asteroidSpeed * direction;
        }

        public void Update()
        {
            var position = GetUpdatedPosition(Speed);

            transform.SetPositionAndRotation(position, transform.rotation);
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
    }
}
