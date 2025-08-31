using System;
using UnityEngine;

namespace Asteroids.Utilities
{
    /// <summary>
    /// Component used to determine position and direction of objects
    /// spawned on the edge of the screen.
    /// </summary>
    public class ScreenEdgeSpawner
    {
        private readonly Camera camera;

        public ScreenEdgeSpawner(Camera camera)
        {
            this.camera = camera;
            if (camera == null)
            {
                throw new ArgumentNullException(nameof(camera), $"{nameof(ScreenEdgeSpawner)} requires reference to {nameof(Camera)}.");
            }
        }

        /// <summary>
        /// Gets a random point just off the edge of the screen in world coordinates and direction towards the center of the screen.
        /// </summary>
        /// <returns></returns>
        public (Vector2, Vector2) GetScreenEdgeSpawnPointAndDirection(float additionalPadding)
        {
            // Get normalized viewport coordinates on an edge
            int screenEdge = UnityEngine.Random.Range(0, 4);
            float t = UnityEngine.Random.value;
            Vector3 viewportCoordinates = screenEdge switch
            {
                0 => new Vector3(0.0f, t, 0.0f), // left
                1 => new Vector3(1.0f, t, 0.0f), // right
                2 => new Vector3(t, 0.0f, 0.0f), // bottom
                _ => new Vector3(t, 1.0f, 0.0f), // top
            };

            // Get matching world coordinates on the gameplay plane
            float zDistance = -camera.transform.position.z;
            Vector2 worldPosition = camera.ViewportToWorldPoint(new Vector3(viewportCoordinates.x, viewportCoordinates.y, zDistance));

            worldPosition += screenEdge switch
            {
                0 => Vector2.left * additionalPadding,
                1 => Vector2.right * additionalPadding,
                2 => Vector2.down * additionalPadding,
                _ => Vector2.up * additionalPadding,
            };

            Vector2 targetPosition = camera.ViewportToWorldPoint(new Vector3(UnityEngine.Random.Range(0.4f, 0.6f), UnityEngine.Random.Range(0.4f, 0.6f), zDistance));
            var direction = (targetPosition - worldPosition).normalized;

            return (worldPosition, direction);
        }
    }
}
