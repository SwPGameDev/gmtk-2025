using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyBehavior : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth;
    public bool dead = false;

    public GameObject target;
    NavMeshAgent agent;
    Rigidbody rb;

    public LayerMask playerLayerMask;
    float distanceToTarget;
    public float range = 5;
    public float damage = 5;

    bool canAttack = true;
    public float attackCooldown = 1;
    float attackTimer = 0;

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
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
                }
            }

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((playerLayerMask.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            if (canAttack)
            {
                canAttack = false;
                collision.gameObject.GetComponent<PlayerBehavior>().TakeHit(damage);

                //audioSource.Play();
            }
        }
    }

    public void TakeHit(float damageParam)
    {
        currentHealth -= damageParam;
        if (currentHealth <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        dead = true;
        agent.enabled = false;
        rb.isKinematic = false;
        rb.AddForce(500 * Vector3.up);
    }
}