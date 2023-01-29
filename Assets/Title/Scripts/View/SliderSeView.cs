using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Title.View
{
    /// <summary>
    /// ビュー
    /// SEスライダー
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class SliderSeView : MonoBehaviour
    {
        /// <summary>スライダー</summary>
        [SerializeField] private Slider _slider;

        private void Reset()
        {
            if (_slider == null)
                _slider = GetComponent<Slider>();
        }

        /// <summary>
        /// スライダーの値をセット
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>成功／失敗</returns>
        public bool SetSliderValue(float value)
        {
            try
            {
                _slider.value = value;

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
