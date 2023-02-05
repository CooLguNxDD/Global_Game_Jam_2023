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

    public int currentHP;

    public GameObject parentObject;

    private Rigidbody2D _rigidbody2D;
    private bool isAttacking = false;
    private bool isIncreasing = false;
    
    public List<GameObject> tileList;
    public bool tileExist;
    public Vector3 scale;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = enemy.HP;
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
        Collider2D.radius = detectionRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHP < 0)
        {
            Destroy(parentObject);
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

        if (!isIncreasing)
        {
            StartCoroutine(IncreaseRange());
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enter");
            GameObject newEnemy = other.gameObject;
            tileList.Add(newEnemy);
            tileExist = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit");
            GameObject thisEnemy = other.gameObject;
            if (tileList.Contains(thisEnemy))
            {
                tileList.Remove(thisEnemy);
            }
        }


    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (tileExist && collision.transform.CompareTag("Player"))
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isAttacking = false;
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;
        yield return new WaitForSeconds(enemy.attackDuration);
        if (isAttacking)
        {
            tileList[0].transform.GetComponent<TowerSampleScript>().currentHP -= enemy.attackDamage;
        }
        isAttacking = false;
    }

    IEnumerator IncreaseRange()
    {
        isIncreasing = true;
        yield return new WaitForSeconds(20f);
        isIncreasing = false;
        detectionRange += 1;
        Collider2D.radius = detectionRange;
    }
}
