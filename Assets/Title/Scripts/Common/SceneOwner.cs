using System.Collections;
using System.Collections.Generic;
using Title.Template;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Title.Common
{
    /// <summary>
    /// シーンオーナー
    /// </summary>
    public class SceneOwner : MonoBehaviour, ITitleGameManager
    {
        /// <summary>次のシーン名</summary>
        [SerializeField] private string nextSceneName = "SelectScene";

        public void OnStart()
        {
            new TitleTemplateResourcesAccessory().Initialize();
        }

        /// <summary>
        /// ステージクリア済みデータの削除
        /// </summary>
        /// <returns>成功／失敗</returns>
        public bool DestroyMainSceneStagesState()
        {
            try
            {
                var tTResources = new TitleTemplateResourcesAccessory();
                var datas = tTResources.LoadResourcesCSV(ConstResorcesNames.MAIN_SCENE_STAGES_STATE);
                var configMaps = tTResources.GetMainSceneStagesState(datas);
                if (!tTResources.SaveDatasCSVOfMainSceneStagesState(ConstResorcesNames.MAIN_SCENE_STAGES_STATE, configMaps))
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
        /// シーン読み込み
        /// </summary>
        /// <param name="name">シーン名</param>
        public void LoadNextScene()
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
