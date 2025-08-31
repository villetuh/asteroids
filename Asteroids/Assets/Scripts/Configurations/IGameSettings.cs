namespace Asteroids.Configurations
{
    /// <summary>
    /// Interface for components providing game related settings.
    /// </summary>
    public interface IGameSettings
    {
        /// <summary>
        /// Settings related to level.
        /// </summary>
        public LevelSettings LevelSettings { get; }

        /// <summary>
        /// Settings related to player.
        /// </summary>
        public PlayerSettings PlayerSettings { get; }

        /// <summary>
        /// Settings related to bullets.
        /// </summary>
        public BulletSettings BulletSettings { get; }

        /// <summary>
        /// Settings related to asteroids.
        /// </summary>
        public AsteroidSettings AsteroidSettings { get; }
    }
}