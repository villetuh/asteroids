namespace Asteroids.Utilities
{
    /// <summary>
    /// Time provider using UnityEngine.Time as source.
    /// </summary>
    public class TimeProvider : ITimeProvider
    {
        /// <inheritdoc cref="ITimeProvider.Time" />
        public float Time => UnityEngine.Time.time;

        /// <inheritdoc cref="ITimeProvider.DeltaTime" />
        public float DeltaTime => UnityEngine.Time.deltaTime;
    }
}
