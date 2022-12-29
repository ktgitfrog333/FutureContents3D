using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// ゴールポイント
    /// </summary>
    public class GoalPointModel : LevelPhysicsSerializerCapsule
    {
        /// <summary>トリガーへ入る</summary>
        private readonly BoolReactiveProperty _isTriggerEntered = new BoolReactiveProperty();
        /// <summary>トリガーへ入る</summary>
        public IReactiveProperty<bool> IsTriggerEntered => _isTriggerEntered;
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        protected override void Reset()
        {
            base.Reset();
            footerPoint = gameObject.transform.position - Vector3.up * 0.25f;
            headerPoint = gameObject.transform.position + Vector3.up * 0.25f;
            radius = .25f;
            direction = Vector3.zero;
            maxDistance = 0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(ConstTagNames.TAG_NAME_PLAYER))
            {
                _isTriggerEntered.Value = true;
            }
        }

        private void FixedUpdate()
        {
            footerPoint = gameObject.transform.position - Vector3.up * 0.25f;
            headerPoint = gameObject.transform.position + Vector3.up * 0.25f;
            if (!Physics.CheckCapsule(footerPoint, headerPoint, radius, LayerMask.GetMask(ConstLayerNames.LAYER_NAME_FLOOR)))
            {
                if (_transform == null)
                    _transform = transform;
                _transform.position += Physics.gravity * Time.deltaTime;
            }
        }
    }
}
