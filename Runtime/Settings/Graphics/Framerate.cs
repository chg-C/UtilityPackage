using UnityEngine;

namespace CHG.Utilities.Settings.Graphics
{
    /// <summary>
    /// Framerate 설정 관련
    /// </summary>
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