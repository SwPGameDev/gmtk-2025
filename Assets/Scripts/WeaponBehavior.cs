using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public LayerMask enemyLayerMask;
    public AudioSource audioSource;
    public Rigidbody rb;
    public PlayerBehavior playerBehavior;

    float damage;

    public float slowDownOnHit;

    private void Start()
    {
        damage = playerBehavior.weaponDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((enemyLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Debug.Log(other.name);
            Debug.Log(rb.angularVelocity);
            other.GetComponent<EnemyBehavior>().TakeHit(damage);

            rb.angularVelocity = new(rb.angularVelocity.x, rb.angularVelocity.y / slowDownOnHit, rb.angularVelocity.z);
            audioSource.Play();
        }
    }
}