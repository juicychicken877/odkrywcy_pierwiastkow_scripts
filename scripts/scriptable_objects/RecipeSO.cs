using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public EquipableObjectsSO[] substrates;
    public EquipableObjectsSO[] products;
    public int substratesCount = 0;
    public int productsCount = 0;
}
