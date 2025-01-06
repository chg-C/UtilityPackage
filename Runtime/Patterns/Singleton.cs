
using UnityEngine;
namespace CHG.Utilities.Patterns 
{
    public class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static readonly object _lock = new object();
        private static bool isShutting = false;

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
        
        public static bool IsAvailable {
            get {
                if(isShutting || instance == null)
                    return false;

                return true;
            }
        }
    }
}
        