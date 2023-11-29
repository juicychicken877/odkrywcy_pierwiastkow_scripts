using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthesisMachineScript : InteractableObjectScript
{
    [SerializeField] protected SynthesiserElementStationScript[] elementStationsScript;
    [SerializeField] protected Transform[] productStations;
    [SerializeField] private EquipableObjectsSO[] products;

    public override void Interaction()
    {
        if (AreElementStationsFull())
        {
            if (AreProductStationsCleared())
                StartReaction();
            else
                UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Not all stations are prepared!");
        }
        else
            UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Not all stations are prepared!");
    }

    protected void StartReaction()
    {
        // animation?

        // create products
        CreateProducts();

        // destroy substrates
        DestroySubstrates();
    }

    protected void CreateProducts()
    {
        for (int i=0; i<productStations.Length; i++)
        {
            Instantiate(products[i].GetPrefab(), productStations[i].transform.position, Quaternion.Euler(0, 0, 0), productStations[i]);
        }
        UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Created Products!");
    }

    protected void DestroySubstrates()
    {
        for (int i = 0; i < elementStationsScript.Length; i++)
        {
            elementStationsScript[i].DestroyElement();
        }
    }

    protected bool AreProductStationsCleared()
    {
        for (int i = 0; i < productStations.Length; i++)
        {
            if (productStations[i].childCount != 0)
                return false;
        }

        return true;
    }

    protected bool AreElementStationsFull()
    {
        for (int i = 0; i < elementStationsScript.Length; i++)
        {
            if (!elementStationsScript[i].HaveElementOnStation())
                return false;
        }

        return true;
    }
}
