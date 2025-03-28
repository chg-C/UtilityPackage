///스크립트 생성 일자 - 2025 - 03 - 12
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using System;
using System.Collections.Generic;
using CHG.Utilities.Patterns;
using CHG.EventDriven.Arguments;
using UnityEngine;
using UnityEngine.Events;

namespace CHG.EventDriven
{
	public class GlobalEventManager : SingletonMonobehaviour<GlobalEventManager>
	{
		#region Inspector Fields
		/// <summary>
		/// 미리 등록된 EventArgs 인자값 조건
		/// </summary>
		[SerializeField]
		private Dictionary<string, Type> _fixedAvailableEventArgs = new Dictionary<string, Type>();
		#endregion

		#region Fields
		/// <summary>
		/// 인자값 없는 Event 목록
		/// </summary>
		[SerializeField]
		Dictionary<string, Action> _voidEventsDictionary = new Dictionary<string, Action>();
		/// <summary>
		/// TODO: List<object>를 뭔가 더 나은 방법으로 바꿀 것
		/// </summary>
		[SerializeField]
		Dictionary<string, List<object>> _eventsDictionary = new Dictionary<string, List<object>>();
		[SerializeField]
		Dictionary<string, Type> _availableEventArgs = new Dictionary<string, Type>();
        #endregion

        #region Properties
        public override bool IsPersistent => true;
		/// <summary>
		/// 미리 지정된 이벤트-허용 타입 목록
		/// </summary>
		public Dictionary<string, Type> FixedAvailableEventArgs
		{
			get => _fixedAvailableEventArgs;
			set => _fixedAvailableEventArgs = value;
		}
		#endregion

		
		#region Methods

		#region Without Arguments
		/// <summary>
		/// 인자값 없는 Event 구독
		/// </summary>
		public void Subscribe(string name, Action action)
		{
			if(!_voidEventsDictionary.ContainsKey(name))
			{
				_voidEventsDictionary.Add(name, null);
			}
			_voidEventsDictionary[name] += action;
		}
		/// <summary>
		/// 인자값 없는 Event 구독 해제
		/// </summary>
		public void Unsubscribe(string name, Action action)
		{
			if(!_voidEventsDictionary.ContainsKey(name))
				return;

			_voidEventsDictionary[name] -= action;
		}
		/// <summary>
		/// 인자값 없는 Event 발동시키기
		/// </summary>
		public void Publish(string name)
		{
			if(!_voidEventsDictionary.ContainsKey(name))
				return;
			
			_voidEventsDictionary[name]?.Invoke();
		}
		#endregion

		#region With Arguments
		/// <summary>
		/// BaseEventArgs를 상속받는 EventArgs 클래스를 인자로 받는 Event 구독
		/// </summary>
		public void Subscribe<T>(string name, Action<T> action) where T : BaseEventArgs
		{
			if(IsValidArgument<T>(name))
			{
				if(!_eventsDictionary.ContainsKey(name))
				{
					_eventsDictionary.Add(name, new List<object>());
				}		
				
				_eventsDictionary[name].Add(action);
			}
			else
			{
				Debug.LogWarning($"Type Error On Subscribe: Event {name} does not Support {typeof(T)}!");
			}
		}
		/// <summary>
		/// BaseEventArgs를 상속받는 EventArgs 클래스를 인자로 받는 Event 구독 해제
		/// </summary>
		public void Unsubscribe<T>(string name, Action<T> action) where T : BaseEventArgs
		{
			if(IsValidArgument<T>(name))
			{
				if(!_eventsDictionary.ContainsKey(name))
				{
					return;
				}
				
				_eventsDictionary[name].Remove(action);
				
				if(_eventsDictionary[name].Count == 0)
				{
					//아무도 구독하지 않게 된 상황이고 유동적인 제약 조건을 사용중이라면 제약 해제
					if(_availableEventArgs.ContainsKey(name))
					{
						_availableEventArgs.Remove(name);
					}

					_eventsDictionary[name] = null;
					_eventsDictionary.Remove(name);
				}
			}
			else
			{
				Debug.LogWarning($"Type Error On Unsubscribe: Event {name} does not Support {typeof(T)}!");
			}
		}
		/// <summary>
		/// BaseEventArgs를 상속받는 EventArgs 클래스를 인자로 받는 Event 발동
		/// </summary>
		public void Publish<T>(string name, T args) where T : BaseEventArgs
		{
			if(IsValidArgument<T>(name))
			{
				if(!_eventsDictionary.ContainsKey(name))
				{
					return;
				}
				foreach(Action<T> action in _eventsDictionary[name])
				{
					action?.Invoke(args);
				}
			}
			else
			{
				Debug.LogWarning($"Type Error On Publish: Event {name} does not Support {typeof(T)}!");
			}
		}
		
		public bool IsValidArgument<T>(string name) where T : BaseEventArgs
		{
			Type type = null;
			if(_fixedAvailableEventArgs.TryGetValue(name, out type))
			{
				if(type == typeof(T))
					return true;
				else
					return false;
			}
			else if(_availableEventArgs.TryGetValue(name, out type))
			{
				if(type == typeof(T))
					return true;
				else
					return false;
			}
			//이 이름의 Event에 인자값 제한사항 없음. 새로 등록.
			else
			{
				_availableEventArgs.Add(name, typeof(T));
				return true;
			}
		}
		#endregion

		#endregion

		#region MonoBehaviour Methods
		protected virtual void Awake()
		{
		}
		#endregion
		
		#region UnityEditor Only Methods
		#if UNITY_EDITOR
		protected virtual void Reset()
		{
			_eventsDictionary.Clear();
			_voidEventsDictionary.Clear();
			_fixedAvailableEventArgs.Clear();
			_availableEventArgs.Clear();
		}
		protected virtual void OnValidate()
		{
		}
		#endif
		#endregion
	}
}