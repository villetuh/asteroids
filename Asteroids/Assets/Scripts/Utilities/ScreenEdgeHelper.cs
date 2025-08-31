using System;
using UnityEngine;

namespace Asteroids.Utilities
{
    /// <summary>
    /// Component used to determine screen wrapping positions.
    /// </summary>
    public class ScreenEdgeHelper : IScreenEdgeHelper
    {
        private readonly Camera camera;

        public ScreenEdgeHelper(Camera camera)
        {
            this.camera = camera;
            if (camera == null)
            {
                throw new ArgumentNullException(nameof(camera), $"{nameof(ScreenEdgeSpawner)} requires reference to {nameof(Camera)}.");
            }
        }

        /// <inheritdoc cref="IScreenEdgeHelper.GetScreenWrappedPosition(Vector2)" />
        public Vector2 GetScreenWrappedPosition(Vector2 position)
        {
            Vector3 viewportCoordinates = camera.WorldToViewportPoint(new Vector3(position.x, position.y, 0.0f));
            if (viewportCoordinates.x < 0.0f)
            {
                viewportCoordinates.x = 1.0f;
            }
            else if (viewportCoordinates.x > 1.0f)
            {
                viewportCoordinates.x = 0.0f;
            }

            if (viewportCoordinates.y < 0.0f)
            {
                viewportCoordinates.y = 1.0f;
            }
            else if (viewportCoordinates.y > 1.0f)
            {
                viewportCoordinates.y = 0.0f;
            }

            float zDistance = -camera.transform.position.z;
            Vector2 worldPosition = camera.ViewportToWorldPoint(new Vector3(viewportCoordinates.x, viewportCoordinates.y, zDistance));
            return worldPosition;
        }
    }
}
