using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public int EnemyID;
    public Global.TileType type;
    public Sprite enemyImg;

    public float moveSpeed;
    public float acceleration;
    
    public int HP;

    public int attackDamage = 5;
    public float attackDuration = 0.5f;
    
    public float detectionRange;
    
}
