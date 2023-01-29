using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Title.Template;
using Title.Common;
using UniRx;

namespace Title.Model
{
    /// <summary>
    /// モデル
    /// BGMスライダー
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(EventTrigger))]
    public class SliderBgmModel : UIEventController
    {
        /// <summary>ボタン</summary>
        private Button _button;
        /// <summary>イベントトリガー</summary>
        private EventTrigger _eventTrigger;
        /// <summary>ボリューム番号</summary>
        private readonly IntReactiveProperty _index = new IntReactiveProperty();
        /// <summary>ボリューム番号</summary>
        public IReactiveProperty<int> Index => _index;

        protected override void OnEnable()
        {
            base.OnEnable();
            var tTResources = new TitleTemplateResourcesAccessory();
            var datas = tTResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG);
            var configMap = tTResources.GetSystemConfig(datas);
            _index.Value = configMap[EnumSystemConfig.BGMVolumeIndex];
        }

        /// <summary>
        /// インデックス番号をセット
        /// </summary>
        /// <param name="index">インデックス番号</param>
        /// <returns>成功／失敗</returns>
        public bool SetIndex(int index)
        {
            try
            {
                _index.Value = index;

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
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
