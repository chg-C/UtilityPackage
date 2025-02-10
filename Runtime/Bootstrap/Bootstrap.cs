using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CHG.Utilities.Bootstrap
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Bootstrap : ScriptableObject
    {
        public const string SettingPath = "Bootstrap";
        public const string DefaultName = "DefaultBootstrap";

        [SerializeField]
        private BootstrapProfile profile = null;

        public BootstrapProfile Profile
        {
            get => profile;
            set => profile = value;
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            Bootstrap setting = FindBootstrap();
            if(setting != null && setting.Profile != null)
            {
                setting.Profile.Init();
            }
        }
        public static Bootstrap FindBootstrap()
        {
            return Resources.Load<Bootstrap>(SettingPath+"/"+DefaultName);
        }
    }
}
