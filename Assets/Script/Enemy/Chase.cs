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

    public int currentHP;

    public GameObject parentObject;

    private Rigidbody2D _rigidbody2D;
    private bool isAttacking = false;
    
    public List<GameObject> tileList;
    public bool tileExist;
    

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
        gameObject.transform.eulerAngles = Vector3.zero;
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
}
