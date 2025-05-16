namespace DoodleLegend.PlayerInput
{
    public interface IInputHandler 
    {
        float GetHorizontal();
        bool IsJumpPressed();
        bool IsPowerUpActivated(); 
    }
}