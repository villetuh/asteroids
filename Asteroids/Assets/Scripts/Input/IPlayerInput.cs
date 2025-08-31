using Asteroids.Entities;
using System;

namespace Asteroids.Input
{
    /// <summary>
    /// Interface for player input handling.
    /// </summary>
    public interface IPlayerInput
    {
        /// <summary>
        /// Event invoked when rotation related input changes.
        /// </summary>
        public Action<RotationDirection> OnRotate { get; set; }

        /// <summary>
        /// Event invoked when thrust related input changes.
        /// </summary>
        public Action<bool> OnThrust { get; set; }

        /// <summary>
        /// Event invoked when fire input is detected.
        /// </summary>
        public Action OnFire { get; set; }
    }
}
