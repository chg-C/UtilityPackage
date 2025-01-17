using UnityEngine;

namespace CHG.Utilities
{
    public class SelfDeactivator : MonoBehaviour
    {
        [SerializeField, Tooltip("Deactive Delay")]
        float delay = 0;
        public float Delay
        {
            get => delay;
            set => delay = value;
        }


        private void Update() {
            delay -= Time.deltaTime;
            if(delay <= 0f)
            {
                delay = 0;
                Deactivate();
            }
        }
        public void Deactivate()
        {
            this.gameObject.SetActive(false);
        }
    }
}