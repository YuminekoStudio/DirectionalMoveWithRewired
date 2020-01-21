using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yumineko.Directional {
    public class StepParticle : MonoBehaviour {
        [SerializeField] ParticleSystem stepParticle;
        [SerializeField] double interval = 0.1f;
        private double count = 0;

        private CharacterBase _chara;
        private CharacterBase Character { get { return _chara ?? (_chara = GetComponent<CharacterBase> ()); } }

        // Start is called before the first frame update
        void Start () {

        }

        // Update is called once per frame
        void Update () {
            count += Time.deltaTime;
            if (IsEmitTiming ()) {
                stepParticle.transform.position = Character.Collider.Rig.position;
                stepParticle.Emit (1);
                count = 0;
            }
        }

        bool IsEmitTiming () {
            if (Character.DMove.DAnimator.Direction == Vector2.zero) return false;
            return (count >= interval);
        }
    }
}