using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using CHG.Utilities.Patterns;
using UnityEngine.InputSystem;

namespace CHG.Utilities.Input
{
    public class GlobalInputManager : SingletonMonobehaviour<GlobalInputManager>
    {
        #region Input System Actions
        [SerializeField]
        InputActionAsset actionAsset;
        [SerializeField]
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
        public void ButtonInput(string actionName, bool pressed)
        {
            OnButtonEvent(actionName, pressed);
        }
        public void RegisterButtonEvent(string name, UnityAction<bool> action)
        {
            buttonContainers[name]?.AddAction(action);
        }
        public void UnregisterButtonEvent(string name, UnityAction<bool> action)
        {
            buttonContainers[name]?.RemoveAction(action);
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
        
        public void MoveInput(Vector2 movement)
        {
            Move(movement);
        }
        public void RegisterMovementEvent(UnityAction<Vector2> moveAction)
        {
            moveContainer.AddAction(moveAction);
        }
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