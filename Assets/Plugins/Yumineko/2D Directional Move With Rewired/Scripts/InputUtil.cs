using Rewired;
using UnityEngine;

/// <summary>
/// 入力に関するユーティリティクラス
/// </summary>
public class InputUtil {
    public const string HButton = "Button Move Horizontal";
    public const string VButton = "Button Move Vertical";
    public const string HAxis = "Move Horizontal";
    public const string VAxis = "Move Vertical";
    private Vector2 fixedDirection;

    /// <summary>
    /// スティック入力の移動方向を0か1に補正した値で返却。
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public Vector2 GetMoveAxisRaw (Player player) {
        return player.GetAxis2DRaw (HAxis, VAxis);
    }

    /// <summary>
    /// 移動用のアニメーションベクトルを返却
    /// </summary>
    /// <param name="player">入力対象</param>
    /// <returns>4方向またはVector2.zero</returns>
    public Vector2 GetAnimationDirection (Player player) {
        //  ボタン入力がない場合、以下を無視してスティック入力値を返す
        Vector2 buttonVector = GetButtonDirection (player);
        if (buttonVector == Vector2.zero) return GetMoveAxisRaw (player);

        //  押下直後の方向を向く。押下直後でないなら何もしない
        var dir = GetButtonDownDirection (player);
        if (dir != Vector2.zero) fixedDirection = dir;

        //  左右の同時押しされたら、上下の入力のみを使用する
        if (player.GetButton (HButton) && player.GetNegativeButton (HButton)) {
            if (player.GetButton (VButton)) fixedDirection = Vector2.up;
            else if (player.GetNegativeButton (VButton)) fixedDirection = Vector2.down;
        }
        //  上下の同時押しされたら、左右の入力のみを使用する
        if (player.GetButton (VButton) && player.GetNegativeButton (VButton)) {
            if (player.GetButton (HButton)) fixedDirection = Vector2.right;
            else if (player.GetNegativeButton (HButton)) fixedDirection = Vector2.left;
        }
        //  ボタンが離れた時に向きを更新
        if (player.GetButtonUp (HButton) || player.GetButtonUp (VButton) ||
            player.GetNegativeButtonUp (HButton) || player.GetNegativeButtonUp (VButton)) {
            fixedDirection = GetButtonDirection (player);
        }

        return fixedDirection;
    }

    /// <summary>
    /// 移動用のベクトルを返却
    /// </summary>
    public Vector2 GetMoveDirection (Player player) {
        Vector2 result = GetButtonDirection (player);
        if (result == Vector2.zero) result = GetMoveAxisRaw (player);
        return result;
    }

    /// <summary>
    /// 押され続けているボタンも含めた判定で向きを返却
    /// </summary>
    /// <returns></returns>
    public Vector2 GetButtonDirection (Player player) {
        float x = player.GetButton (HButton) ? 1.0f : player.GetNegativeButton (HButton) ? -1.0f : 0.0f;
        float y = player.GetButton (VButton) ? 1.0f : player.GetNegativeButton (VButton) ? -1.0f : 0.0f;
        return new Vector2 (x, y);
    }

    /// <summary>
    /// 押下直後の方向を返す。なにも押されていないなら0を返す
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    Vector2 GetButtonDownDirection (Player player) {
        if (player.GetButtonDown (HButton) && !player.GetNegativeButton (HButton)) return Vector2.right;
        if (player.GetButtonDown (VButton) && !player.GetNegativeButton (VButton)) return Vector2.up;
        if (player.GetNegativeButtonDown (HButton) && !player.GetButton (HButton)) return Vector2.left;
        if (player.GetNegativeButtonDown (VButton) && !player.GetButton (VButton)) return Vector2.down;
        return Vector2.zero;
    }

    /// <summary>
    /// 上下、または左右を同時押ししているか
    /// </summary>
    /// <returns>同時押ししていればtrue</returns>
    public bool ReverceInput (Player player) {
        return ((player.GetButton (HButton) && player.GetNegativeButton (HButton)) ||
            (player.GetButton (VButton) && player.GetNegativeButton (VButton)));
    }
}