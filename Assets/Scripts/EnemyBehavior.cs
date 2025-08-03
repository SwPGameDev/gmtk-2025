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

    private bool canAttack = true;
    public float attackCooldown = 2;
    private float attackTimer = 0;

    public AudioSource hurtSounds;
    public AudioSource attackSounds;
    public AudioSource deathSounds;

    Collider col;

    public Animator anim;
    public float moveDelay = 2;
    float delayTimer = 0;
    bool startMoving = false;

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (delayTimer < moveDelay)
        {
            delayTimer += Time.deltaTime;
        }
        else
        {
            startMoving = true;
        }

        if (!dead && startMoving)
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
                anim.SetTrigger("attack");

                canAttack = false;
                collision.gameObject.GetComponent<PlayerBehavior>().TakeHit(damage);

                //attackSounds.Play();
            }
        }
    }

    public void TakeHit(float damageParam)
    {
        currentHealth -= damageParam;
        //hurtSounds.Play();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            anim.SetTrigger("hitReaction");
        }
    }

    public void Die()
    {
        anim.SetTrigger("death");
        col.enabled = false;
        //deathSounds.Play();
        dead = true;
        agent.enabled = false;

        Destroy(gameObject, 5);
    }
}