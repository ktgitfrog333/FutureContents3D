using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Select.Common;
using Select.View;
using Select.Model;
using UniRx;
using UniRx.Triggers;
using Select.Audio;
using System.Linq;

namespace Select.Presenter
{
    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// </summary>
    public class SelectPresenter : MonoBehaviour, ISelectGameManager
    {
        /// <summary>ページのビュー</summary>
        [SerializeField] private PageView[] pageViews;
        /// <summary>ページのモデル</summary>
        [SerializeField] private PageModel[] pageModels;
        /// <summary>ロゴステージのビュー</summary>
        [SerializeField] private LogoStageView[] logoStageViews;
        /// <summary>ロゴステージのモデル</summary>
        [SerializeField] private LogoStageModel[] logoStageModels;
        /// <summary>ロゴカーソルのビュー</summary>
        [SerializeField] private LogoCursorView[] logoCursorViews;
        /// <summary>ロゴカーソルのモデル</summary>
        [SerializeField] private LogoCursorModel[] logoCursorModels;
        /// <summary>コメントテキストのビュー</summary>
        [SerializeField] private CommentTextView[] commentTextViews;
        /// <summary>コメントテキストのモデル</summary>
        [SerializeField] private CommentTextModel[] commentTextModels;
        /// <summary>FadeImageのビュー</summary>
        [SerializeField] private FadeImageView fadeImageView;
        /// <summary>Fadeimageのモデル</summary>
        [SerializeField] private FadeImageModel fadeImageModel;
        /// <summary>1ページあたりのコンテンツ数</summary>
        [SerializeField] private int contentsCountInPage = 5;
        /// <summary>ステージ選択のフレーム</summary>
        [SerializeField] private Transform selectStageFrame;

        private void Reset()
        {
            var logoStages = GameObject.Find("LogoStages").transform;

            // ページのビューとモデルをセット
            List<PageView> pageViewList = new List<PageView>();
            List<PageModel> pageModelList = new List<PageModel>();
            foreach (Transform child in logoStages)
            {
                pageViewList.Add(child.GetComponent<PageView>());
                pageModelList.Add(child.GetComponent<PageModel>());
            }
            pageViews = pageViewList.ToArray();
            pageModels = pageModelList.ToArray();

            // ロゴステージのビューとモデルをセット
            List<LogoStageView> logoStageViewList = new List<LogoStageView>();
            List<LogoStageModel> logoStageModelList = new List<LogoStageModel>();
            foreach (Transform pages in logoStages)
            {
                foreach (Transform child in pages)
                {
                    logoStageViewList.Add(child.GetComponent<LogoStageView>());
                    logoStageModelList.Add(child.GetComponent<LogoStageModel>());
                }
            }
            logoStageViews = logoStageViewList.ToArray();
            logoStageModels = logoStageModelList.ToArray();

            var logoCursors = GameObject.Find("LogoCursors").transform;

            // ロゴカーソルのビューとモデルをセット
            List<LogoCursorView> logoCursorViewList = new List<LogoCursorView>();
            List<LogoCursorModel> logoCursorModelList = new List<LogoCursorModel>();
            foreach (Transform child in logoCursors)
            {
                logoCursorViewList.Add(child.GetComponent<LogoCursorView>());
                logoCursorModelList.Add(child.GetComponent<LogoCursorModel>());
            }
            logoCursorViews = logoCursorViewList.ToArray();
            logoCursorModels = logoCursorModelList.ToArray();

            var selectFrame = GameObject.Find("SelectFrame").transform;

            // コメントテキストのビューとモデルをセット
            List<CommentTextView> commentTextViewList = new List<CommentTextView>();
            List<CommentTextModel> commentTextModelList = new List<CommentTextModel>();
            foreach (Transform child in selectFrame)
            {
                commentTextViewList.Add(child.GetComponent<CommentTextView>());
                commentTextModelList.Add(child.GetComponent<CommentTextModel>());
            }
            commentTextViews = commentTextViewList.ToArray();
            commentTextModels = commentTextModelList.ToArray();

            // フェードのビューとモデルをセット
            fadeImageView = GameObject.Find("FadeImage").GetComponent<FadeImageView>();
            fadeImageModel = GameObject.Find("FadeImage").GetComponent<FadeImageModel>();

            selectStageFrame = GameObject.Find("SelectStageFrame").transform;
        }

