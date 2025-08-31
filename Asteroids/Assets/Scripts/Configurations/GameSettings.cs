using System;
using UnityEngine;

namespace Asteroids.Configurations
{
    [Serializable]
    public class LevelSettings
    {
        [field: SerializeField]
        [Tooltip("Time to wait before spawning a next asteroid.")]
        public float TimeBetweenAsteroidSpawns { get; set; } = 5.0f;
    }

    [Serializable]
    public class PlayerSettings
    {
        [field: SerializeField]
        [Tooltip("Rotation speed of the player in degrees per second.")]
        public float RotationSpeed { get; set; } = 180.0f;

        [field: SerializeField]
        [Tooltip("Thrust power of the player in units per second.")]
        public float ThrustPower { get; set; } = 2.5f;

        [field: SerializeField]
        [Tooltip("Drag applied to the player ship.")]
        public float Drag { get; set; } = 0.33f;

        [field: SerializeField]
        [Tooltip("Offset applied to bullets to place them in front of the player when spawned.")]
        public float BulletOffset { get; set; } = 0.5f;
    }

    [Serializable]
    public class BulletSettings
    {
        [field: SerializeField]
        [Tooltip("Speed of the bullet in units per second.")]
        public float Speed { get; set; } = 10.0f;

        [field: SerializeField]
        [Tooltip("Lifetime of the bullet in seconds.")]
        public float Lifetime { get; set; } = 1.25f;
    }

    [Serializable]
    public class AsteroidSettings
    {
        [field: SerializeField]
        [Tooltip("Rotation speed of the asteroid in degrees per second.")]
        public float RotationSpeed { get; set; } = 20.0f;

        [field: SerializeField]
        [Tooltip("Speed of the asteroids in units per second.")]
        public float Speed { get; set; } = 1.0f;
    }

    /// <summary>
    /// Configuration related to the game play
    /// </summary>
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
    public class GameSettings : ScriptableObject, IGameSettings
    {
        [SerializeField]
        private LevelSettings levelSettings = new();
        /// <inheritdoc cref="IGameSettings.LevelSettings" />
        public LevelSettings LevelSettings => levelSettings;

        [SerializeField]
        private PlayerSettings playerSettings = new();
        /// <inheritdoc cref="IGameSettings.PlayerSettings" />
        public PlayerSettings PlayerSettings => playerSettings;

        [SerializeField]
        private BulletSettings bulletSettings = new();
        /// <inheritdoc cref="IGameSettings.BulletSettings" />
        public BulletSettings BulletSettings => bulletSettings;

        [SerializeField]
        private AsteroidSettings asteroidSettings = new();
        /// <inheritdoc cref="IGameSettings.AsteroidSettings" />
        public AsteroidSettings AsteroidSettings => asteroidSettings;
    }
}
