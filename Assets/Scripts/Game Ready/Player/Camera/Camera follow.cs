using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerafollow : MonoBehaviour
{
    [SerializeField] Transform PlayerTf;
    Vector3 offset;

    public enum FollowMode { Lerp, SmoothDamp }
    public FollowMode followMode = FollowMode.Lerp;

    [Header("Lerp Settings")]
    public float lerpSmoothSpeed = 5f;

    [Header("SmoothDamp Settings")]
    public float smoothTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        PlayerTf = GameObject.FindWithTag("PlayerTrack").transform;
        offset = new Vector3(0, 0, 10);
    }

    void LateUpdate()
    {
        Vector3 targetPos = PlayerTf.position + offset;

        if (followMode == FollowMode.Lerp)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSmoothSpeed * Time.deltaTime);
        }
        else if (followMode == FollowMode.SmoothDamp)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }
}
