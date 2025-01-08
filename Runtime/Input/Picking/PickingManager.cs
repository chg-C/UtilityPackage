using CHG.Utilities.EditorExpansion;
using CHG.Utilities.Patterns;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace CHG.Utilities.Input
{
    [RequireComponent(typeof(Camera))]
    public class PickingManager : SingletonMonobehaviour<PickingManager>
    {
        [SerializeField, Tooltip("Picking을 발생시킬 Event의 이름"), Required]
        string pickingEventName;
        
        Camera mainCam;
        ActionContainer<Ray> pickingActions = new ActionContainer<Ray>("Picking");
        Ray ray;
        
        private void Awake() {
            mainCam = GetComponent<Camera>();
        }
        private void OnEnable() {
            GlobalInputManager.Instance.RegisterButtonEvent(pickingEventName, OnPicking);            
        }
        private void OnDisable() {
            if(GlobalInputManager.IsAvailable)
            {
                GlobalInputManager.Instance.UnregisterButtonEvent(pickingEventName, OnPicking);
            }
        }

        void OnPicking(bool isDown)
        {
            Vector2 inputPosition = Vector2.zero;
            
            #if UNITY_EDITOR
            inputPosition = Mouse.current.position.ReadValue();
            #elif UNITY_ANDROID || UNITY_IPHONE
            inputPosition = Touch.activeTouches[0].screenPosition;
            #endif
            ray = mainCam.ScreenPointToRay(inputPosition);

            pickingActions?.InvokeAll(ray);
        }
        /// <summary>
        /// Ray를 인자값으로 받는 피킹 이벤트 등록하기
        /// </summary>
        /// <param name="pickingAction">Ray를 인자값으로 받는 메서드</param>
        public void RegisterPickingEvent(UnityAction<Ray> pickingAction)
        {
            pickingActions.AddAction(pickingAction);
        }
        /// <summary>
        /// 등록한 피킹 이벤트를 해제하기
        /// </summary>
        /// <param name="pickingAction">Ray를 인자값으로 받는 메서드</param>
        public void UnregisterPickingEvent(UnityAction<Ray> pickingAction)
        {
            pickingActions.RemoveAction(pickingAction);
        }
    }
}