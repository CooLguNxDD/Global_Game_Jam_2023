using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class projectileController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 20f;
    public float stayTime = 2f;

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
        shootDir = (target - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += shootDir * (speed * Time.deltaTime);
    }
}
