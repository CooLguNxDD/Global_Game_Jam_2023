using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSampleScript : MonoBehaviour
{
    public Tower tower;
    public GameObject originalTile;
    public GameObject towerImage;
    public GameObject towerBG;

    public CircleCollider2D RangeCircleCollider;
    public int currentHP;

    //enemy checking
    private bool enemyExist;
    private List<GameObject> enemyList;

    //shooting part
    public bool _isShooting;
    
    public GameObject BulletSpawnPoint;

    public event EventHandler OnShoot;
    
    private void Awake()
    {
        enemyList = new List<GameObject>();
    }

    private void Start()
    {
        currentHP = tower.HP;
        if (Global.TileType.TOWER == tower.type)
        {
            _isShooting = false;
            
            OnShoot += ShootProjectile;
            //RangeCircleCollider.radius = tower.range;
            Display();
        }
    }

    public void Display()
    {
        towerImage.GetComponent<SpriteRenderer>().sprite = tower.TowerImage;
        towerBG.GetComponent<SpriteRenderer>().sprite = tower.TowerBG;
    }

    public void ShootProjectile(object sender, EventArgs e)
    {
        GameObject bullet = Instantiate(tower.projectile, BulletSpawnPoint.transform.position, Quaternion.identity);

        projectileController controller = bullet.GetComponent<projectileController>();
        controller.target = enemyList[0].transform.position;
        controller.speed = tower.projectileSpeed;
        controller.stayTime = tower.projectileStayTime;
        controller.damage = tower.damage;
    }

    private void LookAt2D(Transform current, Transform others ,float RotationOffset, float RotationSpeed)
    {
        Vector3 vectorToTarget = others.position - current.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - RotationOffset;
        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        current.rotation = Quaternion.Slerp(current.rotation, quaternion, Time.deltaTime * RotationSpeed);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(Global.TileType.TOWER != tower.type) return;
        
        if (other.transform.CompareTag("Enemy"))
        {
            GameObject newEnemy = other.gameObject;
            enemyList.Add(newEnemy);
            enemyExist = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(Global.TileType.TOWER != tower.type) return;
        
        GameObject thisEnemy = other.gameObject;
        if (enemyList.Contains(thisEnemy))
        {
            enemyList.Remove(thisEnemy);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(Global.TileType.TOWER != tower.type) return;
        
        if (other.transform.CompareTag("Enemy") && enemyExist)
        {

            if (!_isShooting)
            {
                StartCoroutine(Shoot());
            }
        }
        else
        {
            enemyExist = false;
        }
    }
    //subscribe to shoot event
    IEnumerator Shoot()
    {
        _isShooting = true;
        OnShoot(this, EventArgs.Empty);
        yield return new WaitForSeconds(tower.reloadTime);
        _isShooting = false;
    }

    private void FixedUpdate()
    {
        if (currentHP < 0)
        {
            Destroy(gameObject);
        }
        if(Global.TileType.TOWER != tower.type) return;
        if (enemyExist && enemyList.Count > 0) 
        {
            LookAt2D(transform, enemyList[0].transform ,tower.rotationOffset, tower.rotationSpeed);
        }


    }
}
