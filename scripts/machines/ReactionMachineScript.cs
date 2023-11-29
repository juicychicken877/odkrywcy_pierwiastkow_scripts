using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ReactionMachineScript : InteractableObjectScript
{
    [SerializeField] private Transform productStation;
    [SerializeField] private Transform elementStation;

    private ReactionMachineUIScript reactionMachineUIScript;
    private EquipableObjectsSO[] playerEquipment;

    private List<RecipeSO> playerRecipes = new List<RecipeSO>();

    private void Start()
    {
        reactionMachineUIScript = UIManagerScript.Instance.GetComponent<ReactionMachineUIScript>();

        player = PlayerScript.Instance;
    }

    public override void Interaction()
    {
        // get player recipes
        UpdateRecipes();

        // update ui
        reactionMachineUIScript.CreateAndUpdateListItem(playerRecipes);

        // open or close ui
        reactionMachineUIScript.ChangeMenuActive();
    }

    public void StartReaction()
    {
        playerEquipment = player.GetPlayerEquipment();

        if (reactionMachineUIScript.GetActiveButtonIndex() != -1)
        {
            RecipeSO recipe = playerRecipes[reactionMachineUIScript.GetActiveButtonIndex()];

            if (AreProductStationsCleared())
            {
                // get arguments
                EquipableObjectsSO[] neededItems = new EquipableObjectsSO[recipe.substratesCount];

                for (int i = 0; i < recipe.substratesCount; i++)
                {
                    neededItems[i] = recipe.substrates[i];
                }

                // check if player has needed items - if something null - player doesnt have
                EquipableObjectsSO[] playerItems = GetPlayerItemsForReaction(playerEquipment, neededItems);

                if (playerItems.Contains<EquipableObjectsSO>(null))
                {
                    UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("You dont have proper items for this reaction!");
                }
                else
                {
                    DropSubstrates(recipe, playerItems);

                    CreateProducts(recipe);

                    UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Created Products!");

                    DestroySubstrates();

                    // close menu
                    reactionMachineUIScript.ChangeMenuActive();
                }  
            }
            else
            {
                UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Not all stations are prepared!");
            }
        }
    }

    private EquipableObjectsSO[] GetPlayerItemsForReaction(EquipableObjectsSO[] playerEquipment, EquipableObjectsSO[] neededItems)
    {
        int count = 0;
        EquipableObjectsSO[] items = new EquipableObjectsSO[neededItems.Length];

        for (int i=0; i<player.GetPlayerEquipmentCapacity(); i++)
        {
            if (playerEquipment[i] != null)
            {
                for (int j=0;j<neededItems.Length; j++)
                {
                    if (playerEquipment[i].GetPrefabName() == neededItems[j].GetPrefabName() && items[j] == null)
                    {
                        items[j] = playerEquipment[i];
                        count++;
                    }
                }
            }
            // if found all
            if (count == neededItems.Length)
                return items;

        }

        return items;
    }

    private void DestroySubstrates()
    {
        EquipableObjectScript[] items = elementStation.GetComponentsInChildren<EquipableObjectScript>();

        for (int i=0; i<items.Length; i++)
        {
            Destroy(items[i].gameObject);
        }
    }

    private void DropSubstrates(RecipeSO recipe, EquipableObjectsSO[] playerItems)
    {
        for (int i = 0; i < recipe.substratesCount; i++)
        {
            player.DeleteFromEQ(player.GetFirstOccurenceInInventory(playerItems[i]), elementStation.transform.position, elementStation.transform);
        }
    }

    private void CreateProducts(RecipeSO recipe)
    {
        for (int i=0; i<recipe.productsCount; i++)
        {
            Instantiate(recipe.products[i].GetPrefab(), productStation.transform.position, Quaternion.Euler(0, 0, 0), productStation.transform);
        }
    }

    private bool AreProductStationsCleared()
    {
        if (productStation.childCount != 0)
            return false;
        return true;
    }

    public void UpdateRecipes()
    {
        playerRecipes = PlayerScript.Instance.GetPlayerRecipes();
    }


}
