using UnityEngine;

namespace Asteroids.Entities
{
    /// <summary>
    /// Component handling player related logic.
    /// </summary>
    public class Player : MonoBehaviour, IPlayerActions, IGameEntity
    {
        private PlayerRotateDirection rotationDirection = PlayerRotateDirection.None;
        private readonly float rotationSpeed = 180.0f; // degrees per second

        private bool thrusting = false;
        private readonly float thrustPower = 2.5f; // units per second
        private readonly float drag = 0.33f; // velocity retention per second

        public GameEntityType EntityType => GameEntityType.Player;

        public float Rotation
        {
            get { return transform.rotation.z; }
        }

        public Vector2 Position
        {
            get { return (Vector2)transform.position; }
        }

        public Vector2 Speed { get; private set; }

        public void Rotate(PlayerRotateDirection direction)
        {
            rotationDirection = direction;
        }
        
        public void Thrust(bool thrust)
        {
            thrusting = thrust;
        }
        
        public void Fire()
        {
            // Implement firing logic here
            Debug.Log($"{nameof(Player)}: Firing");
        }

        public void Update()
        {
            var rotation = GetUpdatedRotation(rotationDirection);
            var speed = GetUpdatedSpeed(rotation, thrusting);
            var position = GetUpdatedPosition(speed);

            Speed = speed;
            transform.SetPositionAndRotation(position, Quaternion.Euler(Vector3.forward * rotation));
        }

        private float GetUpdatedRotation(PlayerRotateDirection rotationDirection)
        {
            var rotation = transform.rotation.eulerAngles.z;

            if (rotationDirection != PlayerRotateDirection.None)
            {
                var rotationDirectionValue = rotationDirection == PlayerRotateDirection.Left ? 1.0f : -1.0f;
                rotation += (float)rotationDirectionValue * rotationSpeed * Time.deltaTime;
                rotation = rotation % 360.0f;
            }

            return rotation;
        }

        private Vector2 GetUpdatedSpeed(float rotation, bool thrusting)
        {
            var speed = Speed;
            if (thrusting)
            {
                float radians = rotation * Mathf.Deg2Rad;
                var thrustVector = thrustPower * new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
                speed += thrustVector * Time.deltaTime;

                if (speed.magnitude > thrustPower)
                {
                    speed = speed.normalized * thrustPower;
                }
            }
            else
            {
                speed *= Mathf.Pow(drag, Time.deltaTime);

                if (speed.magnitude < 0.01f)
                {
                    speed = Vector2.zero;
                }
            }

            return speed;
        }

        private Vector2 GetUpdatedPosition(Vector2 speed)
        {
            var position = (Vector2)transform.position;
            if (speed != Vector2.zero)
            {
                position += Speed * Time.deltaTime;
            }
            return position;
        }
    }
}
