using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// セーフゾーン
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class SafeZoneModel : MonoBehaviour
    {
        /// <summary>トリガーから出る</summary>
        private readonly BoolReactiveProperty _isTriggerExited = new BoolReactiveProperty();
        /// <summary>トリガーから出る</summary>
        public IReactiveProperty<bool> IsTriggerExited => _isTriggerExited;

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(ConstTagNames.TAG_NAME_PLAYER))
            {
                _isTriggerExited.Value = true;
            }
        }
    }
}
