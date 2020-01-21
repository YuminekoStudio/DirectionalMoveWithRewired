using System.Collections;
using System.Collections.Generic;
using Rewired;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Yumineko.Directional {
    public class InputMove : MonoBehaviour {
        Player player;
        private DirectionalAnimationMove _danim;
        public DirectionalAnimationMove DAnimMover { get { return this._danim ? this._danim : this._danim = GetComponent<DirectionalAnimationMove> (); } }

        InputUtil inputUtil;

        private bool moving;
        private Vector2 inputVector;
        // Start is called before the first frame update
        void Start () {
            player = ReInput.players.GetPlayer (0);
            inputUtil = new InputUtil ();

            this.UpdateAsObservable ()
                .Subscribe (_ => {
                    UpdateVector ();
                    PlayCheck ();
                });
        }

        void UpdateVector () {
            inputVector = inputUtil.GetMoveDirection (player);
            DAnimMover.DAnimator.Direction = inputUtil.GetAnimationDirection (player);
            DAnimMover.Mover.Direction = inputVector;

            //  逆方向の同時押しなら、移動方向をアニメーション方向に補正。
            if (inputUtil.ReverceInput (player)) {
                DAnimMover.Mover.Direction = DAnimMover.DAnimator.Direction;
            }
        }

        void PlayCheck () {
            if (moving && inputVector == Vector2.zero) {
                DAnimMover.Stop ();
                moving = false;
            }
            else if (moving == false && inputVector != Vector2.zero) {
                DAnimMover.Play ();
                moving = true;
            }
        }
    }
}