using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Main.Common;
using Main.Audio;
using DG.Tweening;

namespace Main.Model
{
    /// <summary>
    /// モデル
    /// プレイヤー
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerModel : LevelPhysicsSerializerCapsule
    {
        /// <summary>移動速度</summary>
        [SerializeField] private float moveSpeed = 4f;
        /// <summary>ジャンプ速度</summary>
        [SerializeField] private float jumpSpeed = 6f;
        /// <summary>操作禁止フラグ</summary>
        private bool _inputBan;
        /// <summary>操作禁止フラグ</summary>
        public bool InputBan => _inputBan;

        /// <summary>
        /// 操作禁止フラグをセット
        /// </summary>
        /// <param name="unactive">許可／禁止</param>
        /// <returns>成功／失敗</returns>
        public bool SetInputBan(bool unactive)
        {
            try
            {
                _inputBan = unactive;
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void Start()
        {
            // Rigidbody
            var rigidbody = GetComponent<Rigidbody>();
            // 移動制御のベロシティ
            var moveVelocity = new Vector3();
            // 位置・スケールのキャッシュ
            var tForm = transform;
            // ジャンプフラグ
            var isJumped = false;
            // 移動入力に応じて移動座標をセット
            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (!_inputBan)
                    {
                        moveVelocity = new Vector3(MainGameManager.Instance.InputSystemsOwner.InputPlayer.Moved.x, moveVelocity.y, moveVelocity.z) * moveSpeed * (1f + Time.deltaTime);
                        footerPoint = gameObject.transform.position - Vector3.up * 0.25f;
                        headerPoint = gameObject.transform.position + Vector3.up * 0.25f;
                        if (!isJumped &&
                            MainGameManager.Instance.InputSystemsOwner.InputPlayer.Jumped &&
                            Physics.CheckCapsule(footerPoint, headerPoint, radius, LayerMask.GetMask(ConstLayerNames.LAYER_NAME_FLOOR)))
                        {
                            isJumped = true;
                        }
                        // 空中のみ重力が有効（Y軸引力にAddForceのX軸制御が負けるため）
                        rigidbody.useGravity = !Physics.CheckCapsule(footerPoint, headerPoint, radius, LayerMask.GetMask(ConstLayerNames.LAYER_NAME_FLOOR));
                    }
                });
            // 移動制御
            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (isJumped)
                    {
                        moveVelocity = new Vector3(moveVelocity.x, jumpSpeed, moveVelocity.z);
                        MainGameManager.Instance.AudioOwner.PlaySFX(ClipToPlay.se_player_jump);
                        isJumped = false;
                        // ジャンプ挙動
                        rigidbody.AddForce(moveVelocity, ForceMode.Impulse);
                    }
                    else
                    {
                        moveVelocity = new Vector3(moveVelocity.x, 0f, moveVelocity.z);
                    }
                    // 歩く走る挙動
                    rigidbody.AddForce(moveVelocity);
                });
        }
    }
}
