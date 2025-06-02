using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ObjectCollision : MonoBehaviour
{
    Collider2D objectCollider;
    // Start is called before the first frame update
    void Start()
    {
        objectCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        if (tag == "Environmental Object" || tag == "Player")
        {
            Destroy(gameObject);
        }
        else if (tag != "Enemy")
        {
           Debug.LogWarning($"collision tag did not match any predefined tags. predefined tags are: Environmental Object, Player, Enemy. tag is: {tag}");
        }  
    }
}
