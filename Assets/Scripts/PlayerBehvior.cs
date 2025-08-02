using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour, IUnit
{

    // STATS
    public float MaxHealth = 50;
    public float currentHealth;
    public float weaponDamage = 5;

    void Start()
    {
        currentHealth = MaxHealth;
    }


    public void TakeHit(float damageParam)
    {
        currentHealth -= damageParam;
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
