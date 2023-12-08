using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Title.Common;
using Universal.Template;
using Universal.Bean;
using Universal.Common;

namespace Title.View
{
    /// <summary>
    /// ビュー
    /// バイブレーション機能ラジオボタンのON
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class OnVibrationView : MonoBehaviour
    {
        /// <summary>終了時間</summary>
        [SerializeField] private float duration = .1f;
        /// <summary>イメージ</summary>
        [SerializeField] private Image image;

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            var temp = new TemplateResourcesAccessory();
            var datas = temp.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
            var color = new Color(image.color.r, image.color.g, image.color.b, datas.vibrationEnableIndex == (int)EnumVibrationEnableState.ON ? 1f : 0f);
            image.color = color;
        }

        /// <summary>
        /// フェードのDOTweenアニメーション再生
        /// </summary>
        /// <param name="state">ステータス</param>
        /// <returns>成功／失敗</returns>
        public bool PlayFadeAnimation(EnumOptionContentState state)
        {
            try
            {
                image.DOFade(endValue: state.Equals(EnumOptionContentState.Selected) ? 1f : 0f, duration)
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
