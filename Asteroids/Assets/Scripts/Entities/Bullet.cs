using UnityEngine;

namespace Asteroids.Entities
{
    /// <summary>
    /// Component handling bullet related logic.
    /// </summary>
    public class Bullet : MonoBehaviour, IGameEntity
    {
        private readonly float bulletSpeed = 10.0f; // units per second

        public GameEntityType EntityType => GameEntityType.Bullet;

        public float Rotation
        {
            get { return transform.rotation.z; }
        }

        public Vector2 Position
        {
            get { return (Vector2)transform.position; }
        }

        public Vector2 Speed { get; private set; }

        public void SetPositionAndDirection(Vector2 position, Vector2 direction)
        {
            transform.SetPositionAndRotation(position, transform.rotation);
            Speed = bulletSpeed * direction;
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
