///스크립트 생성 일자 - 2025 - 05 - 20
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using UnityEngine;

namespace CHG.StatSystem
{
	[CreateAssetMenu(fileName = "StatType", menuName = "Stat System/Stat Type", order = 100)]
	public class StatType : ScriptableObject
	{
		#region Inspector Fields
		[SerializeField, Tooltip("스테이터스 이름")]
		private string _statName;
		[SerializeField, Tooltip("스테이터스의 Tag")]
		private string _statTag;
		#endregion

		#region Fields
		#endregion
		
		#region Properties
		public string StatName => _statName;
		public string StatTag => _statTag;
		#endregion

		#region Methods
		#endregion

		#region UnityEditor Only Methods
		#if UNITY_EDITOR
		protected virtual void Reset()
		{
		}
		protected virtual void OnValidate()
		{
		}
		#endif
		#endregion
	}
}