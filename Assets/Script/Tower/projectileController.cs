using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class projectileController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public float stayTime;
    public int damage;

    public Vector3 target;
    
    private Vector3 shootDir;
    
    //code monkey!
    public static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    private void Start()
    {
        Destroy(gameObject, stayTime);
    }

    public void SeekEnemy()
    {
        shootDir = (target - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
        transform.position += shootDir * (speed * Time.deltaTime);
        
    }

    void Update()
    {
        SeekEnemy();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.transform.GetComponent<Chase>().currentHP -= damage;
            Destroy(gameObject);
        }
    }
}
