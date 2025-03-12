///스크립트 생성 일자 - 2025 - 03 - 10
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using UnityEngine;


namespace CHG.Utilities.Triggers
{
	/// <summary>
	/// Task의 상태
	/// </summary>
	public enum TaskState
	{
		/// <summary>
		/// Task가 진행되지 않았음
		/// </summary>
		Waiting,
		/// <summary>
		/// Task 진행중
		/// </summary>
		Running,
		/// <summary>
		/// Task 완료됨
		/// </summary>
		Completed
	}

	/// <summary>
	/// Task Run 함수의 결과
	/// </summary>
	public enum TaskResult
	{
		/// <summary>
		/// Task 진행중
		/// </summary>
		Running,
		/// <summary>
		/// Task 완료
		/// </summary>
		Completed,
		/// <summary>
		/// 오류 발생_이 Task 다시 시도
		/// </summary>
		Error_Retry,
		/// <summary>
		/// 오류 발생_이 Task를 포기하고 다음 Task로 이동
		/// </summary>
		Error_Continue
	}

	public abstract class TriggerTask : MonoBehaviour
	{
		#region Inspector Fields
		[SerializeField, Tooltip("Task 시작까지의 Delay")]
		private float _delay;
		#endregion

		#region Fields
		float delayTick;
		private TaskState _currentState;		
		#endregion
		
		#region Properties
		/// <summary>
		/// Task 시작까지의 Delay
		/// </summary>
		public float Delay
		{
			get => _delay;
			set => _delay = Mathf.Max(0, value);
		}
		/// <summary>
		/// 현재 Task의 상태
		/// </summary>
		public TaskState CurrentState
		{
			get => _currentState;
			protected set => _currentState = value;
		}
		#endregion

		#region Events
		protected virtual void OnTaskStart()
		{
			
		}
		protected virtual void OnTaskStop()
		{
			
		}
		#endregion
		
		#region Methods
		protected abstract TaskResult Execute();
		public virtual TaskState Tick(float deltaTime)
		{
			if(CurrentState == TaskState.Completed)
				return TaskState.Completed;
			
			if(CurrentState != TaskState.Running)
			{
				delayTick = Delay;
				OnTaskStart();
				CurrentState = TaskState.Running;
			}

			if(delayTick > 0)
			{
				delayTick -= deltaTime;
				return CurrentState;
			}

			var result = Execute();

			switch(result)
			{
				case TaskResult.Running:
					CurrentState = TaskState.Running;
					break;
				case TaskResult.Error_Continue:
				case TaskResult.Completed:
					CurrentState = TaskState.Completed;
					break;
				case TaskResult.Error_Retry:
					CurrentState = TaskState.Waiting;
					break;				
				default:
					throw new System.NotImplementedException();
			}

			if(CurrentState == TaskState.Completed)
				OnTaskStop();
			
			return CurrentState;
		}
		public virtual void Reset()
		{
			CurrentState = TaskState.Waiting;
		}
		#endregion

		#region MonoBehaviour Methods
		protected virtual void Start()
		{
			delayTick = Delay;
		}
        void OnValidate()
        {
            Delay = _delay;
        }
        #endregion
    }
}