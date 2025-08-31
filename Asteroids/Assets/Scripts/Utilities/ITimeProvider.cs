namespace Asteroids.Utilities
{
    /// <summary>
    /// Interface for components providing time information.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Current time
        /// </summary>
        public float Time { get; }

        /// <summary>
        /// Frame delta time
        /// </summary>
        public float DeltaTime { get; }
    }
}
