using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Title.View
{
    /// <summary>
    /// ビュー
    /// オプション設定項目選択のカーソル
    /// </summary>
    public class OptionCursorView : MonoBehaviour
    {
        /// <summary>トランスフォーム</summary>
        private Transform _transform;
        /// <summary>アニメーション再生時間</summary>
        [SerializeField] private float duration = .1f;

        /// <summary>
        /// カーソル配置位置の変更
        /// </summary>
        /// <param name="position">移動先のポジション</param>
        /// <param name="sizeDelta">サイズ</param>
        /// <returns>成功／失敗</returns>
        public bool SetSelect(Vector3 position, Vector2 sizeDelta)
        {
            try
            {
                if (isActiveAndEnabled)
                {
                    if (_transform == null)
                        _transform = transform;
                    _transform.position = position;
                    (_transform as RectTransform).sizeDelta = sizeDelta;
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// カーソル移動アニメーション
        /// </summary>
        /// <param name="position">移動先のポジション</param>
        /// <param name="sizeDelta">サイズ</param>
        /// <returns>成功／失敗</returns>
        public bool PlaySelectAnimation(Vector3 position, Vector2 sizeDelta)
        {
            try
            {
                if (isActiveAndEnabled)
                {
                    if (_transform == null)
                        _transform = transform;
                    var seq = DOTween.Sequence()
                        .Append(_transform.DOMove(position, duration))
                        .Join((_transform as RectTransform).DOSizeDelta(sizeDelta, duration))
                        .SetUpdate(true);
                    seq.Play();
                }

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }
    }
}
