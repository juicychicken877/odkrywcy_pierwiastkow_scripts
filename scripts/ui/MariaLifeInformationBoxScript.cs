using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MariaLifeInformationBoxScript : MonoBehaviour
{
    [SerializeField] private Text header;
    [SerializeField] private Text description;

    public void SetHeaderText(string text)
    {
        header.text = text;
    }
    public void SetDescriptionText(string text)
    {
        description.text = text;
    }
}
