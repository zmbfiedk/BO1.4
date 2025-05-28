
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyprefab;

    [SerializeField] private float minimunspawntime;

    [SerializeField] private float maximunspawntime;
    
    private float timeTilspawn;

    void Awake()
    {
        SetTimeUntilSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        timeTilspawn -= Time.deltaTime;

        if(timeTilspawn <= 0)
        {
            Instantiate(enemyprefab,transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeTilspawn = Random.Range(minimunspawntime, maximunspawntime);
    }
}
