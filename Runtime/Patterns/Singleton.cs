
using UnityEngine;
namespace CHG.Utilities.Patterns 
{
    /// <summary>
    /// Singleton Pattern 베이스 클래스
    /// </summary>
    /// <typeparam name="T">Singleton Pattern을 적용할 Monobehaviour</typeparam>
    public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static readonly object _lock = new object();
        private static bool isShutting = false;

        /// <summary>
        /// Singleton 객체
        /// </summary>
        public static T Instance
        {
            get
            {
                if(isShutting)
                {
                    Debug.LogError("Singleton Instance " + typeof(T) + " already destroyed.");
                    return null;
                }

                lock(_lock)
                {
                    if(instance == null)
                    {
                        instance = FindAnyObjectByType<T>();
                        if(instance == null)
                        {
                            GameObject singletonObj = new GameObject();
                            instance = singletonObj.AddComponent<T>();

                            singletonObj.name = typeof(T).ToString() + " Singleton";
                            
                            DontDestroyOnLoad(singletonObj);
                        }
                    }
                }

                return instance;
            }
        }
        
        private void OnApplicationQuit() {
            isShutting = true;
        }
        
        /// <summary>
        /// 현재 Singleton 객체의 유효성 확인
        /// <br/>
        /// 사용 가능하다면 true, 프로그램이 종료중이거나 현재 Instance가 아직 존재하지 않는다면 false
        /// </summary>
        public static bool IsAvailable {
            get {
                if(isShutting || instance == null)
                    return false;

                return true;
            }
        }
    }
}
        