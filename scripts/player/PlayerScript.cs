using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float walkMovementSpeed = 5f;
    [SerializeField] private float runMovementSpeed = 7f;
    [SerializeField] private LayerMask interactableObjectsLayer;
    [SerializeField] private Transform equipableObjects;
    [SerializeField] private Transform dropTarget;

    public static PlayerScript _instance;

    public static PlayerScript Instance
    {
        get
        {
            return _instance;
        }
    }

    private EquipableObjectsSO[] equipment = {null, null, null, null, null, null, null, null, null, null};
    private List<RecipeSO> myRecipes = new List<RecipeSO>();

    private UIManagerScript UIManagerScript;
    private EquipmentUIScript equipmentUIScript;
    private CharacterController myCharacterController;
    private InputSystemScript inputSystemScript;
    private InteractableObjectScript selectedInteractableObject;
    private Transform cameraTransform;

    private float gravity = 9.81f;
    private int equipmentCapacity = 10;
    private int equipmentCount = 0;
    private float currentMovementSpeed = 5f;
    private bool isMovementLocked = false;

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
        inputSystemScript = InputSystemScript.Instance;
        UIManagerScript = UIManagerScript.Instance;
        equipmentUIScript = UIManagerScript.GetComponent<EquipmentUIScript>();

        cameraTransform = Camera.main.transform;
        cameraTransform.rotation.Normalize();

        myCharacterController = GetComponent<CharacterController>();

        inputSystemScript.OnInteractionAction += InputSystemScript_OnInteractionAction;
    }

    public void ChangeMovementLocked()
    {
        if (isMovementLocked)
        {
            currentMovementSpeed = walkMovementSpeed;
        }
        else
        {
            currentMovementSpeed = 0;
        }

        isMovementLocked = !isMovementLocked;
    }

    private void InputSystemScript_OnInteractionAction(object sender, EventArgs e)
    {
        if (selectedInteractableObject != null)
        {
            selectedInteractableObject.Interaction();
        }
    }

    private void Update()
    {
        SwitchMovementSpeed();
        HandleMovement();
        HandleGravity();
        HandleInteraction();
    }
    private void HandleInteraction()
    {
        float interactionDistance = 3f;

        //cast a interaction ray where the camera points

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit raycastHit, interactionDistance, interactableObjectsLayer))
        {
            if (raycastHit.transform.TryGetComponent(out InteractableObjectScript interactableObject))
            {
                selectedInteractableObject = interactableObject;
            }
            else
            {
                selectedInteractableObject = null;
            }
        }
        else
        {
            selectedInteractableObject = null;
        }
    }
    private void HandleGravity()
    {
        if (myCharacterController.isGrounded)
        {

        }
        else
        {
            float velocity = gravity * Time.deltaTime;

            myCharacterController.Move(Vector3.down * velocity);
        }
    }

    private void HandleMovement()
    {
        float movementDistance = currentMovementSpeed * Time.deltaTime;

        Vector2 inputVector = inputSystemScript.GetNormalizedMovementVector();
        Vector3 movementDirection = cameraTransform.right * inputVector.x + cameraTransform.forward * inputVector.y;

        movementDirection.y = 0f;
        movementDirection = movementDirection.normalized;

        // move
        myCharacterController.Move(movementDirection * movementDistance);
    }

    public void EquipItem(EquipableObjectsSO obj, GameObject instance)
    {
        if (equipmentCount < equipmentCapacity)
        {
            // find first place to store
            for (int i=0; i<equipmentCapacity; i++)
            {
                if (equipment[i] == null)
                {
                    // change values in eq array
                    equipment[i] = obj;
                    break;
                }
            }

            equipmentCount += 1;

            // update ui
            equipmentUIScript.UpdateUI(equipment);

            // destroy instance of the object in scene
            Destroy(instance);
        }
        else
        {
            UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Inventory is full");
        }
    }

    public void DeleteFromEQ(int index, Vector3 dropPosition, Transform parent)
    {
        // if index == -1 then theres no active button yet

        if (index != -1 && equipment[index] != null)
        {
            equipmentUIScript.ClearEQField(index);

            // drop on scene
            Instantiate(equipment[index].prefab, dropPosition, new Quaternion(0, 0, 0, 0), parent);

            equipment[index] = null;
            equipmentCount -= 1;
        }
    }

    public int GetFirstOccurenceInInventory(EquipableObjectsSO obj)
    {
        for (int i=0; i<equipmentCapacity; i++)
        {
            if (equipment[i] != null)
                if (obj.GetPrefabName() == equipment[i].GetPrefabName())
                    return i;
        }

        return 0;
    }

    public void LearnRecipes(RecipeSO[] recipesToLearn)
    {
        for (int i=0; i<recipesToLearn.Length; i++)
        {
            myRecipes.Add(recipesToLearn[i]);
        }
        UIManagerScript.Instance.GetComponent<MessageUIScript>().Message("Learned Recipes!");
    }

    private void SwitchMovementSpeed()
    {
        if (inputSystemScript.WasShiftPressed())
        {
            currentMovementSpeed = runMovementSpeed;
        }
        if (inputSystemScript.WasShiftRealeased())
        {
            currentMovementSpeed = walkMovementSpeed;
        }
    }

    // get set
    public EquipableObjectsSO[] GetPlayerEquipment()
    {
        return equipment;
    }

    public int GetPlayerEquipmentCapacity()
    {
        return equipmentCapacity;
    }

    public List<RecipeSO> GetPlayerRecipes()
    {
        return myRecipes;
    }

    public Vector3 GetDropItemTargetPosition()
    {
        return dropTarget.transform.position;
    }

    public Transform GetEquipableObjectsObject()
    {
        return equipableObjects;
    }
}
