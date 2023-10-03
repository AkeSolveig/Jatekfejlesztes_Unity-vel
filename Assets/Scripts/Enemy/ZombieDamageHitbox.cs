using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDamageHitbox : MonoBehaviour
{
    public enum collisonType { head, body, limbs}
    public collisonType bodyPart;

    public ZombieStats stats;

    public void BodyPartHit(int damage,bool headshot)
    {
        stats.TakeDamage(damage, headshot);
    }
}
