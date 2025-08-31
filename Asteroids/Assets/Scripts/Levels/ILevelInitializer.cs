namespace Asteroids.Levels
{
    /// <summary>
    /// Interface for components handling level creation.
    /// </summary>
    public interface ILevelInitializer
    {
        /// <summary>
        /// Creates level with related game objects.
        /// </summary>
        /// <returns></returns>
        public Level CreateLevel();
    }
}
