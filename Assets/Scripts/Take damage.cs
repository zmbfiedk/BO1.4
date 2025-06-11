using System;
using UnityEngine;

public class Takedamage : MonoBehaviour
{
    public static event Action onDeath;

    private SpriteRenderer sp;
    private Enemyfollow EF;

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        EF = GetComponent<Enemyfollow>();
    }

    void Update()
    {
        EnemySee();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            Hitdetection();
        }
    }

    public void Hitdetection()
    {
        onDeath?.Invoke(); //This is all you need
        Destroy(gameObject);
    }

    private void EnemySee()
    {
        if (EF == null) return;
        sp.color = EF.follow ? Color.red : Color.green;
    }
}
