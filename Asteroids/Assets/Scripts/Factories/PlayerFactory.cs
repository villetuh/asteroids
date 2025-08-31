using Asteroids.Entities;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Asteroids.Factories
{
    /// <summary>
    /// Factory creating player instances.
    /// </summary>
    public class PlayerFactory : IPlayerFactory
    {
        private readonly IObjectResolver container;
        private readonly Player playerPrefab;

        public PlayerFactory(IObjectResolver container, [Key("PlayerPrefab")] Player playerPrefab)
        {
            this.container = container
                ?? throw new ArgumentNullException(nameof(container), $"{nameof(PlayerFactory)} requires reference to {nameof(IObjectResolver)}.");

            this.playerPrefab = playerPrefab;
            if (playerPrefab == null)
            {
                throw new ArgumentNullException(nameof(playerPrefab), $"{nameof(PlayerFactory)} requires reference to {nameof(Player)} prefab.");
            }
                
        }

        /// <inheritdoc cref="IPlayerFactory.CreatePlayer" />
        public Player CreatePlayer()
        {
            return container.Instantiate(playerPrefab);
        }

        /// <inheritdoc cref="IPlayerFactory.DestroyPlayer(Player)" />
        public void DestroyPlayer(Player player)
        {
            if (player != null)
            {
                GameObject.Destroy(player.gameObject);
            }
        }
    }
}
