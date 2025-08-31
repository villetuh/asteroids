using Asteroids.Entities;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Asteroids.Factories
{
    /// <summary>
    /// Factory creating bullet instances.
    /// </summary>
    public class BulletFactory : IBulletFactory
    {
        private readonly IObjectResolver container;
        private readonly Bullet bulletPrefab;

        public BulletFactory(IObjectResolver container, [Key("BulletPrefab")] Bullet bulletPrefab)
        {
            this.container = container
                ?? throw new ArgumentNullException(nameof(container), $"{nameof(BulletFactory)} requires reference to {nameof(IObjectResolver)}.");

            this.bulletPrefab = bulletPrefab;
            if (bulletPrefab == null)
            {
                throw new ArgumentNullException(nameof(bulletPrefab), $"{nameof(BulletFactory)} requires reference to {nameof(Bullet)} prefab.");
            }
        }

        /// <inheritdoc cref="IBulletFactory.CreateBullet" />
        public Bullet CreateBullet(Vector2 position, Vector2 speed)
        {
            var bullet = container.Instantiate(bulletPrefab);

            bullet.SetPositionAndDirection(position, speed);

            return bullet;
        }
    }
}
