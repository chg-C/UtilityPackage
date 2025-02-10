
using UnityEngine;

namespace CHG.Utilities.Patterns 
{
    //Init Interface
    public interface IInitializable
    {
        public void Init();
    }
    public interface ISingleton<T>
    {
        public static T Instance {get;}
        public bool IsPersistent {get;}
    }

    
    /// <summary>
    /// Singleton Pattern 베이스 클래스
    /// </summary>
    /// <typeparam name="T">Singleton Pattern을 적용할 Monobehaviour</typeparam>
    public abstract class SingletonMonobehaviour<T> : MonoBehaviour, IInitializable, ISingleton<T> 
                                            where T : MonoBehaviour, IInitializable, ISingleton<T>
    {
        private static T instance;
        private static readonly object _lock = new object();
        private static bool isShutting = false;

        /// <summary>
        /// 이 Singleton 클래스가 Scene이 바뀌어도 유지되는지 여부, 기본 true. 변경하고 싶다면 override해서 사용할 것.
        /// </summary>
        public virtual bool IsPersistent => true;

        /// <summary>
        /// Singleton 객체
        /// </summary>
        public static T Instance
        {
            get
            {
                if(isShutting)
                {
                    Debug.LogError(typeof(T) + " - 어플리케이션 종료중 호출되었습니다.");
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
                            instance.Init();

                            singletonObj.name = "_Singleton_"+typeof(T).ToString();
                            
                            if(instance.IsPersistent == true)
                                DontDestroyOnLoad(singletonObj);
                        }
                        else
                        {
                            if(instance.IsPersistent)
                            {
                                if(instance.transform.parent != null)
                                    instance.transform.parent = null;

                                    DontDestroyOnLoad(instance.gameObject);
                            }
                        }
                    }
                }

                return instance;
            }
        }

        protected virtual void Start()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        

        /// <summary>
        /// Instance 생성시 확실하게 호출되는 초기화 함수 <br/>
        /// 해당 기능이 필요하다면 재정의해서 사용할 것
        /// </summary>
        public virtual void Init()
        {

        }

        /// <summary>
        /// 현재 Singleton 객체의 유효성 확인 <br/>
        /// 사용 가능하다면 true, 프로그램이 종료중이거나 현재 Instance가 아직 존재하지 않는다면 false
        /// </summary>
        public static bool IsAvailable {
            get {
                if(isShutting || instance == null)
                    return false;

                return true;
            }
        }
        private void OnDestroy() {
            if(!isShutting)
            {
                instance = null;
            }
        }
        //
        private void OnApplicationQuit() {
            isShutting = true;
            instance = null;
        }
    }
}
        