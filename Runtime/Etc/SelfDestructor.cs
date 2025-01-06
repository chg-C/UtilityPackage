
    using UnityEngine;
    namespace CHG.Utilities
    {
        public class SelfDestructor : MonoBehaviour
        {
            [SerializeField]
            bool destroyOnAwake;
            [SerializeField]
            float delay = 0;

            
            SelfDestructor(float delay)
            {
                this.delay = delay;
            }

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
        