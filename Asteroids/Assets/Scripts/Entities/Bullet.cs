using Asteroids.Configurations;
using Asteroids.Utilities;
using UnityEngine;
using VContainer;

namespace Asteroids.Entities
{
    /// <summary>
    /// Component handling bullet related logic.
    /// </summary>
    public class Bullet : MonoBehaviour, IBullet
    {
        private IGameController gameController;
        private ITimeProvider timeProvider;
        private IScreenEdgeHelper screenEdgeHelper;
        private IGameSettings gameSettings;

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
        private void Construct(IGameController gameController, ITimeProvider timeProvider,
            IScreenEdgeHelper screenEdgeHelper, IGameSettings gameSettings)
        {
            this.gameController = gameController
                ?? throw new System.ArgumentNullException(nameof(gameController), $"{nameof(Bullet)} requires reference to {nameof(IGameController)}.");

            this.timeProvider = timeProvider
                ?? throw new System.ArgumentNullException(nameof(timeProvider), $"{nameof(Bullet)} requires reference to {nameof(ITimeProvider)}.");

            this.screenEdgeHelper = screenEdgeHelper
                ?? throw new System.ArgumentNullException(nameof(screenEdgeHelper), $"{nameof(Bullet)} requires reference to {nameof(ScreenEdgeHelper)}.");

            this.gameSettings = gameSettings;
            if (gameSettings == null)
            {
                throw new System.ArgumentNullException(nameof(gameSettings), $"{nameof(Bullet)} requires reference to {nameof(IGameSettings)}.");
            }
        }

        public void SetPositionAndDirection(Vector2 position, Vector2 direction)
        {
            transform.SetPositionAndRotation(position, transform.rotation);
            Speed = gameSettings.BulletSettings.Speed * direction;

            createTime = timeProvider.Time;
        }

        public void Update()
        {
            if (IsMaxLifeTimeReached(timeProvider.Time))
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
                position += Speed * timeProvider.DeltaTime;
            }

            position = screenEdgeHelper.GetScreenWrappedPosition(position);

            return position;
        }

        private bool IsMaxLifeTimeReached(float currentTime)
        {
            if (currentTime - createTime >= gameSettings.BulletSettings.Lifetime)
            {
                return true;
            }
            return false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var gameEntity = collision.GetComponentInParent<IGameEntity>();
            if (gameEntity != null && gameEntity.EntityType == GameEntityType.Asteroid)
            {
                gameController.OnAsteroidHit(this, gameEntity as IAsteroid);
            }
        }
    }
}
