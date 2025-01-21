using UnityEngine;

namespace CHG.Utilities.Settings.Graphics
{
    public class Framerate : MonoBehaviour
    {
        [SerializeField]
        int targetFramerate;
        [SerializeField]
        bool useVSync;
        
        void UpdateFramerate()
        {
            Application.targetFrameRate = targetFramerate;
            QualitySettings.vSyncCount = useVSync ? 1 : 0;
        }
    }
}