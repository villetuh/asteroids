namespace Asteroids.Entities
{
    /// <summary>
    /// Rotation direction for the player rotate action.
    /// </summary>
    public enum PlayerRotateDirection
    {
        Left = -1,
        None = 0,
        Right = 1
    }

    /// <summary>
    /// Interface for actions that the player can perform.
    /// </summary>
    public interface IPlayerActions
    {
        public void Rotate(PlayerRotateDirection direction);

        public void Thrust(bool thrust);

        public void Fire();
    }
}
