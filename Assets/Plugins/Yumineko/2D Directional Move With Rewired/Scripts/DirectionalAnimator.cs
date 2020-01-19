using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class DirectionalAnimator {
    private Animator _anim;
    /// <summary>
    /// 操作対象のAnimator。nullの場合はTargetMonoからGetComponentを試みる
    /// </summary>
    public Animator TargetAnim { get { return _anim ?? (_anim = TargetMono.GetComponent<Animator> ()); } set { _anim = value; } }

    public Vector2 Direction { get; set; }
    public float Speed { get; set; }

    /// <summary>
    /// 操作対象となるMonoBehaviour
    /// </summary>
    public MonoBehaviour TargetMono { get; set; }

    /// <summary>
    /// 操作対象となるMonoBehabiourを指定すると、UniRxによって自動で毎フレーム更新処理が呼ばれる
    /// </summary>
    public DirectionalAnimator (MonoBehaviour targetMono, Animator targetAnim = null, float speed = 1.0f) {
        TargetMono = targetMono;
        TargetAnim = targetAnim;
        Speed = speed;
        TargetMono?.UpdateAsObservable ()
            .Subscribe (_ => {
                Update ();
            });
    }

    /// <summary>
    /// 現在のステートのNormalized Timeを指定値にする。
    /// </summary>
    void SetNormalize (float value) {
        var info = TargetAnim.GetCurrentAnimatorStateInfo (0);
        TargetAnim.Play (info.fullPathHash, 0, value);
    }

    /// <summary>
    /// 移動アニメーションを頭から再生する
    /// </summary>
    public void Play () {
        SetNormalize (0.0f);
        TargetAnim.speed = Speed;
    }

    /// <summary>
    /// UniRxによって毎フレーム呼び出される
    /// </summary>
    void Update () {
        if (Direction != Vector2.zero) {
            if (TargetAnim.speed == 0.0f) {
                Play ();
            }

            TargetAnim.SetFloat ("x", Direction.x);
            TargetAnim.SetFloat ("y", Direction.y);
        }
        else {
            Stop ();
        }
    }

    /// <summary>
    /// 移動アニメーションを両足揃いで静止する
    /// </summary>
    public void Stop () {
        SetNormalize (0.9f);
        Direction = Vector2.zero;
        TargetAnim.speed = 0.0f;
    }
}