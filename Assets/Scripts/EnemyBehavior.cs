using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;


public class EnemyBehavior : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth;
    public bool dead = false;

    public GameObject target;
    private NavMeshAgent agent;

    public LayerMask playerLayerMask;
    public float damage = 5;
    public float range = 2;

    private bool canAttack = true;
    public float attackCooldown = 2;
    private float attackTimer = 0;

    public AudioSource hurtSounds;
    public AudioSource attackSounds;
    public AudioSource deathSounds;
    public AudioSource impactSounds;

    Collider col;

    public Animator anim;
    public float moveSpeed = 3.5f;

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        agent.speed = 0;

        float randomDelay = Random.Range(-0.1f, 0.4f);

        StartCoroutine(DelayedMoveStart(2 + randomDelay));
    }

    private void Update()
    {
        if (!dead)
        {
            if (target != null)
            {
                agent.destination = target.transform.position;
            }

            if (!canAttack)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackCooldown)
                {
                    canAttack = true;
                    attackTimer = 0;
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (canAttack)
        {
            if ((playerLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
            {
                ResetAllTriggers();
                anim.SetTrigger("attack");
                agent.speed = 0;
                StartCoroutine(DelayedMoveStart(1.5f));
                StartCoroutine(AttackHitCheck(1));

                canAttack = false;

                attackSounds.Play();
            }
        }
    }

    private void ResetAllTriggers()
    {
        foreach (var param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                anim.ResetTrigger(param.name);
            }
        }
    }

    IEnumerator AttackHitCheck(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (Vector3.Distance(transform.position, target.transform.position) < range)
        {
            impactSounds.Play();
            Debug.Log("Hit, range: " + (Vector3.Distance(transform.position, target.transform.position)));
            target.GetComponent<PlayerBehavior>().TakeHit(damage);
        }
    }

    IEnumerator DelayedMoveStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetAllTriggers();
        anim.SetTrigger("walk");
        agent.speed = moveSpeed;
    }

    public void TakeHit(float damageParam)
    {
        currentHealth -= damageParam;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            hurtSounds.Play();
            ResetAllTriggers();
            anim.SetTrigger("hitReaction");
            agent.speed = 0;
            StartCoroutine(DelayedMoveStart(2));
        }
    }

    public void Die()
    {
        ScoreTracker.Instance.IncreaseScore();

        StopAllCoroutines();
        ResetAllTriggers();
        anim.SetTrigger("death");
        canAttack = false;
        col.enabled = false;
        deathSounds.Play();
        dead = true;
        agent.enabled = false;

        Destroy(gameObject, 5);
    }
}