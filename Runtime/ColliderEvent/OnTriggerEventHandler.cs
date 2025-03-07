///스크립트 생성 일자 - 2025 - 03 - 07
///스크립트 담당자 - 최현규
///스크립트 생성 버전 - 0.1

using UnityEngine;

namespace CHG.Utilities.ColliderEvents
{
    /// <summary>
    /// OnTriggerEnter / OnTriggerExit 이벤트 핸들러
    /// </summary>
    [DisallowMultipleComponent]
	public class OnTriggerEventHandler : ColliderEventHandler
	{
        void OnTriggerEnter(Collider other)
        {            
            if(NeedTag)
            {
                if(other.gameObject.tag != _targetTag)
                    return;
            }
            if(NeedLayer)
            {
                if((_targetLayer.value & (1<<other.gameObject.layer)) == 0)
                    return;
            }

            _onEnter?.Invoke();
        }
        void OnTriggerExit(Collider other)
        {            
            if(NeedTag)
            {
                if(other.gameObject.tag != _targetTag)
                    return;
            }
            if(NeedLayer)
            {
                if((_targetLayer.value & (1<<other.gameObject.layer)) == 0)
                    return;
            }

            _onExit?.Invoke();
        }
    }
}