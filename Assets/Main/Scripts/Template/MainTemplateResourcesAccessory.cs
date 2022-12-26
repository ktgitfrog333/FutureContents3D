using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Accessory;
using Main.Common;

namespace Main.Template
{
    /// <summary>
    /// リソースアクセスのテンプレート
    /// タイトル用
    /// </summary>
    public class MainTemplateResourcesAccessory
    {
        /// <summary>
        /// 初期処理
        /// </summary>
        public void Initialize()
        {
            new MainResourcesAccessory().Initialize();
        }

        /// <summary>
        /// タイトルカラム名を含むCSVデータの取得
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <returns>二次元配列の文字列データ</returns>
        public List<string[]> LoadSaveDatasCSV(string resourcesLoadName)
        {
            return new MainResourcesAccessory().LoadSaveDatasCSV(resourcesLoadName);
        }

        /// <summary>
        /// システムオプション設定をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumSystemCommonCash, int> GetSystemConfig(List<string[]> datas)
        {
            return new MainResourcesAccessory().GetSystemConfig(datas);
        }

        /// <summary>
        /// ステージ設定をオブジェクトへ一時セット
        /// </summary>
        /// <param name="datas">二次元配列の文字列データ</param>
        /// <returns>格納オブジェクト</returns>
        public Dictionary<EnumMainSceneStagesConfig, int>[] GetMainSceneStagesConfig(List<string[]> datas)
        {
            return new MainResourcesAccessory().GetMainSceneStagesConfig(datas);
        }

        /// <summary>
        /// システムオプション設定をCSVデータへ保存
        /// </summary>
        /// <param name="resourcesLoadName">リソースCSVファイル名</param>
        /// <param name="configMap">格納オブジェクト</param>
        /// <returns>成功／失敗</returns>
        public bool SaveDatasCSVOfSystemConfig(string resourcesLoadName, Dictionary<EnumSystemCommonCash, int> configMap)
        {
            return new MainResourcesAccessory().SaveDatasCSVOfSystemConfig(resourcesLoadName, configMap);
        }
    }
}
