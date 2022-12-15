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
        /// <summary>
        /// シーン読み込み
        /// </summary>
        /// <param name="name">シーン名</param>
        public void LoadSceneName(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}
