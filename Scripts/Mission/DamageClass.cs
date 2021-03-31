using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageClass
{
    public int Damage;
    public Elements Element;

    public DamageClass(int damage, Elements element)
    {
        Damage = damage;
        Element = element;
    }
}
