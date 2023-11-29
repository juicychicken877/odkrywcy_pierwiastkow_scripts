using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class ReactionMachineUIScript : MonoBehaviour
{
    [SerializeField] private GameObject reactionMachineUI;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject listItemPattern2_1;
    [SerializeField] private GameObject listItemPattern1_2;
    [SerializeField] private ScrollRect scrollRect;

    private List<GameObject> UIContent = new List<GameObject>();

    [SerializeField] private Color activeButtonColor;
    [SerializeField] private Color inActiveButtonColor;

    private int previousButtonIndex = -1;

    public void ChangeMenuActive()
    {
        // open and close menu
        if (UIManagerScript.Instance.GetPermissionToChangeMember(reactionMachineUI))
        {
            if (reactionMachineUI.activeInHierarchy)
            {
                reactionMachineUI.SetActive(false);
            }
            else
            {
                reactionMachineUI.SetActive(true);
                scrollRect.verticalNormalizedPosition = 1.0f;
            }
        }
        else
        {
            UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Cannot open menu because something is already opened!");
        }
    }

    public void ButtonActive(int index)
    {
        // if clicked on button change color
        if (previousButtonIndex >= 0)
        {
            UIContent[previousButtonIndex].GetComponent<ReactionMachineListItem>().SetButtonColor(inActiveButtonColor);
        }

        UIContent[index].GetComponent<ReactionMachineListItem>().SetButtonColor(activeButtonColor);

        previousButtonIndex = index;
    }

    public void CreateAndUpdateListItem(List<RecipeSO> recipes)
    {
        // create list items if theres not
        for (int i = UIContent.Count; i < recipes.Count; i++)
        {
            if (recipes[i] != null) 
            {
                // create depending on how much substrates and products
                switch ((recipes[i].substratesCount, recipes[i].productsCount))
                {
                    case (2, 1): 
                        GameObject obj = Instantiate(listItemPattern2_1);
                        obj.transform.SetParent(content.transform, false);

                        FillListItemContent(recipes[i], obj, i);
                    break;
                    case (1, 2):
                        GameObject obj2 = Instantiate(listItemPattern1_2);
                        obj2.transform.SetParent(content.transform, false);

                        FillListItemContent(recipes[i], obj2, i);
                        break;
                }
            }
        }
    }

    private void FillListItemContent(RecipeSO recipe, GameObject obj, int index)
    {
        // fill content
        ReactionMachineListItem script = obj.GetComponent<ReactionMachineListItem>();

        // set icons and text
        script.SetSubstratesUI(recipe.substrates, recipe.substratesCount);
        script.SetProductsUI(recipe.products, recipe.productsCount);

        script.SetButtonOnClick(index);

        // add list item to list
        UIContent.Add(obj);
    }

    public int GetActiveButtonIndex()
    {
        return previousButtonIndex;
    }
}
