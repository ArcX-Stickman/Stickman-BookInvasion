using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologyBook : MonoBehaviour
{
    public float attackCD;
    public float damage;
    public BoxCollider2D boxCollider2D;
    public LayerMask PlayerLayer;
    public float cooldownTimer = Mathf.Infinity;
    public float range;
    public Animator BioAmin;
    public Health PlayerHealth;

    public void Start()
    {
        BioAmin = GetComponent<Animator>();
    }


    // Update is called once per frame
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if(cooldownTimer >= attackCD)
        {
            cooldownTimer = 0;
            BioAmin.SetTrigger("Attack");
        }
    }



    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x, new Vector3(boxCollider2D.bounds.size.x * range, boxCollider2D.bounds.size.y, boxCollider2D.bounds.size.z),
            0, Vector2.left, 0, PlayerLayer);


        
        
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider2D.bounds.center + transform.right * range,boxCollider2D.bounds.size);
    }


    public void DamageStickman()
    {
        if (PlayerInSight())
        {
            PlayerHealth.PlayerHP = PlayerHealth.PlayerHP - 30;
        }
    }


}
