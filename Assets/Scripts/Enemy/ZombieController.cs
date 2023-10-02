using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private float timeOfLastAttack = 0;
    private bool hasStopped = false;
    private bool isAttacking = false;

    private NavMeshAgent agent = null;
    private Animator animator = null;
    private ZombieStats stats = null;
    private Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("AITarget").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<ZombieStats>();
        //animator.SetFloat("speedMultiplier", 0.5f);
    }

    private void Update()
    {
        MoveToPlayer();
    }

    private void RotateToTarget()
    {
        
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }

    private void MoveToPlayer()
    {
        agent.SetDestination(target.position);
        //RotateToTarget();
        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(distanceToTarget <= agent.stoppingDistance)
        {

            if (!hasStopped)
            {
                animator.SetBool("HasStopped", true);
                hasStopped = true;
                timeOfLastAttack = Time.time;
            }
            CharacterStats targetStats = target.parent.gameObject.GetComponent<CharacterStats>();
            if(!isAttacking)
                StartCoroutine(AttackTarget(targetStats));

        }
        else
        {
            if (hasStopped)
            {
                hasStopped = false;
                animator.SetBool("HasStopped", false);
            }
        }
    }

    private IEnumerator AttackTarget(CharacterStats statsToDamage)
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

      
        var animController = GetComponent<Animator>().runtimeAnimatorController;
        var clip = animController.animationClips.First(a => a.name == "Zombie Attack");
        
        yield return new WaitForSeconds(clip.length/2);
        if(Vector3.Distance(target.position, transform.position) <= agent.stoppingDistance)
        {
            stats.DealDamage(statsToDamage);
        }
        yield return new WaitForSeconds(clip.length / 2);
        isAttacking = false;
    }
}
