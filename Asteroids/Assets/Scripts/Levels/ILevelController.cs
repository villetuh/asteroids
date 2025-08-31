using Asteroids.Entities;
using UnityEngine;

namespace Asteroids.Levels
{
    /// <summary>
    /// Interface for components handling level related logic.
    /// </summary>
    public interface ILevelController
    {
        /// <summary>
        /// Creates level with initial game objects.
        /// </summary>
        /// <returns></returns>
        public void CreateLevel();

        /// <summary>
        /// Clears current level of all game objects.
        /// </summary>
        public void ClearLevel();

        /// <summary>
        /// Method used to update the level based on current time.
        /// </summary>
        /// <param name="time"></param>
        public void UpdateLevel(float time);

        /// <summary>
        /// Update level based on asteroid being hit.
        /// </summary>
        /// <param name="bullet"></param>
        /// <param name="asteroid"></param>
        public void HandleAsteroidHit(Bullet bullet, Asteroid asteroid);

        /// <summary>
        /// Update level based on bullet being fired.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        public void HandleBulletFired(Vector2 position, Vector2 direction);

        /// <summary>
        /// Update level when bullet has expired.
        /// </summary>
        /// <param name="bullet"></param>
        public void HandleBulletExpired(Bullet bullet);

        /// <summary>
        /// Update level when player has been hit by an asteroid.
        /// </summary>
        /// <param name="asteroid"></param>
        public void HandlePlayerHit(Asteroid asteroid);
    }
}
