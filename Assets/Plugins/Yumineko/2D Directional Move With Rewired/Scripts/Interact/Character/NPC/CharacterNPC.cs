using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yumineko.Directional {
    public class CharacterNPC : CharacterBase, IInteractObject {

        private OverrideSprite _overrideSprite;
        public OverrideSprite OverrideSprite { get { return _overrideSprite ?? (_overrideSprite = GetComponent<OverrideSprite> ()); } }
        public void InvokeInteract () {
            Debug.Log ("Interact! Object Name is" + name);
        }
    }
}