using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSampleScript : MonoBehaviour
{
    public Tower tower;

    public GameObject towerImage;
    public GameObject towerBG;

    public CircleCollider2D RangeCircleCollider;

    //enemy checking
    private bool enemyExist;
    private List<GameObject> enemyList;

    //shooting part
    private bool _isShooting;
    public event EventHandler OnShoot;
    public GameObject BulletSpawnPoint;
    
    private void Awake()
    {
        _isShooting = false;
        enemyList = new List<GameObject>();
    }

    private void Start()
    {
        OnShoot += ShootProjectile;
        //RangeCircleCollider.radius = tower.range;
        Display();
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
        if (other.transform.CompareTag("Enemy"))
        {
            GameObject newEnemy = other.gameObject;
            enemyList.Add(newEnemy);
            enemyExist = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject thisEnemy = other.gameObject;
        if (enemyList.Contains(thisEnemy))
        {
            enemyList.Remove(thisEnemy);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
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
        yield return new WaitForSeconds(tower.reloadTime);
        
        OnShoot?.Invoke(this, EventArgs.Empty);
        _isShooting = false;
        
        yield return null;
    }

    private void FixedUpdate()
    {
        if (enemyExist && enemyList.Count > 0)
        {
            LookAt2D(transform, enemyList[0].transform ,tower.rotationOffset, tower.rotationSpeed);
        }
    }
}
