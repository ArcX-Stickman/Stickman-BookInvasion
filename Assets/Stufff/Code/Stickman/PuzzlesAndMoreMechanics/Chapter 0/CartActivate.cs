using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartActivate : MonoBehaviour
{
    public Movement StickManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StickManager.Cart.gameObject.SetActive(true);
            StickManager.StickAnim.SetBool("Cart",true);
            Destroy(gameObject);
        }
    }
}
