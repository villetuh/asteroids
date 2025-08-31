using System;
using UnityEngine;

namespace Asteroids.Configurations
{
    [Serializable]
    public class LevelSettings
    {
        [field: SerializeField]
        [Tooltip("Time to wait before spawning a next asteroid.")]
        public float InitialAsteroidSpawnInterval { get; set; } = 5.0f;

        [field: SerializeField]
        [Tooltip("Time limit on how short the wait before spawning a next asteroid can be.")]
        public float MinAsteroidSpawnInterval { get; set; } = 1.0f;

        [field: SerializeField]
        [Tooltip("Time reduction per asteroid spawned.")]
        public float AsteroidSpawnIntervalReductionStep { get; set; } = 0.05f;
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

    [Serializable]
    public class ScoreSettings
    {
        [field: SerializeField]
        [Tooltip("Score granted for hitting a large asteroid.")]
        public int LargeAsteroidScore { get; set; } = 10;

        [field: SerializeField]
        [Tooltip("Score granted for hitting a medium asteroid.")]
        public int MediumAsteroidScore { get; set; } = 30;

        [field: SerializeField]
        [Tooltip("Score granted for hitting a small asteroid.")]
        public int SmallAsteroidScore { get; set; } = 50;
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

        [SerializeField]
        private ScoreSettings scoreSettings = new();
        /// <inheritdoc cref="IGameSettings.ScoreSettings" />
        public ScoreSettings ScoreSettings => scoreSettings;
    }
}
