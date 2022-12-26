using Main.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Common
{
    /// <summary>
    /// シーンオーナー
    /// </summary>
    public class SceneOwner : MonoBehaviour
    {
        /// <summary>次のシーン名</summary>
        [SerializeField] private string nextSceneName = "MainScene";
        /// <summary>前のシーン名</summary>
        [SerializeField] private string backSceneName = "SelectScene";

        /// <summary>
        /// シーンIDを取得
        /// </summary>
        /// <returns>シーンID</returns>
        public Dictionary<EnumSystemCommonCash, int> GetSystemCommonCash()
        {
            try
            {
                var tSResources = new MainTemplateResourcesAccessory();
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
        /// シーンIDをカウントアップ（次のステージ番号）
        /// </summary>
        /// <param name="configMap">シーンID</param>
        /// <returns>登録後のシーンID</returns>
        public Dictionary<EnumSystemCommonCash, int> CountUpSceneId(Dictionary<EnumSystemCommonCash, int> configMap)
        {
            try
            {
                var registedValue = configMap;
                var id = registedValue[EnumSystemCommonCash.SceneId];
                registedValue[EnumSystemCommonCash.SceneId] = ++id;

                return registedValue;
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
                var tSResources = new MainTemplateResourcesAccessory();
                if (configMap == null)
                    throw new System.Exception("設定データがnull");
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
        /// メインシーンをロード
        /// </summary>
        public void LoadMainScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }

        /// <summary>
        /// セレクトシーンをロード
        /// </summary>
        public void LoadSelectScene()
        {
            SceneManager.LoadScene(backSceneName);
        }
    }
}
