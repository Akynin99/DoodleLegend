using UnityEngine;

namespace DoodleLegend.PlayerInput
{
    public static class WebGLUtils 
    {
        public static bool IsMobile() 
        {
            string userAgent = Application.unityVersion; 
            return userAgent.Contains("Mobile") || 
                   userAgent.Contains("Android") || 
                   userAgent.Contains("iOS");
        }
    }
}