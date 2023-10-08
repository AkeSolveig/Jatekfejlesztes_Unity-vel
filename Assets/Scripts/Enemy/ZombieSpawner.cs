using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawners;
    private List<Transform> activeSpawners;
    //[SerializeField] private GameObject zombie;
    private Transform AITarget;
    private Transform mainHall;
    private bool isCheckingSpawners = false;

    private void Awake()
    {
        activeSpawners = new List<Transform>();
        AITarget = GameObject.FindGameObjectWithTag("AITarget").transform;
        mainHall = GameObject.FindGameObjectWithTag("MainHall").transform;
        GetSpawners();
    }
    public void GetSpawners()
    {
        spawners = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //SpawnZombie();
            foreach (var item in activeSpawners)
            {
                Debug.Log(item.ToString());
            }
            
        }
        if (!isCheckingSpawners)
        {
            StartCoroutine(CheckSpawners());
        }
        
    }

    private IEnumerator CheckSpawners()
    {
        isCheckingSpawners = true;
        GetSpawners();
        if (activeSpawners.Count != 0)
        {
            activeSpawners.Clear();
        }
        foreach (Transform spawner in spawners)
        {
            if (Vector3.Distance(AITarget.position, spawner.position) < 35
                && Vector3.Distance(AITarget.position, spawner.position) > 7
                && (Mathf.Abs(AITarget.position.y - spawner.position.y) < 5
                || Vector3.Distance(AITarget.position, mainHall.position) < 30))
            {

                activeSpawners.Add(spawner);
            }
        }
        yield return new WaitForSeconds(5);
        isCheckingSpawners = false;

    }

    public void SpawnZombie(GameObject zombie,int zombiesHealth, bool isRunning)
    {
        int randomInt = Random.Range(0, activeSpawners.Count);
        Transform randomSpawner = activeSpawners[randomInt];
        GameObject newZombie = Instantiate(zombie, randomSpawner.position, randomSpawner.rotation);
        CharacterStats newZombieStats = newZombie.GetComponent<CharacterStats>();
        newZombieStats.SetMaxHealthTo(zombiesHealth);
        newZombieStats.isRunner = isRunning;
    }


    
}
