using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform tf;

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 direction = tf.position - transform.position;
        float distance = direction.magnitude;
        float minDistance = 1.5f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (distance > minDistance)
        {
            float speed = (distance - minDistance) * 5f;
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }

}
