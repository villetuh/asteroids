using Asteroids.Entities;
using System;

namespace Asteroids.Levels
{
    /// <summary>
    /// Data related to a level.
    /// </summary>
    public class Level
    {
        public Player Player { get; private set; }

        public Level(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player), $"{nameof(Level)} requires reference to {nameof(Player)}.");
            }
            Player = player;
        }
    }
}
