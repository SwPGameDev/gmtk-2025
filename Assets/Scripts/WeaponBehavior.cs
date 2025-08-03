using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public LayerMask enemyLayerMask;

    public Rigidbody rb;
    public PlayerBehavior playerBehavior;

    float damage;

    public float slowDownOnHitMod;

    // Audio
    public AudioSource impactSound;
    public AudioSource whistleSound;

    private void Start()
    {
        damage = playerBehavior.weaponDamage;
    }

    private void FixedUpdate()
    {
        whistleSound.pitch = Mathf.Clamp(Mathf.Abs(rb.angularVelocity.y), 0, 5);
        whistleSound.volume = Mathf.Clamp(Mathf.Abs(rb.angularVelocity.y), 0, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((enemyLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Debug.Log(other.name);
            Debug.Log(rb.angularVelocity);
            other.GetComponent<EnemyBehavior>().TakeHit(damage);

            if (rb.angularVelocity.y > 0 )
            {
                rb.angularVelocity = new(0, rb.angularVelocity.y - (other.attachedRigidbody.mass * slowDownOnHitMod), 0);
            }
            else
            {
                rb.angularVelocity = new(0, rb.angularVelocity.y + (other.attachedRigidbody.mass * slowDownOnHitMod), 0);
            }

            //rb.angularVelocity = new(rb.angularVelocity.x, rb.angularVelocity.y / slowDownOnHitMod, rb.angularVelocity.z);

            impactSound.Play();
        }
    }
}