using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyGridController : MonoBehaviour
{
    public int enemiesPerRow = 10;
    public float stride;
    public List<GameObject> enemyRowTypes;
    public float firstShootDelay = 5.0f;
    public float shootTimeStride = 0.5f;
    
    private GameManager _gameMgr;
    
    private Vector3 _leftTopEdgePosOffset;
    
    private List<List<GameObject>> _enemies;

    private int _initialEnemiesCount;
    private int _aliveEnemiesCount;

    private float _nextShootTime;

    // Start is called before the first frame update
    void Start()
    {
        _gameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        _enemies = new List<List<GameObject>>();
        
        _initialEnemiesCount = _aliveEnemiesCount = enemiesPerRow * enemyRowTypes.Count;

        _leftTopEdgePosOffset = new Vector3(-enemiesPerRow, enemyRowTypes.Count) / 2 * stride;
        Vector3 leftTopEdgePos = transform.position + _leftTopEdgePosOffset;
                                 
        for (int r = 0; r < enemyRowTypes.Count; r++)
        {
            _enemies.Add(new List<GameObject>());
            for (int c = 0; c < enemiesPerRow; c++)
            {
                Vector3 position = leftTopEdgePos + new Vector3(c, -r) * stride;

                GameObject enemy = Instantiate(enemyRowTypes[r], position, Quaternion.identity);
                enemy.transform.parent = transform;

                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                enemyController.row = r;
                enemyController.col = c;
                
                _enemies[r].Add(enemy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameMgr.CurrGameState == GameManager.GameState.PlayerAlive &&
            Time.time > _nextShootTime)
        {
            while (!Shoot()) {};
            _nextShootTime = Time.time + shootTimeStride;
        }
    }

    private bool Shoot()
    {
        int shootCol = Random.Range(0, enemiesPerRow - 1);
        int shootRow = enemyRowTypes.Count;
        for (int i = shootRow - 1; i >= 0; i--)
        {
            if (i == 0 && _enemies[i][shootCol] is null)
            {
                return false;
            }
            
            if (_enemies[i][shootCol] is null && _enemies[i - 1][shootCol] is not null)
            {
                shootRow = i;
                break;
            }
        }
        
        Vector3 leftTopEdgePos = transform.position + _leftTopEdgePosOffset;
        Vector3 projPosition = leftTopEdgePos + new Vector3(shootCol, -shootRow) * stride;

        GameObject projType = enemyRowTypes[shootRow - 1].GetComponent<EnemyController>().projectileType;
        Instantiate(projType, projPosition, Quaternion.identity);
        
        return true;
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        _aliveEnemiesCount--;
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        _enemies[enemyController.row][enemyController.col] = null;
        
        _gameMgr.OnEnemyDeath();
        if (_aliveEnemiesCount <= 0)
        {
            _gameMgr.OnAllEnemiesDeath();
        }
    }
}
