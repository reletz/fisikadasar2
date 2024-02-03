using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class layangmove : MonoBehaviour
{
    [SerializeField] float speed = 5;
    Vector2 move;
    public float x1 = 60;
 

    // Update is called once per frame
    void Update()
    {
        move = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        transform.Translate(move*speed*Time.deltaTime);
        transform.position = new Vector2(Mathf.Clamp(transform.position.x,-x1,x1),transform.position.y);
    }
}
