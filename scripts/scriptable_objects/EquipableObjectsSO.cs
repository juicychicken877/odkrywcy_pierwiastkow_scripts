using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class EquipableObjectsSO : ScriptableObject
{
    public Transform prefab;
    public Image equipmentIcon;
    public bool isRadioactive;
    public string description;
    // get set

    public Transform GetPrefab()
    {
        return prefab;
    }

    public string GetPrefabName()
    {
        return prefab.GetComponent<EquipableObjectScript>().type;
    }

    public string GetPrefabFullName()
    {
        return prefab.GetComponent<EquipableObjectScript>().fullName;
    }
}
