using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Title.Common
{
    /// <summary>
    /// シーンオーナー
    /// </summary>
    public class SceneOwner : MonoBehaviour
    {
        /// <summary>次のシーン名</summary>
        [SerializeField] private string nextSceneName = "SelectScene";

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
