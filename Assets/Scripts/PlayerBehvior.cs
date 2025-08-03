using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour, IUnit
{
    public AudioSource hurtAudio;


    // STATS
    public float MaxHealth = 50;
    public float currentHealth;
    public float weaponDamage = 1;

    void Start()
    {
        currentHealth = MaxHealth;
    }


    public void TakeHit(float damageParam)
    {
        currentHealth -= damageParam;

        hurtAudio.Play();

        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }


    public void PlayerDie()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
