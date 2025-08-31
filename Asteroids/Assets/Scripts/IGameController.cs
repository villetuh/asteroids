using Asteroids.Entities;
using UnityEngine;

namespace Asteroids
{
    /// <summary>
    /// Interface for game controller handling main game logic.
    /// </summary>
    public interface IGameController
    {
        /// <summary>
        /// Event invoked when player performs a fire action.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public void OnPlayerFire(Vector2 position, Vector2 direction);

        /// <summary>
        /// Event invoked when bullet reaches its lifetime without hitting anything.
        /// </summary>
        /// <param name="bullet"></param>
        public void OnBulletExpired(Bullet bullet);

        /// <summary>
        /// Event invoked when bullet hits an asteroid.
        /// </summary>
        /// <param name="bullet"></param>
        /// <param name="asteroid"></param>
        public void OnAsteroidHit(Bullet bullet, Asteroid asteroid);
    }
}
