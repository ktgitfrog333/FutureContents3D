using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Template;
using Universal.Common;
using Select.Common;

namespace Select.Test
{
    /// <summary>
    /// テスト用
    /// セレクトシーンのリソース管理
    /// </summary>
    public class TestSelectResourcesAccessory : MonoBehaviour
    {
        [SerializeField] private int[] inputConfigDatas;
        [SerializeField] private bool testMode;

        private void Start()
        {
            new TemplateResourcesAccessory();
        }

        public void OnClicked()
        {
            if (testMode)
                TestCase_1();
        }

        public void TestCase_1()
        {
            Debug.Log("---OnClicked---");
            var tSResources = new TemplateResourcesAccessory();
            Debug.Log("---LoadResourcesCSV---");
            var datas = tSResources.LoadSaveDatasJsonOfUserBean(ConstResorcesNames.USER_DATA);
            if (datas == null)
                throw new System.Exception("リソース読み込みの失敗");
            // for (var i = 0; i < datas.Count; i++)
            // {
            //     for (var j = 0; j < datas[i].Length; j++)
            //     {
            //         Debug.Log(datas[i][j]);
            //     }
            // }
            // Debug.Log("---GetSystemCommonCash---");
            // var configMap = tSResources.GetSystemCommonCash(datas);
            // foreach (var map in configMap)
            // {
            //     Debug.Log($"Key:{map.Key}_Val:{map.Value}");
            // }
            // Debug.Log("---SaveResourcesCSVOfSystemCommonCash---");
            // //var configMap = new Dictionary<EnumSystemConfig, int>();
            // var idx = 0;
            // configMap[EnumSystemCommonCash.SceneId] = inputConfigDatas[idx++];
            // if (!tSResources.SaveDatasCSVOfSystemCommonCash(ConstResorcesNames.SYSTEM_COMMON_CASH, configMap))
            //     Debug.LogError("CSV保存呼び出しの失敗");
        }
    }
}
