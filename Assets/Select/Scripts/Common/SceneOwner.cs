using Select.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Select.Common
{
    /// <summary>
    /// シーンオーナー
    /// </summary>
    public class SceneOwner : MonoBehaviour
    {
        /// <summary>
        /// シーンIDを取得
        /// </summary>
        /// <returns>シーンID</returns>
        public Dictionary<EnumSystemCommonCash, int> GetSystemCommonCash()
        {
            try
            {
                var tSResources = new SelectTemplateResourcesAccessory();
                tSResources.Initialize();
                var datas = tSResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_COMMON_CASH);
                if (datas == null)
                    throw new System.Exception("リソース読み込みの失敗");

                return tSResources.GetSystemConfig(datas);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// シーンIDを更新
        /// </summary>
        /// <param name="configMap">シーン設定</param>
        /// <returns>成功／失敗</returns>
        public bool SetSystemCommonCash(Dictionary<EnumSystemCommonCash, int> configMap)
        {
            try
            {
                var tSResources = new SelectTemplateResourcesAccessory();
                if (!tSResources.SaveDatasCSVOfSystemConfig(ConstResorcesNames.SYSTEM_COMMON_CASH, configMap))
                    Debug.LogError("CSV保存呼び出しの失敗");

                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        /// <summary>
        /// タイトルシーンをロード
        /// </summary>
        public void LoadTitleScene()
        {
            SceneManager.LoadScene("TitleScene");
        }

        /// <summary>
        /// メインシーンをロード
        /// </summary>
        public void LoadMainScene()
        {
            LoadMainScene(0);
        }

        /// <summary>
        /// メインシーンをロード
        /// </summary>
        /// <param name="sceneName">ロードするモード（1はデモ。それ以外はデフォルト）</param>
        public void LoadMainScene(int mode)
        {
            if (mode == 1)
                Debug.LogWarning("Demoモード");
            SceneManager.LoadScene(mode == 1 ? "DemoMainScene" : "MainScene");
        }
    }
}
