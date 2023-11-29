using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageUIScript : MonoBehaviour
{
    [SerializeField] private Text message;

    public void Message(string text)
    {
        message.text = text;
    }
}
