using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAi : MonoBehaviour
{
    private Vector2 raycastreference;
    GameObject player;
    private LayerMask layermask = 0; // empty until obstacles added
    private bool isAllowed;
    private Vector2 moveposition;
    private bool isMoving;
    [SerializeField] private float speed = 1;
    private EnemyProjectileAttack attack;
    private bool isSpotted = false;
    private int movementFailsafe = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        attack = GetComponent<EnemyProjectileAttack>();
        StartCoroutine(enemybehaviour());
        int layerToAdd = LayerMask.NameToLayer("environmental objects");
        layermask |= 1 << layerToAdd;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Vector2 currentPos = transform.position;
            Vector2 direction = (moveposition - currentPos).normalized;
            float distanceThisFrame = speed * Time.deltaTime;
            float distanceToTarget = Vector2.Distance(currentPos, moveposition);

            if (distanceToTarget > distanceThisFrame)
            {
                transform.position += new Vector3(direction.x, direction.y, 0) * distanceThisFrame;
            }
            else
            {
                // Snap to target and stop
                transform.position = new Vector3(moveposition.x, moveposition.y, 0);
                attack.enabled = true;
                isMoving = false;
            }
        }
    }
    private IEnumerator enemybehaviour()
    {
        if (isAllowed)
        {
            isMoving = true;
            isAllowed = false;
            yield return new WaitForSeconds(UnityEngine.Random.Range(3, 7));
            PositionRandomizer();
        }
        else if (!isAllowed)
        {
            movementFailsafe++;
            Debug.Log($"failed attempts = {movementFailsafe}");
            if (movementFailsafe < 30)
            {
                yield return new WaitForSeconds(0.1f); //toned down cooldown if position is invalid 
            }
            else if (movementFailsafe > 30)
            {
                isMoving = true;
                movementFailsafe = 0;
                yield return new WaitForSeconds(UnityEngine.Random.Range(3, 7));
            }
            PositionRandomizer();
        }
    }
    private void PositionRandomizer()
    {
        moveposition = new Vector2(gameObject.transform.position.x + UnityEngine.Random.Range(-1, 2), (gameObject.transform.position.y + UnityEngine.Random.Range(-1, 2)));
        
        RaycastCheck();
    }
    private void RaycastCheck()
    {
        raycastreference = player.transform.position - gameObject.transform.position;
        int distance = ((int)raycastreference.magnitude);
        raycastreference.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, raycastreference, distance, layermask);
        if (hit.collider != null && isSpotted)
        {
            isAllowed = false;
            moveposition = gameObject.transform.position;
        }
        if (hit.collider == null || !isSpotted)
        {
            isAllowed = true;
            attack.enabled = false;
            isSpotted = true;
        }
        StartCoroutine(enemybehaviour());
    }
}
