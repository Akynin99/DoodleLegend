namespace DoodleLegend.PlayerInput
{
    public static class InputHandlerFactory 
    {
        public static IInputHandler Create() 
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return new KeyboardInputHandler();
#elif UNITY_WEBGL
        return WebGLUtils.IsMobile() ? new MobileInputHandler() : new KeyboardInputHandler();
#else
        return new MobileInputHandler();
#endif
        }
    }
}