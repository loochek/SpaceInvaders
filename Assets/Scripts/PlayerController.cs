using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 10.0f;
    public GameObject proj;
    public Vector3 projSpawnOffset = new Vector3(0.0f, 1.0f);
    public Color defaultColor = new Color(1.0f, 1.0f, 1.0f);
    public Color immortalColor = new Color(0.0f, 1.0f, 0.0f);
    public float immortalTime = 5.0f;

    private GameManager _gameMgr;
    private SpriteRenderer _spriteComponent;
    private int _enemyBulletLayer;
    
    private bool _dead = false;
    private bool _immortal = false;
    private float _immortalEndTime;
    
    private bool Immortal
    {
        get => _immortal;
        set
        {
            _spriteComponent.color = value ? immortalColor : defaultColor;
            _immortal = value;
            _immortalEndTime = Time.time + immortalTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _enemyBulletLayer = LayerMask.NameToLayer("Enemy Bullets");
        _gameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spriteComponent = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_dead)
        {
            return;
        }

        if (Immortal && Time.time > _immortalEndTime)
        {
            Immortal = false;
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
        if (!_immortal && collider.gameObject.layer == _enemyBulletLayer)
        {
            collider.gameObject.SetActive(false);
            Destroy(collider.gameObject);
            
            _dead = true;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().Play("PlayerDie");
            _gameMgr.OnPlayerDeath();
        }
        else if (collider.gameObject.CompareTag("Immortal Bonus"))
        {
            collider.gameObject.SetActive(false);
            Destroy(collider.gameObject);
            Immortal = true;
        }
    }

    private void OnDeathAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}