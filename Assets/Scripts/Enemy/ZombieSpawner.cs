using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawners;
    private List<Transform> activeSpawners = new List<Transform>();

    public void GetSpawners()
    {
        activeSpawners.Clear();
        spawners = GetComponentsInChildren<Transform>();
        spawners = spawners.Where(t => t != transform).ToArray();
        foreach (Transform spawner in spawners)
        {
            activeSpawners.Add(spawner);
        }
    }
    public void SpawnZombie(GameObject zombie,int zombiesHealth, bool isRunning)
    {
        GetSpawners();
        int randomInt = Random.Range(0, activeSpawners.Count);
        Transform randomSpawner = activeSpawners[randomInt];
        GameObject newZombie = Instantiate(zombie, randomSpawner.position, randomSpawner.rotation);
        CharacterStats newZombieStats = newZombie.GetComponent<CharacterStats>();
        newZombieStats.SetMaxHealthTo(zombiesHealth);
        newZombieStats.isRunner = isRunning;
    }


    
}
