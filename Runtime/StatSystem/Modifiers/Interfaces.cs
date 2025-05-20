///스크립트 생성 일자 - 2025 - 05 - 20
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using System;
using UnityEngine;

namespace CHG.StatSystem
{
	public interface IStatModifier : IEquatable<IStatModifier>, IComparable<IStatModifier>
	{
		/// <summary>
		/// Target 능력치
		/// </summary>
		object Target { get; }
		/// <summary>
		/// Modifier의 원천
		/// </summary>
		object Source { get; }
		/// <summary>
		/// Modifier의 값
		/// </summary>
		float Value { get; }
		/// <summary>
		/// 이 Modifier의 종류
		/// </summary>
		ModifierType ModifierType { get; }
		/// <summary>
		/// Modifier 적용 순서(오름차순)
		/// </summary>
		int Order { get; }
	}

	public enum ModifierType
	{
		Flat,
		PercentAdd,
		PercentMultiply,
	};
}