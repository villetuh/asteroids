using UnityEngine;

namespace Asteroids.Utilities
{
    /// <summary>
    /// Interface for components handling screen edge wrapping logic.
    /// </summary>
    public interface IScreenEdgeHelper
    {
        /// <summary>
        /// Updates the position to be wrapped around the screen edges if needed.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 GetScreenWrappedPosition(Vector2 position);
    }
}