using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoundaryBehavior : MonoBehaviour
{
    private List<int> _bulletLayers = new List<int>();
    
    // Start is called before the first frame update
    void Start()
    {
        _bulletLayers.Add(LayerMask.NameToLayer("Player Bullets"));
        _bulletLayers.Add(LayerMask.NameToLayer("Enemy Bullets"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_bulletLayers.Contains(collider.gameObject.layer))
        {
            collider.gameObject.SetActive(false);
            Destroy(collider.gameObject);
        }
    }
}
