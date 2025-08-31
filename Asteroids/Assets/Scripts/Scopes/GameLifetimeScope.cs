using Asteroids.Entities;
using Asteroids.Factories;
using Asteroids.Input;
using Asteroids.Levels;
using Asteroids.Utilities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Asteroids.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Prefab references")]
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Asteroid asteroidLargePrefab;
        [SerializeField] private Asteroid asteroidMediumPrefab;
        [SerializeField] private Asteroid asteroidSmallPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            // Prefabs
            builder.RegisterInstance(playerPrefab).Keyed("PlayerPrefab");
            builder.RegisterInstance(bulletPrefab).Keyed("BulletPrefab");
            builder.RegisterInstance(asteroidLargePrefab).Keyed("AsteroidLargePrefab");
            builder.RegisterInstance(asteroidMediumPrefab).Keyed("AsteroidMediumPrefab");
            builder.RegisterInstance(asteroidSmallPrefab).Keyed("AsteroidSmallPrefab");

            // Scene object references
            builder.RegisterComponentInHierarchy<Camera>();

            // Factories
            builder.Register<PlayerFactory>(Lifetime.Scoped).As<IPlayerFactory>();
            builder.Register<BulletFactory>(Lifetime.Scoped).As<IBulletFactory>();
            builder.Register<AsteroidFactory>(Lifetime.Scoped).As<IAsteroidFactory>();

            // Game logic
            builder.RegisterEntryPoint<GameController>(Lifetime.Scoped).As<IGameController>();
            builder.Register<LevelInitializer>(Lifetime.Scoped).As<ILevelInitializer>();

            // Input
            builder.RegisterEntryPoint<PlayerInput>(Lifetime.Scoped).As<IPlayerInput>();

            // Other components
            builder.Register<ScreenEdgeSpawner>(Lifetime.Scoped);
        }
    }
}
