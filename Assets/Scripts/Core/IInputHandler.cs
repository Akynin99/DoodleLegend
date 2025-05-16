namespace DoodleLegend.Core
{
    public interface IInputHandler 
    {
        float GetHorizontal();
        bool IsJumpPressed();
        bool IsPowerUpActivated(); 
    }
}