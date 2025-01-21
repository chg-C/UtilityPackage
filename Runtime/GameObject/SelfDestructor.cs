
    using UnityEngine;
    namespace CHG.Utilities
    {
        public class SelfDestructor : MonoBehaviour
        {
            [SerializeField, Tooltip("Self Destructor가 활성화되는 순간 Destroy 발생")]
            bool destroyOnAwake;
            [SerializeField, Tooltip("Destroy Delay")]
            float delay = 0;

            void Awake() 
            {
                if(destroyOnAwake)
                {
                    Destroy(this.gameObject, delay);
                }
            }

            public void SelfDestroy()
            {
                Destroy(this.gameObject, delay);
            }
            public void ImmidiateSelfDestroy()
            {
                Destroy(this.gameObject);
            }
            public void DestroyChild(string name)
            {
                Transform child = transform.Find(name);
                if(child != null)
                    Destroy(child.gameObject);
            }
        }
    }
        