using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth, damageTaken;
 

    [SerializeField] protected bool isDead;
    protected bool isHeadhshot;
    private void Start()
    {
        InitVariables();
    }
    public virtual void CheckHealth()
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
        //Debug.Log("CheckHealth called" + health + " " + maxHealth);
    }
    public virtual void Die()
    {
        isDead = true;
    }
    public bool IsDead()
    {
        return isDead;
    }

    public void SetHealthTo(float healthToSetTo)
    {
        //Debug.Log("SethealthTo called, " + healthToSetTo);
        health = healthToSetTo;
        CheckHealth();
    }
    public void SetMaxHealthTo(float maxHealthToSetTo)
    {
        maxHealth = maxHealthToSetTo;
        health = maxHealthToSetTo;
        CheckHealth();
    }
    public virtual void TakeDamage(float damage, bool headshot)
    {
        damageTaken = damage;
        isHeadhshot = headshot;
        float healthAfterDamage = health - damageTaken;
        SetHealthTo(healthAfterDamage);
    }
    public virtual void InitVariables()
    {
        maxHealth = 100;
        SetHealthTo(maxHealth);
        isDead = false;
    }

}
