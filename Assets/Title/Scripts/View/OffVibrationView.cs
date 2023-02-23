using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Title.Common;

namespace Title.View
{
    /// <summary>
    /// ビュー
    /// バイブレーション機能ラジオボタンのOFF
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class OffVibrationView : MonoBehaviour
    {
        /// <summary>終了時間</summary>
        [SerializeField] private float duration = .1f;
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public bool PlayFadeAnimation(EnumFadeState state)
        {
            try
            {
                image.DOFade(endValue: state.Equals(EnumFadeState.Open) ? 0f : 1f, duration)
                    .SetUpdate(true);

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
