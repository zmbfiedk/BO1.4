using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{

    [SerializeField] Transform PlayerTf;
    void Start()
    {
        
    }

    void Update()
    {
        gameObject.transform.position = PlayerTf.position;
    }
}
