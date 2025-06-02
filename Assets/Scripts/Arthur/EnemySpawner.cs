
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("level1")]

    [SerializeField] private GameObject Lmeleeenemyprefab1;
    [SerializeField] private GameObject Hmeleeenemyprefab1;
    [SerializeField] private GameObject Rangeenemyprefab1;

    [Header("level2")]

    [SerializeField] private GameObject Lmeleeenemyprefab2;
    [SerializeField] private GameObject Hmeleeenemyprefab2;
    [SerializeField] private GameObject Rangeenemyprefab2;

    [Header("level3")]

    [SerializeField] private GameObject Lmeleeenemyprefab3;
    [SerializeField] private GameObject Hmeleeenemyprefab3;
    [SerializeField] private GameObject Rangeenemyprefab3;

    [SerializeField] private float minimunspawntime;
    [SerializeField] private float maximunspawntime;

    private float timeTilspawn;

    void Awake()
    {
        SetTimeUntilSpawn();
    }

    void Update()
    {
        timeTilspawn -= Time.deltaTime;

        if(timeTilspawn <= 0)
        {
            Instantiate(Lmeleeenemyprefab1,transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeTilspawn = Random.Range(minimunspawntime, maximunspawntime);
    }
}
