using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AimTarget : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    
    private void Awake() {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable() {
        AimAssistSystem.RegisterTarget(_boxCollider);
    }

    private void OnDisable() {
        AimAssistSystem.DeleteTarget(_boxCollider);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
