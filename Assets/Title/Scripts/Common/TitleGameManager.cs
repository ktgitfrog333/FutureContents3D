using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Title.Presenter;
using Title.Audio;

namespace Title.Common
{
    /// <summary>
    /// タイトルのゲームマネージャー
    /// </summary>
    public class TitleGameManager : MonoBehaviour
    {
        /// <summary>タイトルのゲームマネージャー</summary>
        private static TitleGameManager _instance;
        /// <summary>タイトルのゲームマネージャー</summary>
        public static TitleGameManager Instance => _instance;
        /// <summary>プレゼンタ</summary>
        [SerializeField] private TitlePresenter presenter;
        /// <summary>プレゼンタ</summary>
        public TitlePresenter Presenter => presenter;
        /// <summary>オーディオのオーナー</summary>
        [SerializeField] private AudioOwner audioOwner;
        /// <summary>オーディオのオーナー</summary>
        public AudioOwner AudioOwner => audioOwner;
        /// <summary>シーンオーナー</summary>
        [SerializeField] private SceneOwner sceneOwner;
        /// <summary>シーンオーナー</summary>
        public SceneOwner SceneOwner => sceneOwner;
        /// <summary>カーソル表示</summary>
        [SerializeField] private CursolVisible cursolVisible;
        /// <summary>カーソル表示</summary>
        public CursolVisible CursolVisible => cursolVisible;

        private void Reset()
        {
            presenter = GameObject.Find("Presenter").GetComponent<TitlePresenter>();
            audioOwner = GameObject.Find("AudioOwner").GetComponent<AudioOwner>();
            sceneOwner = GameObject.Find("SceneOwner").GetComponent<SceneOwner>();
            cursolVisible = GameObject.Find("CursolVisible").GetComponent<CursolVisible>();
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        private void Start()
        {
            audioOwner.OnStart();
            presenter.OnStart();
            cursolVisible.OnStart();
        }

        /// <summary>
        /// ゲームを終了
        /// </summary>
        public void CallGameExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
        }
    }

    /// <summary>
    /// ゲームマネージャーとのインターフェース
    /// </summary>
    public interface ITitleGameManager
    {
        /// <summary>
        /// オンスタート
        /// </summary>
        void OnStart();
    }
}
