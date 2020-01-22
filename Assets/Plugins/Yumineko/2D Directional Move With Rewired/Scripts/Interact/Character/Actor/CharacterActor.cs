using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Dbg = UnityEngine.Debug;

namespace Yumineko.Directional {
    public class CharacterActor : CharacterBase, IInteractUser {

        /// <summary>
        /// Sprite方向へrangeの長さのRayを飛ばし、IInteractObjectにあたったらInvokeInteractを呼び出す
        /// </summary>
        public bool Interact (float range) {
            //  UIをクリックしていたら失敗を返す
            if (EventSystem.current.IsPointerOverGameObject ()) return false;
            var rayRange = (range > 0) ? range : Mathf.Infinity;

            //レイヤーマスク作成(8番を除外)
            int layerMask = ~(1 << 8);
            RaycastHit2D hit = Physics2D.Raycast (RayOrigin, DMove.DAnimator.Direction, range + Collider.Circle2D.radius, layerMask);
#if UNITY_EDITOR
            Dbg.DrawRay (RayOrigin, DMove.DAnimator.Direction * range, Color.blue, 1, false);
#endif

            hit.collider?.GetComponent<IInteractObject> ()?.InvokeInteract ();

            if (hit.collider != null) Dbg.Log (name + "が" + hit.collider?.name + "をInteract");
            return (hit.collider != null);
        }

        /// <summary>
        /// Rayの開始地点
        /// </summary>
        protected virtual Vector2 RayOrigin { get { return Collider.Rig.position + Collider.Circle2D.offset; } }
    }
}