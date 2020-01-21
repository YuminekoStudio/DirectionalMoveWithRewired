using System.Collections;
using System.Collections.Generic;
using Rewired;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Yumineko.Directional {
    public class InputMove : MonoBehaviour {
        Player player;
        private CharacterBase _character;
        public CharacterBase Character { get { return _character ?? (_character = GetComponent<CharacterBase> ()); } }

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
            Character.DMove.DAnimator.Direction = inputUtil.GetAnimationDirection (player);
            Character.DMove.Mover.Direction = inputVector;

            //  逆方向の同時押しなら、移動方向をアニメーション方向に補正。
            if (inputUtil.ReverceInput (player)) {
                Character.DMove.Mover.Direction = Character.DMove.DAnimator.Direction;
            }
        }

        void PlayCheck () {
            if (moving && inputVector == Vector2.zero) {
                Character.DMove.Stop ();
                moving = false;
            }
            else if (moving == false && inputVector != Vector2.zero) {
                Character.DMove.Play ();
                moving = true;
            }
        }
    }
}