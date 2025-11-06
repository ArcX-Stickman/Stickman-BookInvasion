using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chem_ExplosionKnockOut : MonoBehaviour
{
    public Chemistry Chem;
    public void ChemBoom()
    {
        Chem.Chem_Anim.SetTrigger("Boom");
    }

    public void Well_Its_Time()
    {
        Destroy(gameObject);
    }

}
