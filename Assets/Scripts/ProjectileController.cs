using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 4.0f;
    public bool isPlayerProjectile;

    private Vector3 _dir;
    
    private List<int> _bulletLayers = new List<int>();
    
    // Start is called before the first frame update
    void Start()
    {
        _bulletLayers.Add(LayerMask.NameToLayer("Player Bullets"));
        _bulletLayers.Add(LayerMask.NameToLayer("Enemy Bullets"));
        
        _dir = new Vector3(0.0f, isPlayerProjectile ? speed : -speed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += _dir * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_bulletLayers.Contains(collider.gameObject.layer) &&
            _bulletLayers.Contains(gameObject.layer))
        {
            collider.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}
