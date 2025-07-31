using UnityEngine;

public class PlayerBehvior : MonoBehaviour, IUnit
{
    public float MaxHealth = 50;
    public float currentHealth;

    void Start()
    {
        currentHealth = MaxHealth;
    }

    void Update()
    {
        
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

    }
}
