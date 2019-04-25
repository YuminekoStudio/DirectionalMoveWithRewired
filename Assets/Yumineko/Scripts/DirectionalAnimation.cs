using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class DirectionalAnimation : MonoBehaviour {
    private Animator _anim;
    public Animator Anim { get { return this._anim ? this._anim : this._anim = GetComponent<Animator> (); } }

    Player player;
    public Vector2 Direction { get; private set; }

    public string HKey { get { return "Button Move Horizontal"; } }
    public string VKey { get { return "Button Move Vertical"; } }

    // Start is called before the first frame update
    void Start () {
        player = ReInput.players.GetPlayer (0);
        Direction = Vector2.zero;
    }

    // Update is called once per frame
    void Update () {
        Vector2 inputAxis = player.GetAxis2DRaw ("Move Horizontal", "Move Vertical");
        if (inputAxis != Vector2.zero && InputMoveButton () == false) Direction = inputAxis;

        //  押されたほうを向く
        if (player.GetButtonDown (HKey) && !player.GetNegativeButton (HKey)) Direction = Vector2.right;
        if (player.GetButtonDown (VKey) && !player.GetNegativeButton (VKey)) Direction = Vector2.up;
        if (player.GetNegativeButtonDown (HKey) && !player.GetButton (HKey)) Direction = Vector2.left;
        if (player.GetNegativeButtonDown (VKey) && !player.GetButton (VKey)) Direction = Vector2.down;

        //  左右の同時押し対策
        if (player.GetButton (HKey) && player.GetNegativeButton (HKey)) {
            if (player.GetButton (VKey)) Direction = Vector2.up;
            else if (player.GetNegativeButton (VKey)) Direction = Vector2.down;
        }
        //  上下の同時押し対策
        if (player.GetButton (VKey) && player.GetNegativeButton (VKey)) {
            if (player.GetButton (HKey)) Direction = Vector2.right;
            else if (player.GetNegativeButton (HKey)) Direction = Vector2.left;
        }
        //  ボタンが離れた時に向きを更新
        if (player.GetButtonUp (HKey) || player.GetButtonUp (VKey) ||
            player.GetNegativeButtonUp (HKey) || player.GetNegativeButtonUp (VKey)) Direction = GetButtonDirection ();

        if (Direction != Vector2.zero) {
            Anim.speed = 1.0f;
            Anim.SetFloat ("x", Direction.x);
            Anim.SetFloat ("y", Direction.y);
        } else {
            Anim.speed = 0.0f;
        }

    }

    Vector2 GetButtonDirection () {
        float x = player.GetButton (HKey) ? 1.0f : player.GetNegativeButton (HKey) ? -1.0f : 0.0f;
        float y = player.GetButton (VKey) ? 1.0f : player.GetNegativeButton (VKey) ? -1.0f : 0.0f;
        return new Vector2 (x, y);
    }

    bool InputMoveButton () {
        return (player.GetButton (HKey) || player.GetNegativeButton (HKey) ||
            player.GetButton (VKey) || player.GetNegativeButton (VKey));
    }
}