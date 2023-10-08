using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    [SerializeField] private Image redSplatterImage = null;
    [SerializeField] private Image hurtImage = null;
    [SerializeField] private float hurtTimer = 0.1f;

    [SerializeField] private int regenRate = 1;
    private bool canRegen = false;
    [SerializeField] private float healCooldown = 3.0f;
    [SerializeField] private float maxHealCooldown = 3.0f;
    private bool startCooldown = false;
    public bool isHealthUpgraded = false;
    private void Start()
    {
        InitVariables();
    }

    void UpdateHealth()
    {
        Color splatterAlpha = redSplatterImage.color;
        splatterAlpha.a = 1 - (health / maxHealth);
        redSplatterImage.color = splatterAlpha;
    }

    IEnumerator HurtFlash()
    {
        hurtImage.enabled = true;
        yield return new WaitForSeconds(hurtTimer);
        hurtImage.enabled = false;
    }

    public override void TakeDamage(float damage, bool headshot)
    {
        damageTaken = damage;
        isHeadhshot = headshot;
        float healthAfterDamage = health - damageTaken;
        SetHealthTo(healthAfterDamage);
        if (health >= 0)
        {
            canRegen = false;
            StartCoroutine(HurtFlash());
            UpdateHealth();
            healCooldown = maxHealCooldown;
            startCooldown = true;
        }
    }
    private void Update()
    {
        if (startCooldown)
        {
            healCooldown -= Time.deltaTime;
            if (healCooldown <= 0)
            {
                canRegen = true;
                startCooldown = false;
            }
        }

        if (canRegen)
        {
            if (health <= maxHealth - 0.01)
            {
                health += Time.deltaTime * regenRate;
                UpdateHealth();
            }
            else
            {
                health = maxHealth;
                healCooldown = maxHealCooldown;
                canRegen = false;
            }
        }
    }
}
