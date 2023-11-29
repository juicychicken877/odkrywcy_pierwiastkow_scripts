using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectScript: MonoBehaviour
{
    protected PlayerScript player;

    private void Start()
    {
        player = PlayerScript.Instance;
    }

    public virtual void Interaction()
    {
        Debug.Log(transform.name + " Interaction");
    }
}
