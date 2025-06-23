using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{

    [SerializeField] Transform PlayerTf;
    Vector3 offset;
    void Start()
    {
        PlayerTf = GameObject.FindWithTag("PlayerTrack").transform;
        offset = new Vector3(0, 0, 10);
    }

    void Update()
    {
        gameObject.transform.position = PlayerTf.position + offset;
    }
}
