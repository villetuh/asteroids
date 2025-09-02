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
        public IPlayer Player { get; set; }

        public List<IBullet> Bullets { get; set; } = new List<IBullet>();

        public List<IAsteroid> Asteroids { get; set; } = new List<IAsteroid>();

        public void AddBullet(IBullet bullet)
        {
            if (bullet == null)
            {
                return;
            }
            Bullets.Add(bullet);
        }

        public void RemoveBullet(IBullet bullet)
        {
            if (bullet == null)
            {
                return;
            }
            Bullets.Remove(bullet);
        }

        public void AddAsteroid(IAsteroid asteroid)
        {
            if (asteroid == null)
            {
                return;
            }
            Asteroids.Add(asteroid);
        }

        public void RemoveAsteroid(IAsteroid asteroid)
        {
            if (asteroid == null)
            {
                return;
            }
            Asteroids.Remove(asteroid);
        }
    }
}
