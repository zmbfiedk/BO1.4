using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    GameObject player;
    private Vector2 movedirection;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        getdirection();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)(movedirection * speed * Time.deltaTime);
    }
    private void getdirection()
    {
        movedirection = player.transform.position - gameObject.transform.position;
        movedirection = movedirection.normalized;
    }
}
