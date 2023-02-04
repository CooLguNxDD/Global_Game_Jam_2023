using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSampleScript : MonoBehaviour
{
    //rotation setting
    public float rotationSpeed;
    public float rotationOffset;
    
    //enemy checking
    private bool enemyExist;
    
    //todo: convert to scriptable object 
    private List<GameObject> enemyList;

    //shooting part

    public GameObject projectile;
    public GameObject BulletSpawnPoint;
    
    
    public int reloadTime;
    
    private bool _isShooting;
    

    private void Awake()
    {
        _isShooting = false;
        enemyList = new List<GameObject>();
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
    IEnumerator Shoot()
    {
        _isShooting = true;
        yield return new WaitForSeconds(reloadTime);
        GameObject bullet = Instantiate(projectile, BulletSpawnPoint.transform.position, Quaternion.identity);
        bullet.GetComponent<projectileController>().target = enemyList[0];
        _isShooting = false;
        yield return null;
    }

    private void FixedUpdate()
    {
        if (enemyExist && enemyList[0])
        {
            LookAt2D(transform, enemyList[0].transform ,rotationOffset, rotationSpeed);
        }
    }
}
