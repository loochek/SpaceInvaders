using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject projectileType;
    
    public int row;
    public int col;
    
    private int _playerBulletLayer;
    
    private EnemyGridController _gridController;

    // Start is called before the first frame update
    void Start()
    {
        _playerBulletLayer = LayerMask.NameToLayer("Player Bullets");

        _gridController = transform.parent.gameObject.GetComponent<EnemyGridController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == _playerBulletLayer)
        {
            collider.gameObject.SetActive(false);
            Destroy(collider.gameObject);
            
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().Play("EnemyDie");
            _gridController.OnEnemyDeath(gameObject);
        }
    }

    private void OnDeathAnimationEnd()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
