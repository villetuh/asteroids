using Asteroids.Entities;
using UnityEngine;

namespace Asteroids.Factories
{
    /// <summary>
    /// Interface for components creating bullets.
    /// </summary>
    public interface IBulletFactory
    {
        /// <summary>
        /// Creates a bullet.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public Bullet CreateBullet(Vector2 position, Vector2 direction);

        /// <summary>
        /// Destroys bullet created by the factory.
        /// </summary>
        /// <param name="bullet"></param>
        public void DestroyBullet(Bullet bullet);
    }
}