        public void OnStart()
        {
            // 初期設定
            foreach (var child in logoStageModels)
                if (child != null)
                    child.SetButtonEnabled(false);
            foreach (var child in pageViews)
                if (child != null)
                    child.SetVisible(false);

            // シーン読み込み時のアニメーション
            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ =>
                {
                    // UI操作を許可
                    foreach (var child in logoStageModels)
                        if (child != null)
                            child.SetButtonEnabled(true);
                    // BGMを再生
                    SelectGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_select);
                })
                .AddTo(gameObject);

            // T.B.D ステージ番号を取得する処理を追加する
            var stageIndex = new IntReactiveProperty(1);
            logoStageModels[1].SetSelectedGameObject();
            // 選択ステージ番号の更新
            stageIndex.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // ページ表示切り替え
                    var pageIdx = ((x - 1) / contentsCountInPage) + 1;
                    if (!pageViews[pageIdx].SetVisible(true))
                        Debug.LogError("アルファ値切り替え処理の失敗");
                    // ロゴステージ表示切り替え
                    if (!logoStageViews[x].MoveSelectStageFrame())
                        Debug.LogError("ステージ選択のフレーム移動の失敗");
                    // ロゴカーソル表示切り替え
                    if (1 < pageIdx && pageIdx < pageModels.Length - 1)
                    {
                        // 見開きページ
                        if (!logoCursorViews[(int)EnumLogoCursor.Left].SetImageEnabled(true))
                            Debug.LogError("イメージ有効／無効処理の失敗");
                        if (!logoCursorViews[(int)EnumLogoCursor.Right].SetImageEnabled(true))
                            Debug.LogError("イメージ有効／無効処理の失敗");
                    }
                    else if (pageIdx < 2)
                    {
                        // 最初のページ
                        if (!logoCursorViews[(int)EnumLogoCursor.Left].SetImageEnabled(false))
                            Debug.LogError("イメージ有効／無効処理の失敗");
                        if (!logoCursorViews[(int)EnumLogoCursor.Right].SetImageEnabled(true))
                            Debug.LogError("イメージ有効／無効処理の失敗");
                    }
                    else
                    {
                        // 最後のページ
                        if (!logoCursorViews[(int)EnumLogoCursor.Left].SetImageEnabled(true))
                            Debug.LogError("イメージ有効／無効処理の失敗");
                        if (!logoCursorViews[(int)EnumLogoCursor.Right].SetImageEnabled(false))
                            Debug.LogError("イメージ有効／無効処理の失敗");
                    }
                    // コメントテキスト表示切り替え
                    foreach (var child in commentTextViews.Where(q => q != null && q.TextIsActiveAndEnabled).Select(q => q))
                        if (!child.SetTextEnabled(false))
                            Debug.LogError("テキスト表示切り替え処理の失敗");
                    if (!commentTextViews[x].SetTextEnabled(true))
                        Debug.LogError("テキスト表示切り替え処理の失敗");
                });

            // ステージ選択の操作
            foreach (var child in logoStageModels.Where(q => q != null).Select(q => q))
            {
                child.EventState.ObserveEveryValueChanged(x => x.Value)
                    .Subscribe(x =>
                    {
                        switch ((EnumEventCommand)x)
                        {
                            case EnumEventCommand.Selected:
                                stageIndex.Value = child.Index;
                                break;
                            case EnumEventCommand.Submited:
                                // T.B.D メインシーンへの遷移
                                break;
                            case EnumEventCommand.Canceled:
                                // T.B.D タイトルシーンへの遷移
                                break;
                            default:
                                Debug.LogWarning("例外ケース");
                                break;
                        }
                    });
            }
        }
    }
}
