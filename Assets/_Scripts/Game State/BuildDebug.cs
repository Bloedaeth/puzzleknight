using UnityEngine;

namespace GameLogging
{
    public class BuildDebug
    {
        public static void Log(string msg, bool debugOnly = false)
        {
            if(Application.isEditor || (debugOnly && !Debug.isDebugBuild))
                return;

            Debug.Log("GAME LOG: " + msg);
        }
    }
}
