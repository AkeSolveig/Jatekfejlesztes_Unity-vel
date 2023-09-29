using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieStats : CharacterStats
{
    [SerializeField] private int damage;
    public float attackSpeed;

    [SerializeField] private bool canAttack;
    public GameObject head;
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        InitVariables();
        setRigidBodyState(true);
        setColliderState(true);
    }
    public override void InitVariables()
    {
        maxHealth = 25;
        SetHealthTo(maxHealth);
        isDead = false;

        damage = 10;
        attackSpeed = 1.9f;
        canAttack = true;
    }
    public void DealDamage(CharacterStats statsToDamage)
    {
        statsToDamage.TakeDamage(damage, false);
    }

    public override void Die()
    {
        base.Die();
        Debug.Log(damageTaken);
        if (isHeadhshot && damageTaken >= 10)
        {
            head.transform.localScale = new Vector3(0f, 0f, 0f);
        }
        //Destroy(gameObject);
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.GetComponent<ZombieController>().enabled = false;
        setRigidBodyState(false);
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

  


    void setRigidBodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        //GetComponent<Rigidbody>().isKinematic = !state;
    }
    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
            Physics.IgnoreCollision(collider, player.GetComponent<Collider>());
        }
        //GetComponent<Collider>().enabled = !state;
    }

}
