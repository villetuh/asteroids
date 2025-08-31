using Asteroids.Entities;
using Asteroids.Factories;
using Asteroids.Levels;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Asteroids.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Prefab references")]
        [SerializeField] private Player playerPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            // Prefabs
            builder.RegisterInstance(playerPrefab).Keyed("PlayerPrefab");

            // Factories
            builder.Register<PlayerFactory>(Lifetime.Singleton).As<IPlayerFactory>();

            // Game logic
            builder.RegisterEntryPoint<GameController>(Lifetime.Singleton).As<IGameController>();
            builder.Register<LevelInitializer>(Lifetime.Singleton).As<ILevelInitializer>();
        }
    }
}
