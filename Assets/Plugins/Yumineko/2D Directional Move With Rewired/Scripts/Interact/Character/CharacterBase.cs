using UniRx;
using UnityEngine;
namespace Yumineko.Directional {
    [RequireComponent (typeof (Rigidbody2D))]
    [RequireComponent (typeof (Animator))]
    [RequireComponent (typeof (SpriteRenderer))]
    public abstract class CharacterBase : MonoBehaviour {
        private CharacterCollider _collider;
        public CharacterCollider Collider { get { return _collider ?? (_collider = new CharacterCollider (transform, contactFilterAngle)); } }

        private DirectionalAnimationMove _dMove;
        public DirectionalAnimationMove DMove { get { return _dMove ?? (_dMove = new DirectionalAnimationMove (this)); } }

        [Header ("移動速度")] public float MoveSpeed;
        [Header ("アニメーション速度")] public float AnimationSpeed;
        [Header ("移動方向")] public DirectionType DirType = DirectionType.Dir4;

        void Start () {
            this.ObserveEveryValueChanged (_ => MoveSpeed)
                .Subscribe (m => {
                    DMove.Mover.Speed = m;
                });
            this.ObserveEveryValueChanged (_ => AnimationSpeed)
                .Subscribe (a => {
                    DMove.DAnimator.Speed = a;
                });
            this.ObserveEveryValueChanged (_ => DirType)
                .Subscribe (d => {
                    DMove.DirType = d;
                });
        }

        [SerializeField, Header ("当たり判定の角度")] float contactFilterAngle = 50;
    }
}