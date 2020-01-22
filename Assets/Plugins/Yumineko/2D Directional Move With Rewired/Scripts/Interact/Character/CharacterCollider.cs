using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Yumineko.Directional {
    [System.Serializable]
    public class CharacterCollider {
        Transform _self;
        public CharacterCollider (Transform self, float filterAngle = 50) {
            _self = self;
            CreateFilters (filterAngle);
        }

        public Dictionary<Vector2, ContactFilter2D> Filters { get; private set; }

        private Rigidbody2D _rig;
        public Rigidbody2D Rig { get { return this._rig ?? (this._rig = _self.GetComponent<Rigidbody2D> ()); } }

        private CircleCollider2D _circol2D;
        public CircleCollider2D Circle2D { get { return this._circol2D ?? (this._circol2D = _self.GetComponent<CircleCollider2D> ()); } }

        /// <summary>
        /// 4方向のContact Filterを生成
        /// </summary>
        void CreateFilters (float angle) {
            Filters = new Dictionary<Vector2, ContactFilter2D> ();
            var vector = new Vector2[] { Vector2.left, Vector2.down, Vector2.right, Vector2.up };

            for (int i = 0; i < 4; i++) {
                var filter = new ContactFilter2D ();
                filter.minNormalAngle = (i * 90) - angle;
                filter.maxNormalAngle = (i * 90) + angle;;
                filter.useNormalAngle = true;
                Filters.Add (vector[i], filter);
            }
        }

        public bool IsTouch () {
            return Rig.IsTouching (Circle2D);
        }

        /// <summary>
        /// 指定方向に触れているかどうか。4方向＋ニュートラルで正規化して計算
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool IsTouch (Vector2 direction) {
            if (direction == Vector2.zero) return false;
            Vector2 xDir = (direction.x > 0) ? Vector2.right : (direction.x < 0) ? Vector2.left : Vector2.zero;
            Vector2 yDir = (direction.y > 0) ? Vector2.up : (direction.y < 0) ? Vector2.down : Vector2.zero;

            bool xTouch = (xDir != Vector2.zero) ? Rig.IsTouching (Filters[xDir]) : false;
            bool yTouch = (yDir != Vector2.zero) ? Rig.IsTouching (Filters[yDir]) : false;
            return (xTouch || yTouch);
        }

        /// <summary>
        /// 移動可能な方向を返却。ない場合はVector2.zero
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Vector2> MovableDirections () {
            return Filters.Where (p => IsTouch (p.Key) == false).Select (p => p.Key);
        }

    }
}