using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class InputMove : MonoBehaviour {
    [SerializeField]
    float speed = 1.0f;
    Vector2 inputAxis;
    Player player;
    private Rigidbody2D _rig;
    public Rigidbody2D Rigidbody2D { get { return this._rig ? this._rig : this._rig = GetComponent<Rigidbody2D> (); } }

    private DirectionalAnimation _danim;
    public DirectionalAnimation DAnim { get { return this._danim ? this._danim : this._danim = GetComponent<DirectionalAnimation> (); } }

    // Start is called before the first frame update
    void Start () {
        player = ReInput.players.GetPlayer (0);
    }

    // Update is called once per frame
    void Update () {
        inputAxis = player.GetAxis2DRaw ("Move Horizontal", "Move Vertical");

        if ((player.GetButton ("Button Move Horizontal") && player.GetNegativeButton ("Button Move Horizontal")) ||
            (player.GetButton ("Button Move Vertical") && player.GetNegativeButton ("Button Move Vertical"))) inputAxis = DAnim.Direction;
    }
    void FixedUpdate () {
        Rigidbody2D.MovePosition (Rigidbody2D.position + inputAxis * speed);
    }
}