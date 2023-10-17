using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieStats : CharacterStats
{
    [SerializeField] private int damage;
    public static int numberOfEnemies;

    public GameObject head;
    private GameObject player;
    private Points pointsScript;
    private void Awake()
    {
        InitVariables();
    }
    private void Start()
    {
        numberOfEnemies++;
        pointsScript = GameObject.FindGameObjectWithTag("PointsController").GetComponent<Points>();
        player = GameObject.FindGameObjectWithTag("Player");
        setRigidBodyState(true);
        setColliderState(false);
    }
    public override void InitVariables()
    {
        SetHealthTo(maxHealth);
        isDead = false;
        damage = 10;
    }
    public void DealDamage(CharacterStats statsToDamage)
    {
        statsToDamage.TakeDamage(damage, false);
    }

    public override void TakeDamage(float damage, bool headshot)
    {
        if (!isDead)
        {
            base.TakeDamage(damage, headshot);
            pointsScript.AddScore(10);
        }       
    }

    public override void Die()
    {
        base.Die();
        pointsScript.AddScore(90);
        Debug.Log(damageTaken);
        if (isHeadhshot && damageTaken >= 10)
        {
            head.transform.localScale = new Vector3(0f, 0f, 0f);
        }
        
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<ZombieController>().enabled = false;
        setIgnoreRaycastState();
        setRigidBodyState(false);
        setColliderState(true);
        StartCoroutine(FadeIntoGround());
        
    }
    private IEnumerator FadeIntoGround()
    {
        yield return new WaitForSeconds(7);
        setRigidBodyState(true);
        float time = 0;
        while (time < 2)
        {
            transform.position += Vector3.down * Time.deltaTime/8;
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        numberOfEnemies--;
        Debug.Log(ZombieStats.numberOfEnemies);
    }


    void setIgnoreRaycastState()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach(Transform child in children)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    void setRigidBodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }
    }
    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            if (state)
            {
                Physics.IgnoreCollision(collider, player.GetComponent<Collider>());
            }
                
            Physics.IgnoreCollision(collider,
                            GameObject.FindGameObjectWithTag("SpawnerDoor").GetComponent<Collider>());
        }
    }

}
