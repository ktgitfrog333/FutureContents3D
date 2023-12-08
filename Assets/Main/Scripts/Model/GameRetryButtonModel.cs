using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Main.Common;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// もう一度遊ぶボタン
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    [RequireComponent(typeof(GameContentsConfig))]
    public class GameRetryButtonModel : UIEventController
    {
        /// <summary>ボタン</summary>
        private Button _button;
        /// <summary>イベントトリガー</summary>
        private EventTrigger _eventTrigger;
        /// <summary>設定ファイル</summary>
        [SerializeField] private GameContentsConfig gameContentsConfig;

        private void Reset()
        {
            gameContentsConfig = GetComponent<GameContentsConfig>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            var common = new MainPresenterCommon();
            if (common.IsFinalLevel())
                GetComponent<Button>().navigation = gameContentsConfig.Navigations[0];
        }

        /// <summary>
        /// ボタンのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetButtonEnabled(bool enabled)
        {
            try
            {
                if (_button == null)
                    _button = GetComponent<Button>();
                _button.enabled = enabled;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// イベントトリガーのステータスを変更
        /// </summary>
        /// <param name="enabled">有効／無効</param>
        /// <returns>成功／失敗</returns>
        public bool SetEventTriggerEnabled(bool enabled)
        {
            try
            {
                if (_eventTrigger == null)
                    _eventTrigger = GetComponent<EventTrigger>();
                _eventTrigger.enabled = enabled;

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
