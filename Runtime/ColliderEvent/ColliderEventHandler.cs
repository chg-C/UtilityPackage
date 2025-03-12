///스크립트 생성 일자 - 2025 - 03 - 07
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using System;
using CHG.Utilities.Attribute;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


namespace CHG.Utilities.ColliderEvents
{
	[RequireComponent(typeof(Collider))]
	public abstract class ColliderEventHandler : MonoBehaviour
	{
		#region Member Enum
		/// <summary>
		/// Event Invoke 조건
		/// </summary>
		[Flags]
		public enum Condition
		{
			/// <summary>
			/// 조건 없음
			/// </summary>
			None = 0,
			/// <summary>
			/// Tag 일치
			/// </summary>
			Tags = 1 << 0,
			/// <summary>
			/// Layer 일치
			/// </summary>
			Layers = 1 << 1
		}
		#endregion

		#region Inspector Fields
		[Header("Conditions")]
		[SerializeField, Tooltip("발동 객체 제한")]
		protected Condition _condition;
		//
		[SerializeField, Tooltip("태그가 일치해야 발동"), ConditionalHide("NeedTag")]
		protected string _targetTag;
		[SerializeField, Tooltip("레이어가 일치해야 발동"), ConditionalHide("NeedLayer")]
		protected LayerMask _targetLayer;

		[Header("Events")]
		[SerializeField, Tooltip("진입 이벤트")]
		protected UnityEvent _onEnter = new UnityEvent();
		[SerializeField, Tooltip("탈출 이벤트")]
		protected UnityEvent _onExit = new UnityEvent();

		#if UNITY_EDITOR
		[Header("Gizmos")]
		[SerializeField, Tooltip("디버깅용 Gizmo를 그리기")]
		private bool _showGizmo;
		[SerializeField, Tooltip("Gizmo 색상"), ConditionalHide("_showGizmo")]
		private Color _gizmoColor = Color.red;
		[SerializeField, Tooltip("이 Event Handler에 대한 설명"), ConditionalHide("_showGizmo")]
		private string _tooltipText;
		#endif
		#endregion

		#region Fields
		protected Transform _transform;
		protected Collider _collider;
		
		#endregion
		
		#region Properties
		public new Transform transform
		{
			get
			{
				#if UNITY_EDITOR
				if(_transform == null) _transform = GetComponent<Transform>();
				#endif
				return _transform;
			}
		}
		public new Collider collider
		{
			get
			{
				#if UNITY_EDITOR
				if(_collider == null) _collider = GetComponent<Collider>();
				#endif
				return _collider;
			}
			set => _collider = value;
		}
		public bool NeedTag => _condition.HasFlag(Condition.Tags);
		public bool NeedLayer => _condition.HasFlag(Condition.Layers);
		#endregion
		
		#region Methods

		/// <summary>
		/// 컴퍼넌트를 캐싱
		/// </summary>	
		protected virtual void CacheComponents()
		{
			 _transform = GetComponent<Transform>();
			 _collider = GetComponent<Collider>();
		}
		#endregion

		#region MonoBehaviour Methods
		protected virtual void Awake()
		{
			CacheComponents();
		}
		#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if(!_showGizmo)
				return;
			
			Gizmos.color = _gizmoColor;// Collider의 타입에 따라 Gizmo 그리기
			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

            if (collider is BoxCollider)
            {
                BoxCollider boxCollider = (BoxCollider)collider;
                Gizmos.DrawCube(boxCollider.center, Vector3.Scale(boxCollider.size, transform.lossyScale));
            }
            else if (collider is SphereCollider)
            {
                SphereCollider sphereCollider = (SphereCollider)collider;
				float max = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
                Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius * max);
            }
			else
			{
				Debug.LogWarning("Does Not Support Capsule or Mesh Collider");
				_showGizmo = false;
			}
			Gizmos.matrix = Matrix4x4.identity;
			
			string label = gameObject.name;
			if(!string.IsNullOrEmpty(_tooltipText))
			label += "\n" + _tooltipText;
			Handles.Label(transform.position, label);
        }
		#endif
        #endregion
    }
}