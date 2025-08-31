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
        /// <returns></returns>
        Bullet CreateBullet(Vector2 position, Vector2 direction);
    }
}