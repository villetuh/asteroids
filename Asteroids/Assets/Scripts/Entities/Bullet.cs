using UnityEngine;
using VContainer;

namespace Asteroids.Entities
{
    /// <summary>
    /// Component handling bullet related logic.
    /// </summary>
    public class Bullet : MonoBehaviour, IGameEntity
    {
        private IGameController gameController;

        private readonly float bulletSpeed = 10.0f; // units per second
        private readonly float bulletLifetime = 3.0f; // seconds

        private float createTime = 0.0f;

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

        [Inject]
        private void Construct(IGameController gameController)
        {
            this.gameController = gameController
                ?? throw new System.ArgumentNullException(nameof(gameController), $"{nameof(Bullet)} requires reference to {nameof(IGameController)}.");
        }

        public void SetPositionAndDirection(Vector2 position, Vector2 direction)
        {
            transform.SetPositionAndRotation(position, transform.rotation);
            Speed = bulletSpeed * direction;

            createTime = Time.time;
        }

        public void Update()
        {
            if (IsMaxLifeTimeReached(Time.time))
            {
                gameController.OnBulletExpired(this);
                return;
            }

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

        private bool IsMaxLifeTimeReached(float currentTime)
        {
            if (currentTime - createTime >= bulletLifetime)
            {
                return true;
            }
            return false;
        }
    }
}
