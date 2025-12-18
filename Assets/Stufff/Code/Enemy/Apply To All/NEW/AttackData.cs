using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "BookAI/Attack")]
public class AttackData : ScriptableObject
{

    [Header("Basic Stuff")]
    public float Cooldown;
    public bool isMelee;
    public bool isRanged;
    public float Knockback;




}
