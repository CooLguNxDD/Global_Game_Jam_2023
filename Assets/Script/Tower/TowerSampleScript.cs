using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSampleScript : MonoBehaviour
{
    //rotation setting
    public float rotationSpeed;
    public float rotationOffset;
    
    //enemy checking
    private bool enemyExist;
    private Transform enemy;

    //shooting part

    public GameObject projectile;
    public GameObject BulletSpawnPoint;
    
    
    public int reloadTime;
    
    private bool _isShooting;

    private void Awake()
    {
        _isShooting = false;
    }

    private void LookAt2D(Transform current, Transform others)
    {
        Vector3 vectorToTarget = others.transform.position - current.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationOffset;
        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        current.rotation = Quaternion.Slerp(current.rotation, quaternion, Time.deltaTime * rotationSpeed);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Enemy") && !enemyExist)
        {
            Debug.Log("entered");
            enemy = other.transform;
            enemyExist = true;
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
        GameObject bullet = Instantiate(projectile, BulletSpawnPoint.transform, transform);
        LookAt2D(bullet.transform, enemy);
        yield return new WaitForSeconds(reloadTime);
        _isShooting = false;
        yield return null;
    }

    private void FixedUpdate()
    {
        if (enemyExist)
        {
            LookAt2D(transform, enemy.transform);
        }
    }


}
