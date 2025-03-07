///스크립트 생성 일자 - 2025 - 03 - 07
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using UnityEngine;

namespace CHG.Utilities.ColliderEvents
{
    /// <summary>
    /// OnCollisionEnter / OnCollisionExit 이벤트 핸들러
    /// </summary>
    [DisallowMultipleComponent]
	public class OnCollisionEventHandler : ColliderEventHandler
	{
        void OnCollisionEnter(Collision collision)
        {
            if(NeedTag)
            {
                if(collision.gameObject.tag != _targetTag)
                    return;
            }
            if(NeedLayer)
            {
                if((_targetLayer.value & (1<<collision.gameObject.layer)) == 0)
                    return;
            }

            _onEnter?.Invoke();
        }
        void OnCollisionExit(Collision collision)
        {
            if(NeedTag)
            {
                if(collision.gameObject.tag != _targetTag)
                    return;
            }
            if(NeedLayer)
            {
                if((_targetLayer.value & (1<<collision.gameObject.layer)) == 0)
                    return;
            }

            _onExit?.Invoke();
        }
    }
}