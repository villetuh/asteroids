using Asteroids.Entities;
using System;
using System.Collections.Generic;

namespace Asteroids.Levels
{
    /// <summary>
    /// Data related to a level.
    /// </summary>
    public class Level
    {
        public Player Player { get; private set; }

        public List<Bullet> Bullets { get; private set; } = new List<Bullet>();

        public Level(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player), $"{nameof(Level)} requires reference to {nameof(Player)}.");
            }
            Player = player;
        }

        public void AddBullet(Bullet bullet)
        {
            if (bullet == null)
            {
                return;
            }
            Bullets.Add(bullet);
        }

        public void RemoveBullet(Bullet bullet)
        {
            if (bullet == null)
            {
                return;
            }
            Bullets.Remove(bullet);
        }
    }
}
