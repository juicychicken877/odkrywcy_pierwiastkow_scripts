using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class EquipmentUIScript : MonoBehaviour
{
    [SerializeField] private GameObject equipmentUI;
    [SerializeField] private GameObject iconsObject;
    [SerializeField] private GameObject namesObject;
    [SerializeField] private GameObject buttonsObject;

    [SerializeField] private Color activeButtonColor;
    [SerializeField] private Color inActiveButtonColor;

    private PlayerScript player;
    private Transform[] iconsHolder;
    private Text[] textsHolder;
    private UnityEngine.UI.Button[] buttons;

    private InputSystemScript inputSystemScript;
    
    private int previousButtonIndex = -1;

    private bool[] equipmentUIContent = { false, false, false, false, false, false, false, false, false, false };



    private void Start()
    {
        inputSystemScript = InputSystemScript.Instance;
        player = PlayerScript.Instance;

        inputSystemScript.OnUIEquipmentOn += InputSystemScript_OnUIEquipmentOn;

        iconsHolder = iconsObject.GetComponentsInChildren<Transform>();
        textsHolder = namesObject.GetComponentsInChildren<Text>();
        buttons = buttonsObject.GetComponentsInChildren<UnityEngine.UI.Button>();
    }

    public void ButtonActive(int index)
    {
        // if clicked on button change color
        if (previousButtonIndex >= 0)
            buttons[previousButtonIndex].gameObject.GetComponent<UnityEngine.UI.Image>().color = inActiveButtonColor;

        buttons[index].gameObject.GetComponent<UnityEngine.UI.Image>().color = activeButtonColor;

        previousButtonIndex = index;
    }

    public void UpdateUI(EquipableObjectsSO[] equipment)
    {
        // change text and icon in eq 

        for (int i = 0; i < player.GetPlayerEquipmentCapacity(); i++)
        {
            if (equipment[i] != null && equipmentUIContent[i] == false)
            {
                CreateEQField(i, equipment[i]);
            }
        }
    }

    private void CreateEQField(int index, EquipableObjectsSO obj)
    {
        // create icon and text
        Instantiate(obj.equipmentIcon, iconsHolder[index + 1]);

        textsHolder[index].text = obj.GetPrefabName();

        equipmentUIContent[index] = true;
    }

    public void ClearEQField(int index)
    {
        // delete icon and text
        textsHolder[index].text = "";
        equipmentUIContent[index] = false;

        // destroy child objects - icon
        // 1 since there is parent on 0
        Destroy(iconsHolder[index + 1].GetComponentsInChildren<Transform>()[1].gameObject);
    }

    public void ChangeMenuActive()
    {
        if (equipmentUI.activeInHierarchy)
        {
            equipmentUI.SetActive(false);
        }
        else
        {
            equipmentUI.SetActive(true);
        }
    }

    private void InputSystemScript_OnUIEquipmentOn(object sender, System.EventArgs e)
    {
        // open and close eq

        if (UIManagerScript.Instance.GetPermissionToChangeMember(equipmentUI))
        {
            ChangeMenuActive();
        }
        else
        {
            UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Cannot open inventory because something is already opened!");
        }
    }

    // get set

    public int GetActiveButtonIndex()
    {
        return previousButtonIndex;
    }
}
