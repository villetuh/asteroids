using Asteroids.Entities;

namespace Asteroids.Factories
{
    /// <summary>
    /// Interface for components creating players.
    /// </summary>
    public interface IPlayerFactory
    {
        /// <summary>
        /// Creates a player.
        /// </summary>
        /// <returns></returns>
        public Player CreatePlayer();
    }
}
