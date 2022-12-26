using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main.Common;
using Main.View;
using Main.Model;
using UniRx;
using UniRx.Triggers;
using Main.Audio;
using System.Linq;
using Main.InputSystem;
using DG.Tweening;
using System.Threading.Tasks;

namespace Main.Presenter
{
    /// <summary>
    /// プレゼンタ
    /// セレクトシーン
    /// </summary>
    public class MainPresenter : MonoBehaviour, IMainGameManager
    {
        /// <summary>ポーズ画面のビュー</summary>
        [SerializeField] private PauseView pauseView;
        /// <summary>ポーズボタンのビュー</summary>
        [SerializeField] private GamePauseView gamePauseView;
        /// <summary>ポーズボタンのモデル</summary>
        [SerializeField] private GamePauseModel gamePauseModel;
        /// <summary>クリア画面のビュー</summary>
        [SerializeField] private ClearView clearView;
        /// <summary>ステージクリアロゴのビュー</summary>
        [SerializeField] private StageClearView stageClearView;
        /// <summary>クリア画面のメニュー描画までの時間</summary>
        [SerializeField] private int clearContentsRenderingDelayTime = 3000;
        /// <summary>次のステージへ進むのビュー</summary>
        [SerializeField] private GameProceedButtonView gameProceedButtonView;
        /// <summary>次のステージへ進むのモデル</summary>
        [SerializeField] private GameProceedButtonModel gameProceedButtonModel;
        /// <summary>もう一度遊ぶボタンのビュー</summary>
        [SerializeField] private GameRetryButtonView gameRetryButtonView;
        /// <summary>もう一度遊ぶボタンのモデル</summary>
        [SerializeField] private GameRetryButtonModel gameRetryButtonModel;
        /// <summary>ステージ選択へ戻るのビュー</summary>
        [SerializeField] private GameSelectButtonView gameSelectButtonView;
        /// <summary>ステージ選択へ戻るのモデル</summary>
        [SerializeField] private GameSelectButtonModel gameSelectButtonModel;
        /// <summary>カーソルのビュー</summary>
        [SerializeField] private CursorIconView cursorIconView;
        /// <summary>カーソルのモデル</summary>
        [SerializeField] private CursorIconModel cursorIconModel;
        /// <summary>ショートカットキー押下ゲージのビュー</summary>
        [SerializeField] private PushTimeGageView[] pushTimeGageViews;
        /// <summary>ショートカットキー押下ゲージのビュー</summary>
        [SerializeField] private GameManualScrollView gameManualScrollView;
        /// <summary>遊び方確認ページのビュー</summary>
        [SerializeField] private GameManualViewPageView[] gameManualViewPageViews;
        /// <summary>遊び方確認ページのモデル</summary>
        [SerializeField] private GameManualViewPageModel[] gameManualViewPageModels;
        /// <summary>移動操作ガイドのビュー</summary>
        [SerializeField] private MoveGuideView moveGuideView;
        /// <summary>ジャンプ操作ガイドのビュー</summary>
        [SerializeField] private JumpGuideView jumpGuideView;
        /// <summary>フェードのビュー</summary>
        [SerializeField] private FadeImageView fadeImageView;
        /// <summary>フェードのモデル</summary>
        [SerializeField] private FadeImageModel fadeImageModel;

