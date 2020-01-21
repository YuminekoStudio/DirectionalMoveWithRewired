using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Dbg = UnityEngine.Debug;

namespace Yumineko.Directional {
    public class CharacterActor : CharacterBase {
        private float rayRange;
        public void Interact (float range) {
            if (EventSystem.current.IsPointerOverGameObject ()) return;
            rayRange = (range > 0) ? range : Mathf.Infinity;

            //レイヤーマスク作成
            int layerMask = ~(1 << 8);

            RaycastHit2D hit = Physics2D.Raycast (RayOrigin, DMove.DAnimator.Direction, range + Collider.Circle2D.radius, layerMask);
            hit.collider?.GetComponent<IInteractObject> ()?.InvokeInteract ();
            Dbg.DrawRay (RayOrigin, DMove.DAnimator.Direction * range, Color.blue, 1, false);
            Dbg.Log (DMove.DAnimator.Direction);
            if (hit.collider != null) Dbg.Log ("Actorが" + hit.collider?.name + "をinteract");
        }

        /// <summary>
        /// Rayの開始地点
        /// </summary>
        /// <value></value>
        private Vector2 RayOrigin { get { return Collider.Rig.position + Collider.Circle2D.offset; } }

        void Update () {
            if (Input.GetMouseButtonDown (0)) {
                Interact (20);
            }
        }
    }
}