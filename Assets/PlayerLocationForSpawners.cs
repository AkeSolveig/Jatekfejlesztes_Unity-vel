using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocationForSpawners : MonoBehaviour
{
    //public bool playerInMainhall;
    public string currentLayer;
    public string previousLayer;
    Transform[] doors;


    GameObject zombieSpawner;
    Transform[] spawners;
    //private Door doorScript;

    private bool ignoreDefault = true;
    private void Start()
    {
        GameObject doorsToRooms = GameObject.FindGameObjectWithTag("DoorsToRooms");
        doors = doorsToRooms.GetComponentsInChildren<Transform>();
        

        zombieSpawner = GameObject.FindGameObjectWithTag("ZombieSpawner");
        spawners = zombieSpawner.GetComponentsInChildren<Transform>(true);
    }


    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            currentLayer = LayerMask.LayerToName(hit.collider.gameObject.layer);
            if (previousLayer != LayerMask.LayerToName(hit.collider.gameObject.layer))
            {
                if (!ignoreDefault)
                {
                    foreach (Transform spawner in spawners)
                    {
                        spawner.gameObject.SetActive(false);
                    }
                    
                }
                zombieSpawner.SetActive(true);
                CheckSpawners();
                ignoreDefault = false;
                previousLayer = currentLayer;
                Debug.Log(currentLayer);
            }

        }
        
    }
    public void CheckSpawners()
    {
        foreach (Transform door in doors)
        {
            if (LayerMask.LayerToName(door.gameObject.layer) == currentLayer)
            {
                Debug.Log(door.gameObject.name);
                if (gameObject.transform.childCount > 0)
                {
                    Door doorScript = door.GetComponentInParent<Door>();
                    if (doorScript.IsOpen)
                    {
                        TurnSpawnersActive(doorScript.connectedRoomsTag1, doorScript.connectedRoomsTag2);
                    }
                }
                else
                {
                    Door doorScript = door.GetComponent<Door>();
                    if (doorScript.IsOpen)
                    {
                        TurnSpawnersActive(doorScript.connectedRoomsTag1, doorScript.connectedRoomsTag2);
                    }
                }
                
            }
        }
    }
    private void TurnSpawnersActive(string spawnerTag1, string spawnerTag2)
    {
        Debug.Log(spawnerTag1 + spawnerTag2);
        foreach (Transform spawner in spawners)
        {
            if (spawner.gameObject.tag == spawnerTag1 || spawner.gameObject.tag == spawnerTag2)
            {
                
                spawner.gameObject.SetActive(true);
            }
        }
    }
}