        private void Reset()
        {
            pauseView = GameObject.Find("Pause").GetComponent<PauseView>();
            gamePauseView = GameObject.Find("GamePause").GetComponent<GamePauseView>();
            gamePauseModel = GameObject.Find("GamePause").GetComponent<GamePauseModel>();
            clearView = GameObject.Find("Clear").GetComponent<ClearView>();
            stageClearView = GameObject.Find("StageClear").GetComponent<StageClearView>();
            gameProceedButtonView = GameObject.Find("GameProceedButton").GetComponent<GameProceedButtonView>();
            gameProceedButtonModel = GameObject.Find("GameProceedButton").GetComponent<GameProceedButtonModel>();
            gameRetryButtonView = GameObject.Find("GameRetryButton").GetComponent<GameRetryButtonView>();
            gameRetryButtonModel = GameObject.Find("GameRetryButton").GetComponent<GameRetryButtonModel>();
            gameSelectButtonView = GameObject.Find("GameSelectButton").GetComponent<GameSelectButtonView>();
            gameSelectButtonModel = GameObject.Find("GameSelectButton").GetComponent<GameSelectButtonModel>();
            cursorIconView = GameObject.Find("CursorIcon").GetComponent<CursorIconView>();
            cursorIconModel = GameObject.Find("CursorIcon").GetComponent<CursorIconModel>();
            var ptGameIdx = 0;
            pushTimeGageViews = new PushTimeGageView[3];
            pushTimeGageViews[ptGameIdx++] = GameObject.Find("GULPushTimeGage").GetComponent<PushTimeGageView>();
            pushTimeGageViews[ptGameIdx++] = GameObject.Find("GSLPushTimeGage").GetComponent<PushTimeGageView>();
            pushTimeGageViews[ptGameIdx++] = GameObject.Find("GCLPushTimeGage").GetComponent<PushTimeGageView>();
            gameManualScrollView = GameObject.Find("GameManualScroll").GetComponent<GameManualScrollView>();
            var gmvPageVIdx = 0;
            var gmvPageMIdx = 0;
            gameManualViewPageViews = new GameManualViewPageView[4];
            gameManualViewPageModels = new GameManualViewPageModel[4];
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_1").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_1").GetComponent<GameManualViewPageModel>();
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_2").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_2").GetComponent<GameManualViewPageModel>();
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_3").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_3").GetComponent<GameManualViewPageModel>();
            gameManualViewPageViews[gmvPageVIdx++] = GameObject.Find("GameManualViewPage_4").GetComponent<GameManualViewPageView>();
            gameManualViewPageModels[gmvPageMIdx++] = GameObject.Find("GameManualViewPage_4").GetComponent<GameManualViewPageModel>();
            moveGuideView = GameObject.Find("MoveGuide").GetComponent<MoveGuideView>();
            jumpGuideView = GameObject.Find("JumpGuide").GetComponent<JumpGuideView>();
            fadeImageView = GameObject.Find("FadeImage").GetComponent<FadeImageView>();
            fadeImageModel = GameObject.Find("FadeImage").GetComponent<FadeImageModel>();
        }

        public void OnStart()
        {
            // 初期設定
            pauseView.gameObject.SetActive(false);
            gameProceedButtonView.gameObject.SetActive(false);
            gameRetryButtonView.gameObject.SetActive(false);
            gameSelectButtonView.gameObject.SetActive(false);
            cursorIconView.gameObject.SetActive(false);
            clearView.gameObject.SetActive(false);
            gameManualScrollView.gameObject.SetActive(false);
            moveGuideView.gameObject.SetActive(false);
            jumpGuideView.gameObject.SetActive(false);

            MainGameManager.Instance.AudioOwner.OnStartAndPlayBGM();
            // T.B.D ステージ開始演出
            var isStartDirectionCompleted = new BoolReactiveProperty();
            // シーン読み込み時のアニメーション
            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ =>
                {
                    // T.B.D ステージ開始演出
                    isStartDirectionCompleted.Value = true;
                })
                .AddTo(gameObject);
            // T.B.D ステージ開始演出
            isStartDirectionCompleted.ObserveEveryValueChanged(x => x.Value)
                .Where(x => x)
                .Subscribe(_ =>
                {
                    // T.B.D プレイヤーを開始ポイントへ生成
                });

            // ポーズ押下
            var inputUIPausedState = new BoolReactiveProperty();
            inputUIPausedState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    // ポーズ画面が閉じている　かつ、
                    // クリア画面が閉じている
                    if (x &&
                        !pauseView.gameObject.activeSelf &&
                        !clearView.gameObject.activeSelf)
                    {
                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_play_open);
                        // 遊び方確認ページを開いているなら閉じる
                        if (gameManualScrollView.gameObject.activeSelf)
                        {
                            // T.B.D 遊び方確認ページを閉じる
                        }
                        pauseView.gameObject.SetActive(true);
                        gamePauseModel.SetSelectedGameObject();
                    }
                });
            // ポーズ画面表示中の操作
            gamePauseModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理無し
                            break;
                        case EnumEventCommand.Selected:
                            // 処理無し
                            break;
                        case EnumEventCommand.DeSelected:
                            // 処理無し
                            break;
                        case EnumEventCommand.Submited:
                            // 処理無し
                            break;
                        case EnumEventCommand.Canceled:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                            if (!gamePauseModel.SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gamePauseModel.SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // ポーズ画面クローズのアニメーション
                            Observable.FromCoroutine<bool>(observer => pauseView.PlayCloseAnimation(observer))
                                .Subscribe(_ =>
                                {
                                    pauseView.gameObject.SetActive(false);
                                })
                                .AddTo(gameObject);
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // T.B.D クリア画面表示のため、ゴール到達のフラグ更新
            var isGoalReached = new BoolReactiveProperty();
            // ※※※確認用※※※
            //DOVirtual.DelayedCall(3f, () =>
            //{
            //    isGoalReached.Value = true;
            //});
            isGoalReached.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(async x =>
                {
                    if (x)
                    {
                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.me_game_clear);
                        // 初期処理
                        clearView.gameObject.SetActive(true);
                        stageClearView.gameObject.SetActive(true);
                        gameProceedButtonView.gameObject.SetActive(false);
                        gameRetryButtonView.gameObject.SetActive(false);
                        gameSelectButtonView.gameObject.SetActive(false);
                        // 一定時間後に表示するUI
                        await Task.Delay(clearContentsRenderingDelayTime);
                        gameProceedButtonView.gameObject.SetActive(true);
                        // 初回のみ最初から拡大表示
                        gameProceedButtonView.SetScale();
                        gameRetryButtonView.gameObject.SetActive(true);
                        gameSelectButtonView.gameObject.SetActive(true);
                        cursorIconView.gameObject.SetActive(true);
                        // 初回のみ最初から選択状態
                        gameProceedButtonModel.SetSelectedGameObject();
                    }
                });
            var currentStageDic = MainGameManager.Instance.SceneOwner.GetSystemCommonCash();

            // クリア画面 -> 次のステージへ進む
            gameProceedButtonModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理無し
                            break;
                        case EnumEventCommand.Selected:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            if (!gameProceedButtonView.PlayScaleUpAnimation())
                                Debug.LogError("拡大アニメーション呼び出しの失敗");
                            if (!cursorIconView.PlaySelectAnimation(gameProceedButtonView.transform.position))
                                Debug.LogError("カーソル移動アニメーション呼び出しの失敗");
                            break;
                        case EnumEventCommand.DeSelected:
                            if (!gameProceedButtonView.SetDefaultScale())
                                Debug.LogError("デフォルトサイズへ変更呼び出しの失敗");
                            break;
                        case EnumEventCommand.Submited:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                            if (!gameProceedButtonModel.SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gameProceedButtonModel.SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            var owner = MainGameManager.Instance.SceneOwner;
                            if (!owner.SetSystemCommonCash(owner.CountUpSceneId(currentStageDic)))
                                Debug.LogError("シーンID更新呼び出しの失敗");
                            // シーン読み込み時のアニメーション
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ => owner.LoadMainScene())
                                .AddTo(gameObject);
                            break;
                        case EnumEventCommand.Canceled:
                            // 処理無し
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // クリア画面 -> もう一度遊ぶ
            gameRetryButtonModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理無し
                            break;
                        case EnumEventCommand.Selected:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            if (!gameRetryButtonView.PlayScaleUpAnimation())
                                Debug.LogError("拡大アニメーション呼び出しの失敗");
                            if (!cursorIconView.PlaySelectAnimation(gameRetryButtonView.transform.position))
                                Debug.LogError("カーソル移動アニメーション呼び出しの失敗");
                            break;
                        case EnumEventCommand.DeSelected:
                            if (!gameRetryButtonView.SetDefaultScale())
                                Debug.LogError("デフォルトサイズへ変更呼び出しの失敗");
                            break;
                        case EnumEventCommand.Submited:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_retry);
                            if (!gameRetryButtonModel.SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gameRetryButtonModel.SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // シーン読み込み時のアニメーション
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadMainScene())
                                .AddTo(gameObject);
                            break;
                        case EnumEventCommand.Canceled:
                            // 処理無し
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // クリア画面 -> ステージ選択画面へ戻る
            gameSelectButtonModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Default:
                            // 処理無し
                            break;
                        case EnumEventCommand.Selected:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            if (!gameSelectButtonView.PlayScaleUpAnimation())
                                Debug.LogError("拡大アニメーション呼び出しの失敗");
                            if (!cursorIconView.PlaySelectAnimation(gameSelectButtonView.transform.position))
                                Debug.LogError("カーソル移動アニメーション呼び出しの失敗");
                            break;
                        case EnumEventCommand.DeSelected:
                            if (!gameSelectButtonView.SetDefaultScale())
                                Debug.LogError("デフォルトサイズへ変更呼び出しの失敗");
                            break;
                        case EnumEventCommand.Submited:
                            MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                            if (!gameSelectButtonModel.SetButtonEnabled(false))
                                Debug.LogError("ボタン有効／無効切り替え呼び出しの失敗");
                            if (!gameSelectButtonModel.SetEventTriggerEnabled(false))
                                Debug.LogError("イベント有効／無効切り替え呼び出しの失敗");
                            // シーン読み込み時のアニメーション
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ => MainGameManager.Instance.SceneOwner.LoadSelectScene())
                                .AddTo(gameObject);
                            break;
                        case EnumEventCommand.Canceled:
                            // 処理無し
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    inputUIPausedState.Value = MainGameManager.Instance.InputSystemsOwner.GetComponent<InputSystemsOwner>().InputUI.Paused;
                });
        }
    }
}
