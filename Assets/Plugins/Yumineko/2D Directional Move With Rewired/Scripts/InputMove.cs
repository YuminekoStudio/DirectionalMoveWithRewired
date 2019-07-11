using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class InputMove : MonoBehaviour {
    Player player;
    private DirectionalAnimationMove _danim;
    public DirectionalAnimationMove DAnim { get { return this._danim ? this._danim : this._danim = GetComponent<DirectionalAnimationMove> (); } }

    InputUtil inputUtil;
    // Start is called before the first frame update
    void Start () {
        player = ReInput.players.GetPlayer (0);
        inputUtil = new InputUtil ();
    }

    // Update is called once per frame
    void Update () {
        DAnim.AnimationVector = inputUtil.GetFixedDirection (player);

        Vector2 buttonDirection = inputUtil.GetMoveButton (player);
        //  ボタン入力がある場合はスティックを無視する
        DAnim.MoveVector = (buttonDirection != Vector2.zero) ? buttonDirection : inputUtil.GetMoveAxisRaw (player);

        //  逆方向の同時押しなら、移動方向をアニメーション方向に補正。
        if (inputUtil.ReverceInput (player)) {
            DAnim.MoveVector = DAnim.AnimationVector;
        }
    }
}