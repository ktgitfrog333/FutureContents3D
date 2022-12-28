using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Model
{
    /// <summary>
    /// レベル内のレイ判定の共通設定
    /// カプセルキャスト
    /// </summary>
    [RequireComponent(typeof(CapsuleCollider))]
    public class LevelPhysicsSerializerCapsule : MonoBehaviour
    {
        /// <summary>ポイント１</summary>
        [SerializeField] protected Vector3 footerPoint;
        /// <summary>ポイント２</summary>
        [SerializeField] protected Vector3 headerPoint;
        /// <summary>半径</summary>
        [SerializeField] protected float radius;
        /// <summary>角度</summary>
        [SerializeField] protected Vector3 direction;
        /// <summary>距離</summary>
        [SerializeField] protected float maxDistance;
        /// <summary>プレビュー表示（選択時）の色</summary>
        [SerializeField] protected Color previewColor;

        protected virtual void Reset()
        {
            // 規定値
            footerPoint = gameObject.transform.position - Vector3.up * 0.5f;
            headerPoint = gameObject.transform.position + Vector3.up * 0.5f;
            radius = .5f;
            direction = Vector3.up;
            maxDistance = 6.0f;
            previewColor = Color.green;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = previewColor;
            Gizmos.DrawWireSphere(footerPoint, radius);
            Gizmos.DrawWireSphere(direction * maxDistance + headerPoint, radius);
        }
    }
}
