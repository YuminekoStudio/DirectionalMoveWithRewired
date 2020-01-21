using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Yumineko.Directional {
    public class DirectionalAnimator {
        private Animator _anim;
        /// <summary>
        /// 操作対象のAnimator。nullの場合はTargetMonoからGetComponentを試みる
        /// </summary>
        public Animator TargetAnim { get { return _anim ?? (_anim = TargetMono.GetComponent<Animator> ()); } set { _anim = value; } }

        /// <summary>
        /// アニメーション方向。Vector2.zeroの代入は弾く
        /// </summary>
        private Vector2 _direction = Vector2.right;
        public Vector2 Direction { get { return _direction; } set { if (value != Vector2.zero) _direction = value; } }

        /// <summary>
        /// アニメーションの方向数の指定。2方向と4方向。
        /// </summary>

        public DirectionType DirType { get; set; }

        public float Speed { get; set; } = 1.0f;

        /// <summary>
        /// 操作対象となるMonoBehaviour
        /// </summary>
        public MonoBehaviour TargetMono { get; set; }

        /// <summary>
        /// アニメーションしているかどうか。状態の変化はStart / Stopメソッドを使う
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// 操作対象となるMonoBehabiourを指定すると、UniRxによって自動で毎フレーム更新処理が呼ばれる
        /// </summary>
        public DirectionalAnimator (MonoBehaviour targetMono, Animator targetAnim = null) {
            TargetMono = targetMono;
            TargetAnim = targetAnim;
            TargetMono?.UpdateAsObservable ()
                .Subscribe (_ => {
                    Update ();
                });
        }

        /// <summary>
        /// 現在のステートのNormalized Timeを指定値にする。
        /// </summary>
        void SetNormalize (float value) {
            var info = TargetAnim.GetCurrentAnimatorStateInfo (0);
            TargetAnim.Play (info.fullPathHash, 0, value);
        }

        /// <summary>
        /// 移動アニメーションを頭から再生する
        /// </summary>
        public void Play () {
            SetNormalize (0.0f);
            TargetAnim.speed = Speed;
            IsPlaying = true;
        }

        /// <summary>
        /// UniRxによって毎フレーム呼び出される
        /// </summary>
        void Update () {
            if (IsPlaying) {
                if (TargetAnim.speed == 0.0f) {
                    Play ();
                }
                SetParameter ();
            }
            else {
                Stop ();
            }
        }

        /// <summary>
        /// AnimatorParameterに値を送る
        /// </summary>
        void SetParameter () {
            switch (DirType) {
                case DirectionType.Dir2:
                    if (Direction.x != 0) TargetAnim.SetFloat ("x", Direction.x);
                    TargetAnim.SetFloat ("y", 0);
                    break;
                case DirectionType.Dir4:
                    TargetAnim.SetFloat ("x", Direction.x);
                    TargetAnim.SetFloat ("y", Direction.y);
                    break;
            }
        }

        /// <summary>
        /// 移動アニメーションを両足揃いで静止する
        /// </summary>
        public void Stop () {
            SetNormalize (0.9f);
            TargetAnim.speed = 0.0f;
            IsPlaying = false;
        }
    }
}