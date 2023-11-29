using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthesiserElementStationScript : InteractableObjectScript
{
    [SerializeField] private EquipableObjectsSO neededElement;
    [SerializeField] private Transform elementStationPosition;

    private bool haveElementOnStation = false;
    private GameObject instance;

    private void Update()
    {
        haveElementOnStation = elementStationPosition.childCount != 0;
    }

    public override void Interaction()
    {
        EquipableObjectsSO[] playerEquipment = player.GetPlayerEquipment();

        if (!haveElementOnStation)
        {
            for (int i = 0; i < player.GetPlayerEquipmentCapacity(); i++)
            {
                // if player have needed element in inventory
                if (playerEquipment[i] != null)
                {
                    if (playerEquipment[i].GetPrefabName() == neededElement.GetPrefabName())
                    {
                        // clear item from player eq and drop onto station
                        player.DeleteFromEQ(i, elementStationPosition.transform.position, elementStationPosition);

                        haveElementOnStation = true;
                        break;
                    }
                    // if not
                    else
                    {
                        // display UIManagerScript warning *Nie masz w ekwipunku {przedmiot}*

                        haveElementOnStation = false;
                    }
                }
            }

            if (!haveElementOnStation)
                UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("You dont have proper items for this reaction!");
        }
        // if there is element on station and player wants to interact - equip item
        else
        {
            player.EquipItem(neededElement, GetElementOnStation());
            haveElementOnStation = false;
        }
    }

    public void DestroyElement()
    {
        // destroy children of station
        EquipableObjectScript[] children = elementStationPosition.GetComponentsInChildren<EquipableObjectScript>();

        for (int i=0; i<children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }
    }

    // get set
    public bool HaveElementOnStation()
    {
        return haveElementOnStation;
    }

    public GameObject GetElementOnStation()
    {
        EquipableObjectScript obj = elementStationPosition.GetComponentInChildren<EquipableObjectScript>();

        return obj.gameObject;
    }
}
