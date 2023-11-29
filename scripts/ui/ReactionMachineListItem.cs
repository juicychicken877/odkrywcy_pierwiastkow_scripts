using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReactionMachineListItem : MonoBehaviour
{
    [SerializeField] private GameObject[] substrateIconHolders;
    [SerializeField] private Text[] substrateTextHolders;
    [SerializeField] private GameObject[] productIconHolders;
    [SerializeField] private Text[] productTextHolders;

    [SerializeField] private Button selectButton;
    private ReactionMachineUIScript reactionMachineUIScript;

    private int listIndex = 0;

    private void Start()
    {
        reactionMachineUIScript = UIManagerScript.Instance.GetComponent<ReactionMachineUIScript>();
    }

    // set
    public void SetSubstratesUI(EquipableObjectsSO[] substrates, int substratesCount)
    {
        for (int i=0; i<substratesCount; i++)
        {
            // create icon
            Instantiate(substrates[i].equipmentIcon, substrateIconHolders[i].transform.position, Quaternion.Euler(0, 0, 0), substrateIconHolders[i].transform);

            // change text;
            substrateTextHolders[i].text = substrates[i].GetPrefabName(); 
        }
    }

    public void SetProductsUI(EquipableObjectsSO[] products, int productsCount)
    {
        for (int i = 0; i < productsCount; i++)
        {
            // create icon
            Instantiate(products[i].equipmentIcon, productIconHolders[i].transform.position, Quaternion.Euler(0, 0, 0), productIconHolders[i].transform);

            // change text;
            productTextHolders[i].text = products[i].GetPrefabName();
        }
    }

    private void CallButtonActiveFunction()
    {
        reactionMachineUIScript.ButtonActive(listIndex);
    }

    public void SetButtonOnClick(int index)
    {
        listIndex = index;

        selectButton.onClick.AddListener(CallButtonActiveFunction);
    }

    public void SetButtonColor(Color c)
    {
        selectButton.image.color = c;
    }



}
