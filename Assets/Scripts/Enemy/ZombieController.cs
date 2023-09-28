using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    // Start is called before the first frame update

    private NavMeshAgent agent = null;
    private Animator animator = null;
    [SerializeField] Transform target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        MoveToPlayer();
    }

    private void RotateToTarget()
    {
        
        /*Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;*/
    }

    private void MoveToPlayer()
    {
        agent.SetDestination(target.position);
        animator.SetFloat("Speed", 0.5f, 0.3f,Time.deltaTime);
        //RotateToTarget();
    }
}
