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

        /// <summary>
        /// Method used to update the level based on current time.
        /// </summary>
        /// <param name="time"></param>
        public void UpdateLevel(float time);
    }
}
