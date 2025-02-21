using System.Collections.Generic;
using UnityEngine;

namespace CHG.Utilities.Bootstrap
{
    [CreateAssetMenu(menuName = "Bootstrap/Profile", fileName = "BootstrapProfile")]
    public class BootstrapProfile : ScriptableObject
    {
        public const string DefaultName = "DefaultProfile";

        [SerializeField, Tooltip("프로그램 시작시 Init이 호출되어 이 리스트에 들어있는 Prefab을 정렬된 순서대로 생성하고 DontDestroyOnLoad 플래그 설정")]
        protected List<GameObject> Prefabs;

        public virtual void Init()
        {
            foreach (var prefab in Prefabs)
            {
                if(prefab != null)
                {
                    var instance = Instantiate(prefab);
                    instance.name = prefab.name;

                    DontDestroyOnLoad(instance);
                }
            }
        }
    }
}