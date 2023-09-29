using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth, damageTaken;
 

    [SerializeField] protected bool isDead;
    protected bool isHeadhshot;
    private void Start()
    {
        InitVariables();
    }
    public void CheckHealth()
    {
        if(health <= 0)
        {
            health = 0;
            Die();
        }
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
    }
    public virtual void Die()
    {
        isDead = true;
    }

    public void SetHealthTo(int healthToSetTo)
    {
        health = healthToSetTo;
        CheckHealth();
    }
    public virtual void TakeDamage(int damage, bool headshot)
    {
        damageTaken = damage;
        isHeadhshot = headshot;
        int healthAfterDamage = health - damageTaken;
        SetHealthTo(healthAfterDamage);
    }
    public void Heal(int heal)
    {
        int healthAfterHeal = health = heal;
        SetHealthTo(healthAfterHeal);
    }
    public virtual void InitVariables()
    {
        maxHealth = 100;
        SetHealthTo(maxHealth);
        isDead = false;
    }

}
