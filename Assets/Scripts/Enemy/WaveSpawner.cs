using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    //default settings
    private int enemiesNumber = 5;
    private int currentWave = 1;
    private int zombiesHealth = 100;
    private int chance = 0;
    private bool isSpawningWave;

    //zombie types & spawning
    public GameObject[] enemies;
    [SerializeField] private ZombieSpawner zombiespawner;

    //UI
    public TextMeshProUGUI waveNumberText;

    //AUDIO
    public AudioSource playerAudioSource;
    public AudioClip waveStartSound;


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
        UpdateWaveNumber();
        playerAudioSource.PlayOneShot(waveStartSound);
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < enemiesNumber; i++)
        {
            zombiespawner.SpawnZombie(enemies[Random.Range(0,2)],zombiesHealth,IsRunner());
            yield return new WaitForSeconds(1f);
        }
        currentWave++;
        BuffZombies();
    }

    private void BuffZombies()
    {
        enemiesNumber += 4;
        zombiesHealth += 100;
        isSpawningWave = false;
        if (currentWave == 3)
        {
            chance = 20;
        }
        if (currentWave == 6)
        {
            chance = 40;
        }
        if (currentWave == 9)
        {
            chance = 60;
        }
        if (currentWave == 12)
        {
            chance = 80;
        }
        if (currentWave == 15)
        {
            chance = 100;
        }
    }

    private bool IsRunner()
    {
        int randValue = Random.Range(0, 100);

        if (randValue <= chance)
        {
            return true;
        }
        else
        {
            return false;
        }
       
    }
    private void UpdateWaveNumber()
    {
        waveNumberText.text = "" + currentWave;
    }

   
 

}
