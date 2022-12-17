using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Title.Model;
using Title.View;
using Title.Common;
using Title.Audio;

namespace Title.Presenter
{
    /// <summary>
    /// プレゼンタ
    /// タイトルシーン
    /// </summary>
    public class TitlePresenter : MonoBehaviour, ITitleGameManager
    {
        /// <summary>PushGameStartのビュー</summary>
        [SerializeField] private PushGameStartLogoView pushGameStartLogoView;
        /// <summary>PushGameStartのモデル</summary>
        [SerializeField] private PushGameStartLogoModel pushGameStartLogoModel;
        /// <summary>GameStartLogoのビュー</summary>
        [SerializeField] private GameStartLogoView gameStartLogoView;
        /// <summary>GameStartLogoのモデル</summary>
        [SerializeField] private GameStartLogoModel gameStartLogoModel;
        /// <summary>GameExitLogoのビュー</summary>
        [SerializeField] private GameExitLogoView gameExitLogoView;
        /// <summary>GameExitLogoのモデル</summary>
        [SerializeField] private GameExitLogoModel gameExitLogoModel;
        /// <summary>GameExitConfirmYesLogoのビュー</summary>
        [SerializeField] private GameExitConfirmYesLogoView gameExitConfirmYesLogoView;
        /// <summary>GameExitConfirmYesLogoのモデル</summary>
        [SerializeField] private GameExitConfirmYesLogoModel gameExitConfirmYesLogoModel;
        /// <summary>GameExitConfirmNoLogoのビュー</summary>
        [SerializeField] private GameExitConfirmNoLogoView gameExitConfirmNoLogoView;
        /// <summary>GameExitConfirmNoLogoのモデル</summary>
        [SerializeField] private GameExitConfirmNoLogoModel gameExitConfirmNoLogoModel;
        /// <summary>CursorIconのビュー</summary>
        [SerializeField] private CursorIconView cursorIconView;
        /// <summary>CursorIconのモデル</summary>
        [SerializeField] private CursorIconModel cursorIconModel;
        /// <summary>FadeImageのビュー</summary>
        [SerializeField] private FadeImageView fadeImageView;
        /// <summary>Fadeimageのモデル</summary>
        [SerializeField] private FadeImageModel fadeImageModel;

        private void Reset()
        {
            pushGameStartLogoView = GameObject.Find("PushGameStartLogo").GetComponent<PushGameStartLogoView>();
            pushGameStartLogoModel = GameObject.Find("PushGameStartLogo").GetComponent<PushGameStartLogoModel>();
            gameStartLogoView = GameObject.Find("GameStartLogo").GetComponent<GameStartLogoView>();
            gameStartLogoModel = GameObject.Find("GameStartLogo").GetComponent<GameStartLogoModel>();
            gameExitLogoView = GameObject.Find("GameExitLogo").GetComponent<GameExitLogoView>();
            gameExitLogoModel = GameObject.Find("GameExitLogo").GetComponent<GameExitLogoModel>();
            gameExitConfirmYesLogoView = GameObject.Find("GameExitConfirmYesLogo").GetComponent<GameExitConfirmYesLogoView>();
            gameExitConfirmYesLogoModel = GameObject.Find("GameExitConfirmYesLogo").GetComponent<GameExitConfirmYesLogoModel>();
            gameExitConfirmNoLogoView = GameObject.Find("GameExitConfirmNoLogo").GetComponent<GameExitConfirmNoLogoView>();
            gameExitConfirmNoLogoModel = GameObject.Find("GameExitConfirmNoLogo").GetComponent<GameExitConfirmNoLogoModel>();
            cursorIconView = GameObject.Find("CursorIcon").GetComponent<CursorIconView>();
            cursorIconModel = GameObject.Find("CursorIcon").GetComponent<CursorIconModel>();
            fadeImageView = GameObject.Find("FadeImage").GetComponent<FadeImageView>();
            fadeImageModel = GameObject.Find("FadeImage").GetComponent<FadeImageModel>();
        }

