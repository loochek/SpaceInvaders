using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabelBlinkComponent : MonoBehaviour
{
    public float blinkSpeed = 1.0f;

    private float _nextToggleTime;
    private TMP_Text _label;
    
    // Start is called before the first frame update
    void Start()
    {
        _label = GetComponent<TMP_Text>();
        _nextToggleTime = Time.time + blinkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _nextToggleTime)
        {
            _label.enabled = !_label.enabled;
            _nextToggleTime = Time.time + blinkSpeed;
        }
    }
}
