using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TowerSampleScript : MonoBehaviour
{
    public Global.TileType type;
    public Tower tower;
    public GameObject originalTile;
    public GameObject towerImage;
    public GameObject towerBG;

    public CircleCollider2D RangeCircleCollider;

    public UnityEvent<float> onHPChange;
    public UnityEvent onDestroy;
    public int totalHP;
    public int _currentHP;

    public int currentHP
    {
        get
        {
            return _currentHP;
        }
        set
        {
            if (_currentHP == value) return;

            _currentHP = value;

            if (totalHP != 0)
            {
                onHPChange?.Invoke( 1.0f - (float)value/totalHP);
            } else { Debug.Log("Total HP is zero"); }
        }
    }

    //enemy checking
    private bool enemyExist;
    private List<GameObject> enemyList;

    //shooting part
    public bool _isShooting;
    
    public GameObject BulletSpawnPoint;

    public event EventHandler OnShoot;

    public ProjectilePool projectilePool;
    
    private void Awake()
    {
        enemyList = new List<GameObject>();
    }

    private void Start()
    {
        
        projectilePool = ProjectilePool.Instance;
        totalHP = tower.HP;
        currentHP = tower.HP;
        if (Global.TileType.TOWER == tower.type)
        {
            _isShooting = false;
            
            var position = transform.position;
            position = new Vector3(position.x, position.y, position.z + 2.5f);
            transform.position = position;
            
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
        GameObject bullet = ProjectilePool.Instance.SpawnFromPool("bullet", transform.position, Quaternion.identity);
        ProjectileController controller = bullet.GetComponent<ProjectileController>();
        controller.ProjectileSetup(enemyList[0],tower.projectileSpeed, tower.projectileStayTime, tower.damage );
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
            onDestroy?.Invoke();
            if (type == Global.TileType.ROOT)
            {
                Vector3 worldPos = gameObject.transform.parent.transform.position;
                int x = GetComponent<Tile>().x;
                int y = GetComponent<Tile>().y;
                int oldType = TileManager.instance.board_old[x,y];


                switch(oldType)
                {
                    case (int)Global.TileType.EMPTY:
                        
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.groundPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = TileManager.instance.checkIfBuildableAt(x, y);
                        break;
                    case (int)Global.TileType.ROOT:
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.rootPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        break;
                    case (int)Global.TileType.WATER:
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.waterPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = TileManager.instance.checkIfBuildableAt(x, y);
                        break;
                    case (int)Global.TileType.NUTRIENT:
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.nutrientPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = TileManager.instance.checkIfBuildableAt(x, y);
                        break;
                    case (int)Global.TileType.ROCK:
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.rockPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        break;
                    case (int)Global.TileType.ENEMY_NEST:
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.enemyPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        break;
                }

                GameObject parentObject = gameObject.transform.parent.gameObject;

                Destroy(gameObject);
                Destroy(parentObject);
            } 
            else if (type == Global.TileType.TOWER) 
            {
                Vector3 worldPos = gameObject.transform.position;
                int x = this.transform.Find("Square").GetComponent<Tile>().x;
                int y = this.transform.Find("Square").GetComponent<Tile>().y;
                int oldType = TileManager.instance.board_old[x,y];

                switch(oldType)
                {
                    case (int)Global.TileType.EMPTY:
                        
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.groundPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = TileManager.instance.checkIfBuildableAt(x, y);
                        break;
                    case (int)Global.TileType.ROOT:
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.rootPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        break;
                    case (int)Global.TileType.WATER:
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.waterPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = TileManager.instance.checkIfBuildableAt(x, y);
                        break;
                    case (int)Global.TileType.NUTRIENT:
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.nutrientPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().isBuildAble = TileManager.instance.checkIfBuildableAt(x, y);
                        break;
                    case (int)Global.TileType.ROCK:
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.rockPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        TileManager.instance.board_pieces[x,y].Find("Square").GetComponent<Tile>().setXY(x, y);
                        break;
                    case (int)Global.TileType.ENEMY_NEST:
                        TileManager.instance.board_pieces[x,y] = Instantiate(TileManager.instance.enemyPrefab, 
                            new Vector3(0.0f, 0.0f, 0.0f),
                            new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
                            TileManager.instance.parent.transform
                        ).transform;
                        TileManager.instance.board_pieces[x,y].position = worldPos;
                        break;
                }

                Destroy(gameObject);
            }
        }
        if(Global.TileType.TOWER != tower.type) return;
        if (enemyExist && enemyList.Count > 0) 
        {
            LookAt2D(transform, enemyList[0].transform ,tower.rotationOffset, tower.rotationSpeed);
        }


    }
}
