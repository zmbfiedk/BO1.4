﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class SwordFollow : MonoBehaviour
{
   
    public Transform tf;
    public float minDistance = 1.5f;
    public float speed = 6f;

    private void FollowPlayer()
    { 
        Vector2 direction = tf.position - transform.position;
        float distance = direction.magnitude;  
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (distance > minDistance)
        {
            float speed = (distance - minDistance) * 5f;
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }

    public void Attack()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("Mouse button is being held down!");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = Vector3.MoveTowards(transform.position, mousePos, speed * Time.deltaTime);
            Vector2 direction = mousePos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }


    void Start()
    {
        
    }

    void Update()
    {
        Attack();
        FollowPlayer();
    }
    
}
