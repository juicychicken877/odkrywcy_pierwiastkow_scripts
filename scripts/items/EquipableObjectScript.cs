using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableObjectScript : InteractableObjectScript
{
    [SerializeField] public string type;
    [SerializeField] public string fullName;
    [SerializeField] private EquipableObjectsSO equipableObjectSO;
    public override void Interaction()
    {
        player.EquipItem(equipableObjectSO, gameObject);
    }

}
