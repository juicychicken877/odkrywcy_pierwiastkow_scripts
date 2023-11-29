using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropButtonScript : MonoBehaviour
{
    private PlayerScript player;
    private UIManagerScript UIManagerScript;

    private void Start()
    {
        player = PlayerScript.Instance;
        UIManagerScript = UIManagerScript.Instance;
        
        // call function when button is clicked
        gameObject.GetComponent<Button>().onClick.AddListener(CallDeleteFromEQFunction);
    }

    private void CallDeleteFromEQFunction()
    {
        player.DeleteFromEQ(UIManagerScript.GetComponent<EquipmentUIScript>().GetActiveButtonIndex(), player.GetDropItemTargetPosition(), player.GetEquipableObjectsObject());
    }

}
