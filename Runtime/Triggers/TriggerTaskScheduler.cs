///스크립트 생성 일자 - 2025 - 03 - 10
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.0.8

using System;
using System.Collections;
using UnityEngine;

namespace CHG.Utilities.Triggers
{
	public class TriggerTaskScheduler : MonoBehaviour
	{
		#region Member Enum
		[Serializable]
		public enum ProcessingMode
		{
			Sequential,
			Parallel
		}
		#endregion

		#region Inspector Fields
		[SerializeField, Tooltip("스케줄러의 진행 방식, Serial이면 순차적으로 / Parallel이면 병렬로 이벤트 발생")]
		private ProcessingMode _mode;

		#endregion

		#region Fields
		Transform _transform;
		[SerializeField, HideInInspector]
		TriggerTask[] _tasks;
		#endregion
		
		#region Properties
		public ProcessingMode Mode
		{
			get => _mode;
			set => _mode = value;
		}
		public TriggerTask[] Tasks
		{
			get => _tasks;
			set => _tasks = value;
		}
		#endregion
		
		#region Methods
		protected void ResetAllTasks()
		{
			for(int i = 0; i < _tasks.Length; ++i)
			{
				_tasks[i].Reset();
			}
		}
		public void Execute()
		{
			StartCoroutine(ExecuteCoroutine());
		}
		protected virtual IEnumerator ExecuteCoroutine()
		{
			switch(Mode)
			{
				case ProcessingMode.Sequential:
					yield return SequentialTask();
					break;
				case ProcessingMode.Parallel:
					yield return ParallelTask();
					break;
			}
		}
		protected virtual IEnumerator SequentialTask()
		{
			var wait = new WaitForEndOfFrame();
			TaskState state;
			int currentIndex = 0;

			ResetAllTasks();
			while(currentIndex < _tasks.Length)
			{
				if(_tasks[currentIndex] != null && _tasks[currentIndex].isActiveAndEnabled)
				{
					state = _tasks[currentIndex].Tick(Time.deltaTime);
					if(state == TaskState.Completed)
					{
						++currentIndex;
						continue;
					}
					yield return wait;
				}
				else
				{
					++currentIndex;
					continue;
				}
			}
		}
		protected virtual IEnumerator ParallelTask()
		{
			bool isCompleted = true;
			var wait = new WaitForEndOfFrame();
			do
			{
				for(int i = 0; i < _tasks.Length; ++i)
				{
					if(_tasks[i] != null && _tasks[i].isActiveAndEnabled && _tasks[i].CurrentState != TaskState.Completed)
					{
						TaskState state = _tasks[i].Tick(Time.deltaTime);

						if(state != TaskState.Completed)
							isCompleted = false;
					}
				}
				yield return wait;
			}while(!isCompleted);	
		}
		#endregion

		#region MonoBehaviour Methods
		protected virtual void Awake()
		{
			_tasks = GetComponents<TriggerTask>();
		}
		#endregion
	}
}