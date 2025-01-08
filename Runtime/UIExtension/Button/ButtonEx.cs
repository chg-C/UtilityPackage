using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CHG.Utilities.UI {
    public class ButtonEx : Button
    {   
        bool isDown = false;
        bool isFocused = false;

        [SerializeField, Tooltip("버튼이 클릭되었을 때 호출되는 이벤트")]
        UnityEvent onPointerDownEvent = new UnityEvent();
        [SerializeField, Tooltip("클릭된 버튼에서 손을 떼면 호출되는 이벤트")]
        UnityEvent onPointerUpEvent = new UnityEvent();

        protected override void Start() {
            base.Start();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            onPointerDownEvent.Invoke();
            isDown = true;
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            
            if(isDown && isFocused)
            {
                onPointerUpEvent.Invoke();
                isDown = false;
            }
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            isFocused = true;
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            isFocused = false;
        }
    }
}