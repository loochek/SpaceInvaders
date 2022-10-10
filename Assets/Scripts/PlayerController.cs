using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public GameObject proj;
    public Vector3 projSpawnOffset = new Vector3(0.0f, 1.0f);

    private GameManager _gameMgr;
    private int _enemyBulletLayer;
    private bool _dead = false;

    // Start is called before the first frame update
    void Start()
    {
        _enemyBulletLayer = LayerMask.NameToLayer("Enemy Bullets");
        _gameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_dead)
        {
            return;
        }

        Vector3 position = transform.position;
        position.x += movementSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        transform.position = position;

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(proj, position + projSpawnOffset, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == _enemyBulletLayer)
        {
            collider.gameObject.SetActive(false);
            Destroy(collider.gameObject);
            
            _dead = true;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().Play("PlayerDie");
            _gameMgr.OnPlayerDeath();
        }
    }

    private void OnDeathAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}