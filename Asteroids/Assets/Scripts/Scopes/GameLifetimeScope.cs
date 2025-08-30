using Asteroids.Entities;
using Asteroids.Input;
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
            builder.RegisterEntryPoint<PlayerInput>(Lifetime.Scoped);
            builder.RegisterComponentInNewPrefab<Player>(playerPrefab, Lifetime.Scoped).As<IPlayerActions, IGameEntity>();
        }
    }
}
