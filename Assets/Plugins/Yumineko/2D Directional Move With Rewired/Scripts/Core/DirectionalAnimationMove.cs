using UniRx;
using UniRx.Async;
using UnityEngine;

namespace Yumineko.Directional {
    public class DirectionalAnimationMove {
        private DirectionalAnimator _danim;
        public DirectionalAnimator DAnimator { get { return _danim ?? (_danim = new DirectionalAnimator (TargetCharacter, TargetAnimator)); } }

        private GameObjectMover _mover;
        public GameObjectMover Mover { get { return _mover ?? (_mover = new GameObjectMover (TargetCharacter, TargetRigid2D)); } }

        private Animator _anim;
        public Animator TargetAnimator { get { return _anim ?? (_anim = TargetCharacter.GetComponent<Animator> ()); } }

        private Rigidbody2D _rig;
        public Rigidbody2D TargetRigid2D { get { return _rig ?? (_rig = TargetCharacter.GetComponent<Rigidbody2D> ()); } }

        public CharacterBase TargetCharacter { get; set; }

        public DirectionalAnimationMove (CharacterBase target) {
            TargetCharacter = target;
            this.ObserveEveryValueChanged (_ => DirType)
                .Subscribe (d => {
                    DAnimator.DirType = d;
                });
        }

        public DirectionType DirType { get; set; }

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