using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting.APIUpdating;

public class overgame : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField]UnityEvent TriggerEff;
    [SerializeField] string overTag;

     void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag(overTag))
        {
            TriggerEff?.Invoke();
        }
        _rb.velocity = new Vector2(0,0);
        _rb.gravityScale = 0;
    }

}
