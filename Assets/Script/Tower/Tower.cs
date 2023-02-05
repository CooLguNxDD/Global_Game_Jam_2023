using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tower", menuName = "Tower")]
public class Tower : ScriptableObject
{
    // tower idenitifier
    public int TowerID;
    
    public Global.TileType type;
    
    public float rotationSpeed;
    public float rotationOffset;

    public int damage;
    
    public float reloadTime;
    public float range;

    public int HP;

    public GameObject projectile;
    public float projectileSpeed;
    public float projectileStayTime;

    public Sprite TowerImage;
    public Sprite TowerBG;
}

