using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class gerakburung : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] Rigidbody2D _rb;

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector2(speed,0);
    }
}
