using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MariaLifeBookScript : InteractableObjectScript
{
    [SerializeField] private string header;
    [SerializeField] private string description;

    public override void Interaction()
    {
        UIManagerScript.Instance.GetComponent<JournalUIScript>().AddPieceOfLife(header, description);

        Destroy(gameObject);
    }
}
