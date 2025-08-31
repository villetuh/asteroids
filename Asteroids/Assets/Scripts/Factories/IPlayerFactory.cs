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

        /// <summary>
        /// Destroys player created by the factory.
        /// </summary>
        /// <param name="player"></param>
        public void DestroyPlayer(Player player);
    }
}
