///스크립트 생성 일자 - 2025 - 03 - 10
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using System.Collections.Generic;
using CHG.Utilities.Attribute;
using UnityEngine;

namespace CHG.Utilities.Triggers
{
	public class TriggerInvoker : MonoBehaviour
	{
		#region Constants
		/// <summary>
		/// 오류 방지용 최소 쿨다운
		/// </summary>
		private const float kMinCooldown = 0.01f;
		#endregion

		#region Inspector Fields
		[SerializeField, Tooltip("1회용 트리거, 수동으로 재활성화하지 않는 이상 다시 발동 안 함")]
		private bool _oneShot;

		[SerializeField, Tooltip("이 트리거의 재사용 대기시간"), ConditionalHide("OneShot", inverted = true)]
		private float _cooldown;
		
		[SerializeField, Tooltip("이 트리거가 발동했을 때 처리할 행동 목록")]
		private TriggerTaskScheduler[] _connectedTriggers;
		#endregion

		#region Fields
		private float lastTimeTriggered = -1;
		private bool triggered = false;
		#endregion
		
		#region Properties
		public bool Triggered => triggered;
		/// <summary>
		/// 1회용 트리거, 수동으로 재활성화하지 않는 이상 다시 발동 안 함
		/// </summary>
		public bool OneShot
		{
			get => _oneShot;
			set => _oneShot = value;
		}
		/// <summary>
		/// 재사용 대기시간
		/// </summary>
		public float Cooldown
		{
			get => _cooldown;
			set => _cooldown = Mathf.Max(kMinCooldown, value);
		}
		/// <summary>
		/// 이 트리거가 발동했을 때 처리할 행동 목록
		/// </summary>
		public TriggerTaskScheduler[] ConnectedTriggers => _connectedTriggers;
		#endregion
		
		#region Methods
		/// <summary>
		/// 이 트리거를 재설정
		/// </summary>
		public void ResetTrigger()
		{
			lastTimeTriggered = -1;
		}

		/// <summary>
		/// 트리거 실행, 연결된 Trigger Scheduler를 Execute
		/// </summary>
		[ContextMenu("Test Trigger")]
		public void Trigger()
		{
			if(OneShot && Triggered)
				return;
			else if(OneShot == false && Time.time - lastTimeTriggered < Cooldown)
				return;

			for(int i = 0; i < _connectedTriggers.Length; ++i)
			{
				_connectedTriggers[i].Execute();
			}

			triggered = true;
			lastTimeTriggered = Time.time;
		}
		#endregion

		#region MonoBehaviour Methods
        void OnValidate()
        {
            Cooldown = _cooldown;
        }
        #endregion
    }
}