using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CHG.Utilities.Patterns
{
    [Serializable]
    public abstract class BaseActionContainer
    {
        public string name;

        public BaseActionContainer(string name) {this.name = name;}
        public abstract void Clear();
    }
    [Serializable]
    public class ActionContainer : BaseActionContainer
    {
        public ActionContainer(string name) : base(name)
        {
            
        }

        List<UnityAction> actions = new List<UnityAction>();

        public override void Clear()
        {
            actions.Clear();
        }
        public void AddAction(UnityAction action)
        {
            actions.Add(action);
        }
        public void RemoveAction(UnityAction action)
        {
            actions.Remove(action);
        }

        public void InvokeAll()
        {
            for(int i = 0; i < actions.Count;)
            {
                try
                {
                    actions[i].Invoke();
                    ++i;
                }
                catch(Exception ex)
                {
                    actions.RemoveAt(i);
                    Debug.LogError(ex);
                }
            }
        }

        
    }
    [Serializable]
    public class ActionContainer<T> : BaseActionContainer
    {
        List<UnityAction<T>> actions = new List<UnityAction<T>>();

        public ActionContainer(string name) : base(name)
        {
        }

        public override void Clear()
        {
            actions.Clear();
        }
        public void AddAction(UnityAction<T> action)
        {
            actions.Add(action);
        }
        public void RemoveAction(UnityAction<T> action)
        {
            actions.Remove(action);
        }

        public void InvokeAll(T arg)
        {
            for(int i = 0; i < actions.Count;)
            {
                try
                {
                    actions[i].Invoke(arg);
                    ++i;
                }
                catch(Exception ex)
                {
                    actions.RemoveAt(i);
                    Debug.LogError(ex);
                }
            }
        }
    }
    [Serializable]
    public class ActionContainer<T1, T2> : BaseActionContainer
    {
        List<UnityAction<T1, T2>> actions = new List<UnityAction<T1, T2>>();

        public ActionContainer(string name) : base(name)
        {
        }

        public override void Clear()
        {
            actions.Clear();
        }
        public void AddAction(UnityAction<T1, T2> action)
        {
            actions.Add(action);
        }
        public void RemoveAction(UnityAction<T1, T2> action)
        {
            actions.Remove(action);
        }

        public void InvokeAll(T1 arg1, T2 arg2)
        {

            for(int i = 0; i < actions.Count;)
            {
                try
                {
                    actions[i].Invoke(arg1, arg2);
                    ++i;
                }
                catch(Exception ex)
                {
                    actions.RemoveAt(i);
                    Debug.LogError(ex);
                }
            }
        }
    }
    [Serializable]
    public class ActionContainer<T1, T2, T3> : BaseActionContainer
    {
        List<UnityAction<T1, T2, T3>> actions = new List<UnityAction<T1, T2, T3>>();

        public ActionContainer(string name) : base(name)
        {
        }

        public override void Clear()
        {
            actions.Clear();
        }
        
        public void AddAction(UnityAction<T1, T2, T3> action)
        {
            actions.Add(action);
        }
        public void RemoveAction(UnityAction<T1, T2, T3> action)
        {
            actions.Remove(action);
        }

        public void InvokeAll(T1 arg1, T2 arg2, T3 arg3)
        {
            for(int i = 0; i < actions.Count;)
            {
                try
                {
                    actions[i].Invoke(arg1, arg2, arg3);
                    ++i;
                }
                catch(Exception ex)
                {
                    actions.RemoveAt(i);
                    Debug.LogError(ex);
                }
            }
        }
    }
    [Serializable]
    public class ActionContainer<T1, T2, T3, T4> : BaseActionContainer
    {
        List<UnityAction<T1, T2, T3, T4>> actions = new List<UnityAction<T1, T2, T3, T4>>();

        public ActionContainer(string name) : base(name)
        {
        }

        public override void Clear()
        {
            actions.Clear();
        }
        
        public void AddAction(UnityAction<T1, T2, T3, T4> action)
        {
            actions.Add(action);
        }
        public void RemoveAction(UnityAction<T1, T2, T3, T4> action)
        {
            actions.Remove(action);
        }
        

        public void InvokeAll(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            for(int i = 0; i < actions.Count;)
            {
                try
                {
                    actions[i].Invoke(arg1, arg2, arg3, arg4);
                    ++i;
                }
                catch(Exception ex)
                {
                    actions.RemoveAt(i);
                    Debug.LogError(ex);
                }
            }
        }
    }
}