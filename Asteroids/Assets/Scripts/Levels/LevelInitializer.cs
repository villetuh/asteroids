using Asteroids.Factories;
using System;

namespace Asteroids.Levels
{
    /// <summary>
    /// Component handling level creation.
    /// </summary>
    public class LevelInitializer : ILevelInitializer
    {
        private readonly IPlayerFactory playerFactory;

        public LevelInitializer(IPlayerFactory playerFactory)
        {
            this.playerFactory = playerFactory
                ?? throw new ArgumentNullException(nameof(playerFactory), $"{nameof(LevelInitializer)} requires reference to {nameof(IPlayerFactory)}.");
        }

        /// <inheritdoc cref="ILevelInitializer.CreateLevel" />
        public Level CreateLevel()
        {
            var player = playerFactory.CreatePlayer();

            return new Level(player);
        }
    }
}
