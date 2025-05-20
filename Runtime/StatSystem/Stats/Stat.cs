using System;
using System.Collections.Generic;
using UnityEngine;

///스크립트 생성 일자 - 2025 - 05 - 20
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1
namespace CHG.StatSystem
{
	[Serializable]
	public abstract class Stat<T> : IStatInfo<T>
	{
		#region Inspector Fields
		[SerializeField, Tooltip("이 능력치의 종류")]
		private T _statType;
		[SerializeField, Tooltip("이 능력치의 기본 값")]
		private float _baseValue;
		#endregion

		#region Fields
		/// <summary>
		/// 모든 Modifier 계산을 거친 후 나오는 최종 능력치 값
		/// </summary>
		private float _calculatedValue;
		/// <summary>
		/// Dirty Flag 패턴에 사용할 플래그
		/// </summary>
		private bool _isDirty = true;

		private readonly List<IStatModifier> _modifiers;
		#endregion

		#region Properties
		/// <summary>
		/// 이 능력치의 종류
		/// </summary>
		public T StatType => _statType;
		/// <summary>
		/// 이 능력치의 기본 값
		/// </summary>
		public float BaseValue
		{
			get => _baseValue;
			set
			{
				if (_baseValue != value)
				{
					_baseValue = value;
					_isDirty = true;

					OnStatValueChanged?.Invoke(this);
				}
			}
		}
		/// <summary>
		/// 모든 Modifier 계산을 거친 후 나오는 최종 능력치 값
		/// </summary>
		public float CalculatedValue
		{
			get
			{
				if (_isDirty)
				{
					_calculatedValue = CalculateValue();
					_isDirty = false;
				}

				return _calculatedValue;
			}
		}
		public IReadOnlyList<IStatModifier> Modifiers => _modifiers;
		#endregion

		#region Constructor & Destructor
		public Stat(T statType, float baseValue = 0f)
		{
			_statType = statType;
			_baseValue = baseValue;
			_modifiers = new List<IStatModifier>();
		}
		#endregion

		#region Events
		public event Action<IStatInfo<T>> OnStatValueChanged;
		#endregion

		#region Methods
		public bool IsCoreStat()
		{
			return _statType is Enum;
		}
		public bool IsCustomStat()
		{
			return _statType is StatType;
		}

		public void AddModifier(IStatModifier modifier)
		{
			_modifiers.Add(modifier);
			_modifiers.Sort((m1, m2) => m1.CompareTo(m2));
			_isDirty = true;
			OnStatValueChanged?.Invoke(this);
		}
		public void AddModifiers(List<IStatModifier> modifiers)
		{
			_modifiers.AddRange(modifiers);
			_modifiers.Sort((m1, m2) => m1.Order.CompareTo(m2.Order));
			_isDirty = true;
			OnStatValueChanged?.Invoke(this);
		}
		public bool RemoveModifier(IStatModifier modifier)
		{
			if (_modifiers.Remove(modifier))
			{
				_isDirty = true;
				OnStatValueChanged?.Invoke(this);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Modifier로 계산한 최종 값 리턴
		/// </summary>
		/// <returns></returns>
		private float CalculateValue()
		{
			float calculatedValue = BaseValue;

			float sumPA = 0;

			foreach (var modifier in _modifiers)
			{
				switch (modifier.ModifierType)
				{
					case ModifierType.Flat:
						calculatedValue += modifier.Value;
						break;
					case ModifierType.PercentAdd:
						sumPA += modifier.Value;
						break;
				}
			}

			calculatedValue *= (1 + sumPA);
			
			return calculatedValue;
		}
        #endregion
	}

	/// <summary>
	/// ScriptableObject로 정의되는 Stat Type을 사용하는 커스텀 스탯 클래스
	/// </summary>
	[Serializable]
    public class CustomStat : Stat<StatType>
    {
        public CustomStat(StatType statType, float baseValue = 0) : base(statType, baseValue)
        {
        }
    }
}