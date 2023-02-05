using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    public Enemy enemy;
    public GameObject target;
    private NavMeshAgent agent;
    private bool stop;
    public bool showPath;
    public bool showAhead;

    public CircleCollider2D Collider2D;
    public float detectionRange;

    public float currentHP;

    public GameObject parentObject;

    private Rigidbody2D _rigidbody2D;
    private bool isAttacking = false;
    private bool isIncreasing = false;
    
    public List<GameObject> tileList;
    public bool tileExist;
    public Vector3 scale;

    public float interval = 0.0f;
    public float MaxLifeCycle = 60f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IncreaseRange());
        EnableThisEnemy();
    }

    private void EnableThisEnemy()
    {
        Collider2D.radius = enemy.detectionRange;
        currentHP = enemy.HP * Global.instance.AntHPMultiplyer;
        tileList = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        agent.speed = enemy.moveSpeed;
        agent.acceleration = enemy.acceleration;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        detectionRange = enemy.detectionRange;
        scale = gameObject.transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        interval += Time.deltaTime;
        if (interval > MaxLifeCycle)
        {
            Global.instance.currentEnemy -= 1;
            Destroy(gameObject);
            
        }
        if (currentHP < 0)
        {
            Global.instance.currentEnemy -= 1;
            Destroy(gameObject);
        }

        if (tileExist && tileList.Count > 0)
        {
            agent.SetDestination(tileList[0].transform.position);
        }

        Vector3 angle = gameObject.transform.eulerAngles;
        
        float angleZ = angle.z;
        
        if (Mathf.Abs(angleZ) < 90f)
        {
            gameObject.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        }
        else if (Mathf.Abs(angleZ) > 90f)
        {
            //gameObject.transform.eulerAngles = new Vector3(angle.x, angle.y , )
            gameObject.transform.localScale = new Vector3(-scale.x, -scale.y, scale.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            GameObject newEnemy = other.gameObject;
            tileList.Add(newEnemy);
            tileExist = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject thisEnemy = other.gameObject;
            if (tileList.Contains(thisEnemy))
            {
                tileList.Remove(thisEnemy);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackPlayer(other.gameObject));
            
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isAttacking = false;
    }

    IEnumerator AttackPlayer(GameObject target)
    {
        while (target)
        {

            yield return new WaitForSeconds(enemy.attackDuration);
            if (target)
            {
                target.transform.GetComponent<TowerSampleScript>().currentHP -= enemy.attackDamage;
            }

        }
        isAttacking = false;
    }

    IEnumerator IncreaseRange()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            detectionRange += 1;
            Collider2D.radius = detectionRange;
        }
        
    }
}
