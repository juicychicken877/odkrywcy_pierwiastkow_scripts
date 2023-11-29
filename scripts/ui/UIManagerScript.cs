using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject mainBackground;

    private GameObject openedUIMember;
    public static UIManagerScript _instance;

    public static UIManagerScript Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void ChangeBackgroundActive()
    {
        if (mainBackground.activeInHierarchy)
        {
            mainBackground.SetActive(false);
        }
        else
        {
            mainBackground.SetActive(true);
        }
    }


    public bool GetPermissionToChangeMember(GameObject obj)
    {
        // if something is opened
        if (openedUIMember != null)
        {
            // if opened member is caller (permission to close)
            if (openedUIMember.GetInstanceID() == obj.GetInstanceID())
            {
                openedUIMember = null;

                // unlock movement and camera
                PlayerScript.Instance.ChangeMovementLocked();
                PlayerCameraScript.Instance.ChangeCameraLocked();
                ChangeBackgroundActive();

                return true;
            }
            else
                return false;
        }
        // if not
        else
        {
            openedUIMember = obj;

            // lock movement and camera
            PlayerScript.Instance.ChangeMovementLocked();
            PlayerCameraScript.Instance.ChangeCameraLocked();
            ChangeBackgroundActive();

            return true;
        }
    }
}
