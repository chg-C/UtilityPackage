using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using CHG.Utilities.Patterns;
using UnityEngine.InputSystem;
using CHG.Utilities.Attribute;

namespace CHG.Utilities.Input
{
           public class GlobalInputManager : SingletonMonobehaviour<GlobalInputManager>
    {
        #region Input System Actions
        [Header("Unity Input System")]
        [SerializeField, Tooltip("Manager와 연동될 Action Asset"), Required(true)]
        InputActionAsset actionAsset;

        [SerializeField, Tooltip("Global Input Manager와 연동될 Action Map의 이름"), ConditionalHide("actionAsset")]
        string actionMapName = "Player";
        
        InputActionMap playerActionMap;

        private void SyncActions(InputActionAsset actionAsset)
        {
            playerActionMap = actionAsset.FindActionMap(actionMapName);
            if(playerActionMap == null)
            {
                Debug.LogError("There is No Action Map Named As " + actionMapName);
                return;
            }

            playerActionMap.Enable();

            foreach(var action in playerActionMap.actions)
            {
                if(action.type == InputActionType.Button)
                {
                    if(!buttonContainers.ContainsKey(action.name))
                    {
                        buttonContainers.Add(action.name, new ActionContainer<bool>(action.name));
                    }

                    action.performed += context => OnButtonEvent(action.name, true);
                    action.canceled += context => OnButtonEvent(action.name, false);
                }
            }
            
            InputAction moveAction = playerActionMap.FindAction("Move");
            if(moveAction != null)
            {
                moveAction.performed += context => Move(context.ReadValue<Vector2>());
                moveAction.canceled += context => Stop();
            }
        }
        #endregion

        #region Action Containers
        Dictionary<string, ActionContainer<bool>> buttonContainers = new Dictionary<string, ActionContainer<bool>>();

        /// <summary>
        /// Movement Container.
        /// </summary>
        ActionContainer<Vector2> moveContainer = new ActionContainer<Vector2>("Move");
        bool isMoving = false;
        Vector2 movement;
        #endregion
            
        #region Button Events
        void OnButtonEvent(string actionName, bool pressed)
        {
            buttonContainers[actionName]?.InvokeAll(pressed);
        }
        /// <summary>
        /// Input Event 대신 직접적으로 Action 발생시키기
        /// </summary>
        /// <param name="eventName">발동시킬 Event 이름</param>
        /// <param name="pressed">true면 Down, false면 Up 판정</param>
        public void ButtonInput(string eventName, bool pressed)
        {
            OnButtonEvent(eventName, pressed);
        }

        /// <summary>
        /// boolean Input Event 발생시 발동될 Action 등록하기
        /// </summary>
        /// <param name="eventName">연결할 Event 이름</param>
        /// <param name="action">bool 인자를 받는 Action, true면 Down, false면 Up 판정</param>
        public void RegisterButtonEvent(string eventName, UnityAction<bool> action)
        {
            buttonContainers[eventName]?.AddAction(action);
        }
        /// <summary>
        /// 등록된 boolean Input Event Action을 해제하기
        /// </summary>
        /// <param name="eventName">연결 해제할 Event 이름</param>
        /// <param name="action">연결된 Action</param>
        public void UnregisterButtonEvent(string eventName, UnityAction<bool> action)
        {
            buttonContainers[eventName]?.RemoveAction(action);
        }
        #endregion
        
        #region Vector2 Events
        void Move(Vector2 movement)
        {
            isMoving = true;
            this.movement = movement;
        }
        void Stop()
        {
            isMoving = false;
            movement = Vector2.zero;
            moveContainer.InvokeAll(movement);
        }
        
        /// <summary>
        /// Input Event 대신 직접적으로 Move Action 발생시키기
        /// </summary>
        /// <param name="movement">Input Vector, X/Y값은 -1~1 사이</param>
        public void MoveInput(Vector2 movement)
        {
            Move(movement);
        }
        /// <summary>
        /// Vector2 Input Event 발생시 발동될 Action 등록하기
        /// </summary>
        /// <param name="moveAction">Vector2 인자를 받는 Action, X축 좌 -1 우 1 / Y축 상 1 하 -1</param>
        public void RegisterMovementEvent(UnityAction<Vector2> moveAction)
        {
            moveContainer.AddAction(moveAction);
        }
        /// <summary>
        /// 등록된 Vector2 Input Event Action을 해제하기
        /// </summary>
        /// <param name="moveAction">해제할 Action</param>
        public void UnregisterMovementEvent(UnityAction<Vector2> moveAction)
        {
            moveContainer.RemoveAction(moveAction);
        }
        #endregion

        private void Awake()
        {
            SetAction();
        }
        void SetAction()
        {
            if(actionAsset != null)
            {
                if(actionMapName == string.Empty)
                {
                    actionMapName = "Player";
                }
                SyncActions(actionAsset);
            }
        }
        void FixedUpdate()
        {
            if(isMoving)
            {
                moveContainer.InvokeAll(movement);
            }
        }
    }
}