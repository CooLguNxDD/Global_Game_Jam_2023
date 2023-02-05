using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public float stayTime;
    public float damage;
    public GameObject target;
    public float interval;
    
    private Vector3 shootDir;

    public bool isTarget;
    
    //code monkey!
    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public void ProjectileSetup(GameObject target_obj, float setSpeed, float setStayTime, float setDamage)
    {
        target = target_obj;
        speed = setSpeed;
        stayTime = setStayTime;
        damage = setDamage;
        interval = 0.0f;
        isTarget = true;
    }

    private void Start()
    {
        Debug.Log(target.name);
    }

    public void SeekEnemy()
    {
        if (target)
        {
            shootDir = (target.transform.position - transform.position).normalized;
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
            transform.position += shootDir * (speed * Time.deltaTime);
        }
    }
    void Update()
    {
        if (isTarget)
        {
            SeekEnemy();
        }

        if (!target)
        {
            gameObject.SetActive(false);
        }

        interval += Time.deltaTime;
        if (stayTime < interval)
        {
            interval = 0.0f;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(other.transform.GetComponent<Chase>().currentHP);
            other.transform.GetComponent<Chase>().currentHP -= (int)damage;
            isTarget = false;
            gameObject.SetActive(false);
        }
    }
}
