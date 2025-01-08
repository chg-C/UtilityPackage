using UnityEngine;

namespace CHG.Utilities.Patterns
{
    /// <summary>
    /// Dirty Flag Pattern 구현용 Interface<br/>
    /// 객체 상태가 변경될 때 내부의 Dirty Flag를 True로 만들고(MarkDirty), Dirty Flag가 True일 때에만 변경 관련 처리를 수행한다.<br/>
    /// 처리 완료 후에는 Dirty Flag를 False로 되돌린다(Clean).
    /// </summary>
    public interface IDirtyFlag 
    {
        bool IsDirty {get; set;}

        /// <summary>
        /// 변경이 발생했을 때 Dirty 상태를 true로 만듬
        /// </summary>
        void MarkDirty();
        /// <summary>
        /// Dirty 상태를 false로 만들고 다음 변경을 기다림
        /// </summary>
        void Clean();
    }
}