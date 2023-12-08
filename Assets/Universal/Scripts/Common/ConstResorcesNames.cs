using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Universal.Common
{
    /// <summary>
    /// リソースフォルダ内のファイル名
    /// </summary>
    public class ConstResorcesNames
    {
        /// <summary>ホームディレクトリ（UnityEditor用）</summary>
        public static readonly string HOMEPATH_UNITYEDITOR = @".\Assets\SaveDatas\";
        /// <summary>ホームディレクトリ（ビルド用）</summary>
        public static readonly string HOMEPATH_BUILD = @".\SaveDatas\";
        /// <summary>システム設定キャッシュ</summary>
        public static readonly string SYSTEM_COMMON_CASH = "SystemCommonCash";
        /// <summary>システム設定</summary>
        public static readonly string SYSTEM_CONFIG = "SystemConfig";
        /// <summary>ステージクリア済みデータ</summary>
        public static readonly string MAIN_SCENE_STAGES_STATE = "MainSceneStagesState";
        /// <summary>ユーザーデータ</summary>
        public static readonly string USER_DATA = "UserData";
        /// <summary>管理者データ</summary>
        public static readonly string ADMIN_DATA = "AdminData";
        /// <summary>＋で全ステージ解放</summary>
        public static readonly string ALL = "All";
    }
}
