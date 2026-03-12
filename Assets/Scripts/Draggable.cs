using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
public class Draggable : MonoBehaviour
{
    [HideInInspector] public Object2D objectData;
    [HideInInspector] public Object2DApiClient apiClient;

    private bool AanhetSlepen = false;
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    
    private Vector2 StartSlepen;
    private const float dragThreshold = 5f;
    private bool dragDecided = false;

   
    public bool isSelected = false;
    private static Draggable huidigGeselecteerd;

   
    private const float moveStep = 0.1f;
    private const float rotateStep = 15f;
    private bool needsPositionUpdate = false;

    
    private Color defaultColor;
    private static readonly Color selectedColor = new Color(0.5f, 0.8f, 1f);

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            defaultColor = spriteRenderer.color;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.transform == transform)
            {
                StartSlepen = Mouse.current.position.ReadValue();
                dragDecided = false;
                AanhetSlepen = false;
            }
        }

        if (Mouse.current.leftButton.isPressed && !dragDecided)
        {
            Vector2 currentScreenPos = Mouse.current.position.ReadValue();
            if (Vector2.Distance(currentScreenPos, StartSlepen) > dragThreshold)
            {
                dragDecided = true;
                AanhetSlepen = true;
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (!dragDecided)
            {
                // Als er een klik is zonder slepen word object geselecteerd
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null && hit.transform == transform)
                {
                    ToggleSelectie();
                }
            }
            //Als hij wel word gesleept word poitie opgeslagen in DB
            else if (AanhetSlepen)
            {
                if (objectData != null && apiClient != null)
                {
                    objectData.PositionX = transform.position.x;
                    objectData.PositionY = transform.position.y;
                    UpdateObject2D();
                }
            }

            AanhetSlepen = false;
            dragDecided = true;
        }

        if (AanhetSlepen)
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 positionInWorld = mainCamera.ScreenToWorldPoint(mouseScreenPos);
            positionInWorld.z = 0;
            transform.position = positionInWorld;
        }

        if (isSelected)
            HandleKeyboardInput();
    }

    private void ToggleSelectie()
    {
        if (isSelected)
        {
            Deselecteer();
        }
        else
        {
            if (huidigGeselecteerd != null && huidigGeselecteerd != this)
                huidigGeselecteerd.Deselecteer();

            isSelected = true;
            huidigGeselecteerd = this;
            if (spriteRenderer != null)
                spriteRenderer.color = selectedColor;
        }
    }

    public void Deselecteer()
    {
        isSelected = false;
        if (spriteRenderer != null)
            spriteRenderer.color = defaultColor;
        if (huidigGeselecteerd == this)
            huidigGeselecteerd = null;
    }

    private void HandleKeyboardInput()
    {
        var kb = Keyboard.current;

        //Veranderen van rotatie met R
        if (kb.rKey.wasPressedThisFrame)
        {
            float delta = kb.shiftKey.isPressed ? -rotateStep : rotateStep;
            transform.Rotate(0f, 0f, delta);
            objectData.RotationZ = transform.eulerAngles.z;
            UpdateObject2D();
        }

        //Veranderen van schaal met + en -
        if (kb.equalsKey.wasPressedThisFrame || kb.numpadPlusKey.wasPressedThisFrame)
        {
            transform.localScale *= 1.1f;
            objectData.ScaleX = transform.localScale.x;
            objectData.ScaleY = transform.localScale.y;
            UpdateObject2D();
        }
        if (kb.minusKey.wasPressedThisFrame || kb.numpadMinusKey.wasPressedThisFrame)
        {
            transform.localScale /= 1.1f;
            objectData.ScaleX = transform.localScale.x;
            objectData.ScaleY = transform.localScale.y;
            UpdateObject2D();
        }

        // Verwijderen met Backspace
        if (kb.backspaceKey.wasPressedThisFrame)
        {
            DeleteObject2D();
        }
    }

    public async void CreateObject2D()
    {
        IWebRequestReponse webRequestResponse = await apiClient.CreateObject2D(objectData);

        switch (webRequestResponse)
        {
            case WebRequestData<Object2D> dataResponse:
                objectData.GUID = dataResponse.Data.GUID;
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Create object2D error: " + errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    public async void UpdateObject2D()
    {
        IWebRequestReponse webRequestResponse = await apiClient.UpdateObject2D(objectData);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Update object2D success");
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Update object2D error: " + errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    public async void DeleteObject2D()
    {
        if (objectData == null || apiClient == null) return;

        IWebRequestReponse response = await apiClient.DeleteObject2D(objectData.GUID.ToString());

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Delete object2D success");
                WorldManager.Instance.VerwijderObject(gameObject);
                break;
            case WebRequestError errorResponse:
                Debug.Log("Delete object2D error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + response.GetType());
        }
    }
}
