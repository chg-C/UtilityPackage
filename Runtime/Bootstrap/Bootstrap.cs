using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CHG.Utilities.Bootstrap
{
    /// <summary>
    /// Bootstrap 설정 클래스
    /// BootstrapProfile을 사용해서 프로필 안에 있는 Prefab들을 게임 시작 전에 로딩
    /// </summary>
    public sealed class Bootstrap : ScriptableObject
    {
        public const string SettingPath = "Bootstrap";
        public const string SettingName = "BootstrapSetting";

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
            return Resources.Load<Bootstrap>(SettingPath+"/"+SettingName);
        }
    }
}
