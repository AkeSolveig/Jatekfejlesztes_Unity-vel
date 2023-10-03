using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private int enemiesNumber = 5;
    private int currentWave = 1;
    private int zombiesHealth = 100;
    private int chance = 0;
    private bool isSpawningWave;
    [SerializeField] private GameObject zombie1;

    //[SerializeField] private GameObject zombie2;
    public GameObject[] enemies;
    private float[] percentages = {100f,0f};
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
            zombiespawner.SpawnZombie(enemies[Random.Range(0,2)],zombiesHealth,IsRunner());
            yield return new WaitForSeconds(1f);
        }
        currentWave++;
        BuffZombies();
        isSpawningWave = false;
    }

    private void BuffZombies()
    {
        enemiesNumber += 2;
        zombiesHealth += 50;
        isSpawningWave = false;
    }

    private bool IsRunner()
    {
        int randValue = Random.Range(0, 100);
        if(currentWave == 2)
        {
            chance = 30;
        }
        if(randValue <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
       
    }

    private int GetRandomSpawn()
    {
        float random = Random.Range(0f, 1f);
        float numForAdding = 0;
        float total = 0;
        for(int i = 0; i < percentages.Length; i++)
        {
            total += percentages[i];
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (percentages[i]/total + numForAdding >= random)
            {
                return i;
            }
            else
            {
                numForAdding = +percentages[i] / total;
            }
        }
        return 0;
    }
 

}
