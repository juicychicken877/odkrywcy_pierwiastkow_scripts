using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    public static PlayerCameraScript _instance;

    public static PlayerCameraScript Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] private Transform playerHead;

    [SerializeField] private float sensitivityMultiplier = 1;
  
    private InputSystemScript inputSystemScript;

    private float xRotation;
    private float yRotation;
    private float sensitivityX = 5;
    private float sensitivityY = 5;
    private bool cameraLocked = false;

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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputSystemScript = InputSystemScript.Instance;
    }

    public void ChangeCameraLocked()
    {
        if (cameraLocked)
        {
            cameraLocked = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            cameraLocked = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        HandleRotationAndInput();
        FollowPlayerHead();
    }

    private void HandleRotationAndInput()
    {
        if (!cameraLocked)
        {
            Vector2 inputVector = inputSystemScript.GetMouseVector() * Time.deltaTime * sensitivityMultiplier;

            yRotation += inputVector.x * sensitivityX;
            xRotation -= inputVector.y * sensitivityY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // rotate camera and orientation object
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerHead.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
    private void FollowPlayerHead()
    {
        transform.position = playerHead.transform.position;
    }
}
