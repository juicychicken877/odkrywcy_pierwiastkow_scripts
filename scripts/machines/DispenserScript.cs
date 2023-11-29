using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserScript : InteractableObjectScript
{
    [SerializeField] private Transform productStation;
    [SerializeField] private EquipableObjectsSO product;

    public override void Interaction()
    {
        if (AreProductStationsCleared())
        {
            CreateProducts();
            UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Products Dispensed!");
        }
        else
            UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Not all stations are prepared!");
    }

    private void CreateProducts()
    {
        Instantiate(product.GetPrefab(), productStation.transform.position, Quaternion.Euler(0, 0, 0), productStation.transform);
    }
    private bool AreProductStationsCleared()
    {
        if (productStation.childCount != 0)
            return false;

        return true;
    }
}
