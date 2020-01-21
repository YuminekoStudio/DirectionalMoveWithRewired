using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Yumineko.Directional {
    public class GameObjectMover {
        private Rigidbody2D _rig;
        /// <summary>
        /// 操作対象のRigidbody2D。nullの場合はTargetMonoからGetComponentを試みる
        /// </summary>
        public Rigidbody2D TargetRig { get { return _rig ?? (_rig = TargetMono.GetComponent<Rigidbody2D> ()); } set { _rig = value; } }

        /// <summary>
        /// 操作対象となるMonoBehaviour
        /// </summary>
        public MonoBehaviour TargetMono { get; set; }

        public Vector2 Direction { get; set; }
        public float Speed { get; set; }

        /// <summary>
        /// 移動しているかどうか。状態の変化はStart / Stopメソッドを使う
        /// </summary>
        public bool IsMoving { get; private set; }

        /// <summary>
        /// 操作対象となるMonoBehabiourを指定すると、UniRxによって自動で毎フレーム更新処理が呼ばれる
        /// </summary>
        public GameObjectMover (MonoBehaviour targetMono, Rigidbody2D targetRig = null, float speed = 1.0f) {
            TargetMono = targetMono;
            TargetRig = targetRig;
            Speed = speed;
            TargetMono?.FixedUpdateAsObservable ()
                .Subscribe (_ => {
                    Update ();
                });
        }

        /// <summary>
        /// UniRxによって毎フレーム呼び出される
        /// </summary>
        void Update () {
            if (IsMoving) {
                TargetRig.MovePosition (TargetRig.position + Direction * Speed);
            }
        }

        /// <summary>
        /// 移動を開始する
        /// </summary>
        public void Play () {
            IsMoving = true;
        }

        /// <summary>
        /// 移動を停止する
        /// </summary>
        public void Stop () {
            IsMoving = false;
        }
    }
}