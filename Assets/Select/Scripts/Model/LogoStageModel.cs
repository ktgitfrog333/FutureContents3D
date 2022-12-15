using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace Select.Model
{
    /// <summary>
    /// モデル
    /// ロゴステージ
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class LogoStageModel : UIEventController
    {
        /// <summary>ステージ番号</summary>
        private int _index = -1;
        /// <summary>番号置換の正規表現</summary>
        private readonly Regex _regex = new Regex("^.*_");
        /// <summary>ステージ番号</summary>
        public int Index
        {
            get
            {
                // ステージ番号セットの初期処理
                if (_index < 0)
                    _index = int.Parse(_regex.Replace(name, ""));
                if (_index < 0)
                    throw new System.Exception("置換失敗");
                return _index;
            }
        }
        /// <summary>ボタン</summary>
        private Button _button;

        private void Reset()
        {
            //int idx = 0;
            //foreach (Transform page in transform.parent.parent)
            //{
            //    foreach (Transform logo in page)
            //    {
            //        index = idx++;
            //        if (logo.Equals(transform))
            //            break;
            //    }
            //}
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
    }
}
