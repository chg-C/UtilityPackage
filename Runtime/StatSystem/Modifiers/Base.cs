///스크립트 생성 일자 - 2025 - 05 - 20
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1


namespace CHG.StatSystem
{
    public abstract class BaseStatModifier : IStatModifier
    {
		#region Fields
		protected ModifierType _type;

		/// <summary>
		/// 능력치 변화 목표
		/// </summary>
		protected object _target;
		/// <summary>
		/// Modifier를 발생시킨 주체
		/// </summary>
		protected object _source;
		/// <summary>
		/// Modifier의 실제 값
		/// </summary>
		protected float _value;
		/// <summary>
		/// Modifier의 적용 순서(오름차순)
		/// </summary>
		protected int _order;
		#endregion

		#region Properties
		public object Target => _target;
		/// <summary>
		/// Modifier를 발생시킨 주체
		/// </summary>
		public object Source => _source;
		/// <summary>
		/// Modifier의 값
		/// </summary>
        public float Value => _value;

        public ModifierType ModifierType => _type;

		/// <summary>
		/// Modifier의 적용 순서(오름차순)
		/// </summary>
        public int Order => _order;
        #endregion

        #region Constructor & Destructor
        #endregion

        #region Methods
        #endregion

		public int CompareTo(IStatModifier other)
		{
			return Order.CompareTo(other.Order);
        }

        public bool Equals(IStatModifier other)
        {
			return (other.Target == this.Target &&
				 other.ModifierType == this.ModifierType && other.Source == this.Source && other.Value == this.Value);
				
        }
    }
}