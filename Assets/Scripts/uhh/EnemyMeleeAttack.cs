using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMeleeAttack : MonoBehaviour
{
    public float rangeX = 2f;
    public float rangeY = 2f;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        IsplayerInRange();
        if (IsplayerInRange())
        {
            StartAttack();
        }
    }
    bool IsplayerInRange()
    {
        Vector3 delta = player.position - transform.position;

        if(Mathf.Abs(delta.x) <= rangeX && Mathf.Abs(delta.y) <= rangeY)
        {
            return true;
        }
        return false;
        
    }
    private void StartAttack()
    {
        //run potential animation

    }
}
