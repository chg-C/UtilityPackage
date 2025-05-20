///스크립트 생성 일자 - 2025 - 05 - 19
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHG.StatSystem
{
	/// <summary>
	/// Stat의 데이터 전송 과정에 사용하는 DTO interface
	/// </summary>
	public interface IStatInfo<T>
	{
		T StatType { get; }
		float BaseValue { get; }
		float CalculatedValue { get; }
	}

	/// <summary>
	/// Stat 값의 변경 이벤트 interface
	/// </summary>
	public interface IStatEvents<T> where T : Enum
	{
		event Action<T, IStatInfo<T>> OnCoreStatModified;
		event Action<StatType, IStatInfo<StatType>> OnCustomStatModified;

		event Action<IStatModifier> OnModifierAdded;
		event Action<IStatModifier> OnModifierRemoved;
	}
}