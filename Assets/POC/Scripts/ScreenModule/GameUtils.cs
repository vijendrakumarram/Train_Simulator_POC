using UnityEngine;

namespace EdverseAPI
{

    public static class GameUtils
    {
        public static string DeviceId => SystemInfo.deviceUniqueIdentifier;

        public static int CurrentPlatform
        {
            get
            {
                int platfomId = 0;
#if UNITY_WEBGL
        platfomId = (int)Platform.WEBGL;
#elif UNITY_ANDROID
        platfomId = (int)Platform.ANDROID;
#elif UNITY_STANDALONE_WIN
            platfomId = (int)Platform.DESKTOP;
#elif UNITY_STANDALONE_OSX
            platfomId = (int)Platform.MACOS;
#endif
                return platfomId;
            }
        }

        public enum Platform
        {
            WEBGL = 1,
            ANDROID_VR = 2,
            DESKTOP = 3,
            MACOS = 4
        }
    }
}


