using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private int enemiesNumber = 5;
    private int currentWave = 1;
    private int zombiesHealth = 100;
    private bool isSpawningWave;
    [SerializeField] private GameObject zombie1;
    [SerializeField] private ZombieSpawner zombiespawner;


    private void Start()
    {
        StartCoroutine(SpawningWave());
    }
    private void Update()
    {
        if (!isSpawningWave && ZombieStats.numberOfEnemies == 0)
        {
            StartCoroutine(SpawningWave());
        }
        
    }

    private IEnumerator SpawningWave()
    {
        isSpawningWave = true;
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < enemiesNumber; i++)
        {
            zombiespawner.SpawnZombie(zombie1,zombiesHealth);
            yield return new WaitForSeconds(1f);
        }
        currentWave++;
        isSpawningWave = false;
        BuffZombies();
    }

    private void BuffZombies()
    {
        enemiesNumber += 2;
        zombiesHealth += 50;
        isSpawningWave = false;
    }
 

}
