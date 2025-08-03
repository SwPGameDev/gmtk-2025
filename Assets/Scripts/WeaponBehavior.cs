using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public LayerMask enemyLayerMask;

    public Rigidbody rb;
    public PlayerBehavior playerBehavior;

    float baseDamage = 1;

    public float slowDownOnHitMod;
    public float enemyMass = 1;

    public float minimumSpinVelocity = 1;

    // Audio
    public AudioSource impactSound;
    public AudioSource whistleSound;

    private void Start()
    {
        baseDamage = playerBehavior.weaponDamage;
    }

    private void FixedUpdate()
    {
        whistleSound.pitch = Mathf.Clamp(Mathf.Abs(rb.angularVelocity.y), 0, 5);
        whistleSound.volume = Mathf.Clamp(Mathf.Abs(rb.angularVelocity.y), 0, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Mathf.Abs(rb.angularVelocity.y) > minimumSpinVelocity && (enemyLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Debug.Log(other.name);
            other.GetComponent<EnemyBehavior>().TakeHit(baseDamage + Mathf.Clamp(Mathf.Abs(rb.angularVelocity.y) - 5, 0, 20));
            Debug.Log("Combined Damage: " + (baseDamage + Mathf.Clamp(Mathf.Abs(rb.angularVelocity.y) - 5, 0, 20)));

            if (rb.angularVelocity.y > 0 )
            {
                rb.angularVelocity = new(0, rb.angularVelocity.y - (enemyMass * slowDownOnHitMod), 0);
            }
            else
            {
                rb.angularVelocity = new(0, rb.angularVelocity.y + (enemyMass * slowDownOnHitMod), 0);
            }

            //rb.angularVelocity = new(rb.angularVelocity.x, rb.angularVelocity.y / slowDownOnHitMod, rb.angularVelocity.z);

            impactSound.Play();
        }
    }
}