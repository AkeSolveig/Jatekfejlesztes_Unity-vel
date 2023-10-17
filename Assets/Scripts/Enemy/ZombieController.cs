using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private bool hasStopped = false;
    private bool isAttacking = false;

    private NavMeshAgent agent = null;
    private Animator animator = null;
    private ZombieStats stats = null;
    private Transform target;

    private AudioSource audioSource;
    private AudioClip lastIdleAudioClip;
    private AudioClip lastAttackAudioClip;
    public AudioClip[] attackClips;
    public AudioClip[] idleClips;
    private bool hasAudioEnded = true;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("AITarget").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<ZombieStats>();
        if (stats.isRunner == true)
        {
            animator.SetBool("isRunning", true);
            agent.speed = 3f;
        }
    }

    private void Update()
    {
        MoveToPlayer();
        if (!isAttacking && hasAudioEnded)
        {
            StartCoroutine(PlayIdleSound());
        }
        
    }
    private IEnumerator PlayIdleSound()
    {
        hasAudioEnded = false;
        while (audioSource.clip == lastIdleAudioClip)
        {
            audioSource.clip = idleClips[Random.Range(0, idleClips.Length)];
        }
        lastIdleAudioClip = audioSource.clip;
        audioSource.Play();
        yield return new WaitForSeconds(Random.Range(5f, 10f));
        hasAudioEnded = true;
    }

    private void RotateToTarget()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }

    private void MoveToPlayer()
    {
        if (!isAttacking)
        {
            agent.SetDestination(target.position);
        } 
        else
        {
            RotateToTarget();
        }
            
        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(distanceToTarget <= agent.stoppingDistance)
        {
            if (!hasStopped)
            {
                animator.SetBool("HasStopped", true);
                hasStopped = true;
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

        while (audioSource.clip == lastAttackAudioClip)
        {
            audioSource.clip = attackClips[Random.Range(0, attackClips.Length)];
        }
        lastAttackAudioClip = audioSource.clip;
        audioSource.Play();


        animator.SetInteger("AttackIndex", Random.Range(0, 3));
        animator.SetTrigger("Attack");

        var animController = GetComponent<Animator>().runtimeAnimatorController;
        var clip = animController.animationClips.First(a => a.name == "Zombie Attack");
        
        yield return new WaitForSeconds(clip.length/2);
        if(Vector3.Distance(target.position, transform.position) <= agent.stoppingDistance + 0.5f)
        {
            stats.DealDamage(statsToDamage);
        }
        yield return new WaitForSeconds(clip.length / 2);

        isAttacking = false;
    }
}
