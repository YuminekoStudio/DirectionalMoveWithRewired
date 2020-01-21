using UnityEngine;
[System.Serializable]
public abstract class Character {
    public CharacterCollider Collider { get; set; }

    [Header ("移動スピード")] public float MoveSpeed;

    [SerializeField, Header ("当たり判定の角度")] float contactFilterAngle = 50;
}