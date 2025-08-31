using Asteroids.Entities;
using Asteroids.Factories;
using Asteroids.Input;
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
            builder.Register<PlayerFactory>(Lifetime.Scoped).As<IPlayerFactory>();

            // Game logic
            builder.RegisterEntryPoint<GameController>(Lifetime.Scoped).As<IGameController>();
            builder.Register<LevelInitializer>(Lifetime.Scoped).As<ILevelInitializer>();

            // Input
            builder.RegisterEntryPoint<PlayerInput>(Lifetime.Scoped).As<IPlayerInput>();
        }
    }
}
