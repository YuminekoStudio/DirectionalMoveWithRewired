using UniRx;
using UniRx.Async;
using UnityEngine;

namespace Yumineko.Directional {
    [RequireComponent (typeof (Rigidbody2D))]
    [RequireComponent (typeof (Animator))]
    [RequireComponent (typeof (SpriteRenderer))]
    public class DirectionalAnimationMove : MonoBehaviour {
        private DirectionalAnimator _danim;
        public DirectionalAnimator DAnimator { get { return _danim ?? (_danim = new DirectionalAnimator (this, MyAnimator, animationSpeed)); } }

        private GameObjectMover _mover;
        public GameObjectMover Mover { get { return _mover ?? (_mover = new GameObjectMover (this, MyRigid2D, moveSpeed)); } }

        private Animator _anim;
        public Animator MyAnimator { get { return _anim ?? (_anim = GetComponent<Animator> ()); } }

        private Rigidbody2D _rig;
        public Rigidbody2D MyRigid2D { get { return _rig ?? (_rig = GetComponent<Rigidbody2D> ()); } }

        [SerializeField, Header ("移動速度")] private float moveSpeed = 2.0f;
        [SerializeField, Header ("アニメ速度")] private float animationSpeed = 1.0f;

        [SerializeField]
        DirectionType DirType = DirectionType.Dir4;

        void Start () {
            this.ObserveEveryValueChanged (_ => DirType)
                .Subscribe (d => DAnimator.DirType = d);
        }

        /// <summary>
        /// アニメーション移動を開始する。一回呼べば自動で毎フレーム呼ばれる
        /// </summary>
        public void Play () {
            DAnimator.Play ();
            Mover.Play ();
        }

        public void Stop () {
            DAnimator.Stop ();
            Mover.Stop ();
        }

        /// <summary>
        /// 指定秒、指定方向へ歩く。ミリ秒で指定（1秒なら1000）
        /// </summary>
        /// <returns></returns>
        public async UniTask Walk (Vector2 dir, int milliseconds) {
            DAnimator.Direction = dir;
            Mover.Direction = dir;
            Play ();
            await UniTask.Delay (milliseconds);
            Stop ();
        }
    }
}