using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private int enemiesNumber = 5;
    private int currentWave = 1;
    private int zombiesHealth = 100;
    private int chance = 0;
    private bool isSpawningWave;

    //zombie types & spawning
    public GameObject[] enemies;
    private float[] percentages = {100f,0f};
    [SerializeField] private ZombieSpawner zombiespawner;

    //UI
    public TextMeshProUGUI waveNumberText;

    public AudioSource playerAudioSource;
    public AudioClip waveStartSound;

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
        UpdateWaveNumber();
        playerAudioSource.PlayOneShot(waveStartSound);
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
        enemiesNumber += 3;
        zombiesHealth += 100;
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
    private void UpdateWaveNumber()
    {
        waveNumberText.text = "" + currentWave;
    }

   
 

}