        public void OnStart()
        {
            // 動的に制御するパネルをキャッシュ
            var pushGameStart = pushGameStartLogoView.transform.parent.gameObject;
            var gameStartOrExit = gameStartLogoView.transform.parent.gameObject;
            var gameExitConfirm = gameExitConfirmYesLogoView.transform.parent.gameObject;
            var cursorIcon = cursorIconView.gameObject;
            var fadeImage = fadeImageView.transform.parent.gameObject;
            // 初期設定
            pushGameStart.SetActive(false);
            gameStartOrExit.SetActive(false);
            gameExitConfirm.SetActive(false);
            cursorIcon.SetActive(false);
            fadeImage.SetActive(true);
            // シーン読み込み時のアニメーション
            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Open))
                .Subscribe(_ =>
                {
                    // UI操作を許可
                    pushGameStart.SetActive(true);
                    if (!pushGameStartLogoModel.OnStart())
                        Debug.LogError("PushGameStart開始イベント呼び出しの失敗");
                    // BGMを再生
                    TitleGameManager.Instance.AudioOwner.PlayBGM(ClipToPlayBGM.bgm_title);
                })
                .AddTo(gameObject);
            // プッシュゲームスタート
            pushGameStartLogoModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.AnyKeysPushed:
                            // ゲーム開始／終了UIを表示
                            pushGameStart.SetActive(false);
                            gameStartOrExit.SetActive(true);
                            if (!cursorIcon.activeSelf)
                                cursorIcon.SetActive(true);
                            if (!cursorIconView.SetSelect(gameStartLogoView.transform.position))
                                Debug.LogError("カーソル選択位置変更処理呼び出しの失敗");
                            // ゲームスタートSEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_game_start);
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // カーソル
            cursorIconView.IsAnimationPlaying.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        // ボタン制御を無効
                        gameStartLogoModel.SetButtonEnabled(false);
                        gameStartLogoModel.SetEventTriggerEnabled(false);
                        gameExitLogoModel.SetButtonEnabled(false);
                        gameExitConfirmYesLogoModel.SetButtonEnabled(false);
                        gameExitConfirmYesLogoModel.SetEventTriggerEnabled(false);
                        gameExitConfirmNoLogoModel.SetButtonEnabled(false);
                    }
                    else
                    {
                        // ボタン制御を有効
                        gameStartLogoModel.SetButtonEnabled(true);
                        gameStartLogoModel.SetEventTriggerEnabled(true);
                        gameExitLogoModel.SetButtonEnabled(true);
                        gameExitConfirmYesLogoModel.SetButtonEnabled(true);
                        gameExitConfirmYesLogoModel.SetEventTriggerEnabled(true);
                        gameExitConfirmNoLogoModel.SetButtonEnabled(true);
                    }
                });
            // ゲームを開始
            gameStartLogoModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Selected:
                            if (!cursorIconView.PlaySelectAnimation(gameStartLogoView.transform.position))
                                Debug.LogError("カーソル選択アニメーション呼び出しの失敗");
                            // 選択SEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            break;
                        case EnumEventCommand.Canceled:
                            cursorIcon.SetActive(false);
                            gameStartOrExit.SetActive(false);
                            pushGameStart.SetActive(true);
                            // キャンセルSEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                            break;
                        case EnumEventCommand.Submited:
                            gameStartLogoModel.SetButtonEnabled(false);
                            gameStartLogoModel.SetEventTriggerEnabled(false);
                            // 決定SEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                            // ステージセレクトへの遷移を実装
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ => TitleGameManager.Instance.SceneOwner.LoadNextScene())
                                .AddTo(gameObject);
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // ゲームを終了
            gameExitLogoModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Selected:
                            if (!cursorIconView.PlaySelectAnimation(gameExitLogoView.transform.position))
                                Debug.LogError("カーソル選択アニメーション呼び出しの失敗");
                            // 選択SEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            break;
                        case EnumEventCommand.Canceled:
                            cursorIcon.SetActive(false);
                            gameStartOrExit.SetActive(false);
                            pushGameStart.SetActive(true);
                            // キャンセルSEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                            break;
                        case EnumEventCommand.Submited:
                            gameStartOrExit.SetActive(false);
                            gameExitConfirm.SetActive(true);
                            if (!cursorIconView.SetSelect(gameExitConfirmYesLogoView.transform.position))
                                Debug.LogError("カーソル選択位置変更処理呼び出しの失敗");
                            // 決定SEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // ゲームを終了しますか？　＞　はい
            gameExitConfirmYesLogoModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Selected:
                            if (!cursorIconView.PlaySelectAnimation(gameExitConfirmYesLogoView.transform.position))
                                Debug.LogError("カーソル選択アニメーション呼び出しの失敗");
                            // 選択SEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            break;
                        case EnumEventCommand.Canceled:
                            gameExitConfirm.SetActive(false);
                            gameStartOrExit.SetActive(true);
                            if (!cursorIconView.SetSelect(gameStartLogoView.transform.position))
                                Debug.LogError("カーソル選択位置変更処理呼び出しの失敗");
                            // キャンセルSEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                            break;
                        case EnumEventCommand.Submited:
                            gameExitConfirmYesLogoModel.SetButtonEnabled(false);
                            gameExitConfirmYesLogoModel.SetEventTriggerEnabled(false);
                            // 決定SEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_decided);
                            Observable.FromCoroutine<bool>(observer => fadeImageView.PlayFadeAnimation(observer, EnumFadeState.Close))
                                .Subscribe(_ => TitleGameManager.Instance.CallGameExit())
                                .AddTo(gameObject);
                            break;
                        default:
                            Debug.LogWarning("例外ケース");
                            break;
                    }
                });
            // ゲームを終了しますか？　＞　いいえ
            gameExitConfirmNoLogoModel.EventState.ObserveEveryValueChanged(x => x.Value)
                .Subscribe(x =>
                {
                    switch ((EnumEventCommand)x)
                    {
                        case EnumEventCommand.Selected:
                            if (!cursorIconView.PlaySelectAnimation(gameExitConfirmNoLogoView.transform.position))
                                Debug.LogError("カーソル選択アニメーション呼び出しの失敗");
                            // 選択SEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_select);
                            break;
                        case EnumEventCommand.Canceled:
                            gameExitConfirm.SetActive(false);
                            gameStartOrExit.SetActive(true);
                            if (!cursorIconView.SetSelect(gameStartLogoView.transform.position))
                                Debug.LogError("カーソル選択位置変更処理呼び出しの失敗");
                            // キャンセルSEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                            break;
                        case EnumEventCommand.Submited:
                            gameExitConfirm.SetActive(false);
                            gameStartOrExit.SetActive(true);
                            if (!cursorIconView.SetSelect(gameStartLogoView.transform.position))
                                Debug.LogError("カーソル選択位置変更処理呼び出しの失敗");
                            // キャンセルSEを再生
                            TitleGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_cancel);
                            break;
                        default:
                            //Debug.LogWarning("例外ケース");
                            break;
                    }
                });
        }
    }
}
