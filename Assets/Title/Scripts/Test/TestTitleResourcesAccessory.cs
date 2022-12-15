using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Template;
using Title.Common;

namespace Title.Test
{
    /// <summary>
    /// テスト用
    /// タイトルシーンのリソース管理
    /// </summary>
    public class TestTitleResourcesAccessory : MonoBehaviour
    {
        [SerializeField] private int[] inputConfigDatas;
        [SerializeField] private bool testMode;

        private void Start()
        {
            new TitleTemplateResourcesAccessory().Initialize();
        }

        public void OnClicked()
        {
            if (testMode)
                TestCase_1();
        }

        public void TestCase_1()
        {
            Debug.Log("---OnClicked---");
            var tTResources = new TitleTemplateResourcesAccessory();
            Debug.Log("---LoadResourcesCSV---");
            var datas = tTResources.LoadSaveDatasCSV(ConstResorcesNames.SYSTEM_CONFIG);
            if (datas == null)
                throw new System.Exception("リソース読み込みの失敗");
            for (var i = 0; i < datas.Count; i++)
            {
                for (var j = 0; j < datas[i].Length; j++)
                {
                    Debug.Log(datas[i][j]);
                }
            }
            Debug.Log("---GetSystemConfig---");
            var configMap = tTResources.GetSystemConfig(datas);
            foreach (var map in configMap)
            {
                Debug.Log($"Key:{map.Key}_Val:{map.Value}");
            }
            Debug.Log("---SaveResourcesCSVOfSystemConfig---");
            //var configMap = new Dictionary<EnumSystemConfig, int>();
            var idx = 0;
            configMap[EnumSystemConfig.AudioVolumeIndex] = inputConfigDatas[idx++];
            configMap[EnumSystemConfig.BGMVolumeIndex] = inputConfigDatas[idx++];
            configMap[EnumSystemConfig.SEVolumeIndex] = inputConfigDatas[idx++];
            configMap[EnumSystemConfig.VibrationEnableIndex] = inputConfigDatas[idx++];
            if (!tTResources.SaveDatasCSVOfSystemConfig(ConstResorcesNames.SYSTEM_CONFIG, configMap))
                Debug.LogError("CSV保存呼び出しの失敗");
        }
    }
}
