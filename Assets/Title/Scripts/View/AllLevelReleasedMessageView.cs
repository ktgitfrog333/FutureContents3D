using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Title.View
{
    /// <summary>
    /// ビュー
    /// 全ステージ解放
    /// </summary>
    public class AllLevelReleasedMessageView : MonoBehaviour
    {
        /// <summary>位置の配列</summary>
        [SerializeField] private Vector3[] points = { new Vector3(0f, -50f, 0f), new Vector3(0f, 0f, 0f) };
        /// <summary>終了時間の配列</summary>
        [SerializeField] private float[] durations = { .25f, 1.5f, 1.25f };
        /// <summary>トランスフォーム</summary>
        private Transform _transform;

        /// <summary>
        /// テロップ表示アニメーション
        /// </summary>
        /// <param name="observer">バインド</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlayBoundAnimation(System.IObserver<bool> observer)
        {
            if (_transform == null)
                _transform = transform;
            // 手前に表示させる
            _transform.SetAsLastSibling();
            var seq = DOTween.Sequence()
                .Append(_transform.DOLocalMove(points[0], durations[0]))
                .Insert(durations[1], _transform.DOLocalMove(points[1], durations[2]))
                .SetUpdate(true)
                .OnComplete(() => observer.OnNext(true));
            seq.Play();

            yield return null;
        }
    }
}
