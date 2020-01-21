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
        /// アニメーション方向
        /// </summary>
        /// <value></value>
        public Vector2 Direction { get; set; }

        /// <summary>
        /// アニメーションの方向数の指定。2方向と4方向。
        /// </summary>
        /// <value></value>
        public DirectionType DirType { get; set; }

        public float Speed { get; set; }

        /// <summary>
        /// 操作対象となるMonoBehaviour
        /// </summary>
        public MonoBehaviour TargetMono { get; set; }

        /// <summary>
        /// 操作対象となるMonoBehabiourを指定すると、UniRxによって自動で毎フレーム更新処理が呼ばれる
        /// </summary>
        public DirectionalAnimator (MonoBehaviour targetMono, Animator targetAnim = null, float speed = 1.0f) {
            TargetMono = targetMono;
            TargetAnim = targetAnim;
            Speed = speed;
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
        }

        /// <summary>
        /// UniRxによって毎フレーム呼び出される
        /// </summary>
        void Update () {
            if (Direction != Vector2.zero) {
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
            Direction = Vector2.zero;
            TargetAnim.speed = 0.0f;
        }
    }
}