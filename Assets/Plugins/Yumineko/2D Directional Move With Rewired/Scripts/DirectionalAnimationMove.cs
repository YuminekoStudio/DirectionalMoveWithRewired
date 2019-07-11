using System.Threading.Tasks;
using Rewired;
using UnityEngine;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (Rigidbody2D))]
public class DirectionalAnimationMove : MonoBehaviour {
    private Animator _anim;
    private Animator Anim { get { return this._anim ? this._anim : this._anim = GetComponent<Animator> (); } }

    private Rigidbody2D _rig;
    private Rigidbody2D Rig { get { return this._rig ? this._rig : this._rig = GetComponent<Rigidbody2D> (); } }

    Player player;

    /// <summary>
    /// アニメーション方向。設定されている限り動き続ける。
    /// </summary>
    public Vector2 AnimationVector { get; set; }
    /// <summary>
    /// 移動方向
    /// </summary>
    public Vector2 MoveVector { get; set; }

    [SerializeField]
    private float _moveSpeed;
    public float MoveSpeed {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    [SerializeField]
    private float _animationSpeed;
    public float AnimationSpeed {
        get { return _animationSpeed; }
        set { _animationSpeed = value; }
    }

    // Start is called before the first frame update
    void Start () {
        player = ReInput.players.GetPlayer (0);
    }

    // Update is called once per frame
    void Update () {
        if (AnimationVector != Vector2.zero) {
            if (Anim.speed == 0.0f) {
                Play ();
            }

            Anim.SetFloat ("x", AnimationVector.x);
            Anim.SetFloat ("y", AnimationVector.y);
        } else {
            Stop ();
        }
    }

    void FixedUpdate () {
        Rig.MovePosition (Rig.position + MoveVector * MoveSpeed);
    }

    void Stop () {
        SetNormalize (0.9f);
        AnimationVector = Vector2.zero;
        MoveVector = Vector2.zero;
        Anim.speed = 0.0f;
    }

    void Play () {
        SetNormalize (0.0f);
        Anim.speed = AnimationSpeed;
    }

    public async Task Walk (Vector2 dir, float sec) {
        AnimationVector = dir;
        MoveVector = dir;
        Play ();
        await Task.Delay ((int) (sec * 1000f));
        Stop ();
    }

    /// <summary>
    /// 現在のステートのNormalized Timeを指定値にする。
    /// </summary>
    void SetNormalize (float value) {
        var info = Anim.GetCurrentAnimatorStateInfo (0);
        Anim.Play (info.fullPathHash, 0, value);
    }
}