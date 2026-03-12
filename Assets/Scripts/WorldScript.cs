using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("API Client")]
    public Object2DApiClient object2DApiClient;

    [Header("Schermen")]
    public GameObject SelectWorld;
    public GameObject World;
    public GameObject BackButton;
    public Environment2DApiClient environment2DApiClient;

    [Header("Instellingen")]
    //Registreert in een array welke prefabs er bestaan, de prefab index moet overeen komen met de ID in de DB
    public GameObject[] beschikbarePrefabs;

    //De huidige wereld die is geladen
    public string HuidigeWereldId { get; private set; }

    //World bounds
    public float MaxX { get; private set; }
    public float MaxY { get; private set; }

    //Lijst met geladen objecten, kunnen dus makkelijker gemanaged worden
    private List<GameObject> gespawndeObjecten = new List<GameObject>();


    //Haalt alle objecten op uit de DB voor deze wereld zodat deze geladen kunnen worden
    public async void LaadWereldObjecten(string environmentId, Environment2D wereld)
    {
        HuidigeWereldId = environmentId;
        MaxX = wereld.MaxLenght;
        MaxY = wereld.MaxHeight;
        World.SetActive(true);

        //Verwijderd voor de zekerheid alle objecten die zodat er geen oude objecten meer bestaan
        foreach (GameObject obj in gespawndeObjecten)
            Destroy(obj);
        gespawndeObjecten.Clear();

        //Roept de enviroment API op om de werelden te laden
        IWebRequestReponse response = await object2DApiClient.ReadObject2Ds(environmentId);

        switch (response)
        {
            case WebRequestData<List<Object2D>> dataResponse:
                List<Object2D> object2Ds = dataResponse.Data;
                Debug.Log($"Succes: {object2Ds.Count} objecten gevonden.");

                //Loop die de objecten laat in de wereld
                foreach (Object2D objData in object2Ds)
                {
                    PlaatsObjectInScene(objData);
                }
                break;

            case WebRequestError errorResponse:
                Debug.LogError("Fout bij ophalen objecten: " + errorResponse.ErrorMessage);
                break;
        }
    }

    //Zorgt ervoor dat de object data word ongezet naar objecten in unity
    private void PlaatsObjectInScene(Object2D data)
    {
        //Is de prefab ID geldig? 
        if (data.PrefabID < 0 || data.PrefabID >= beschikbarePrefabs.Length)
        {
            Debug.LogWarning($"PrefabID {data.PrefabID} is niet gevonden in de array.");
            return;
        }

        GameObject prefabOmTeSpawnen = beschikbarePrefabs[data.PrefabID];

        //Positie en rotatie waardes uit de DB worden omgezet naar de object waardes in unity
        Vector3 positie = new Vector3((float)data.PositionX, (float)data.PositionY, 0f);
        Quaternion rotatie = Quaternion.Euler(0f, 0f, (float)data.RotationZ);

        //Object word daadwerkelijk gespawnd en aan een lijst toegevoegd zodat deze makkelijk gemanaged kan worden
        GameObject nieuwObject = Instantiate(prefabOmTeSpawnen, positie, rotatie, World.transform);
        gespawndeObjecten.Add(nieuwObject);

        //De schaal uit de DB word omgezet naar de schaal in unity
        nieuwObject.transform.localScale = new Vector3((float)data.ScaleX, (float)data.ScaleY, 1f);

        //Stelt de juiste sorting layer in in de sprite renderer
        SpriteRenderer spriteRenderer = nieuwObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = data.SortingLayer;
        }

        Draggable draggable = nieuwObject.GetComponent<Draggable>();
        if (draggable != null)
        {
            draggable.objectData = data;
            draggable.apiClient = object2DApiClient;
        }
    }

    public void RegistreerObject(GameObject obj)
    {
        gespawndeObjecten.Add(obj);
    }

    public void VerwijderObject(GameObject obj)
    {
        gespawndeObjecten.Remove(obj);
        Destroy(obj);
    }

    //Terug naar het select world menu 
    public void backToSelectWorld()
    {
        foreach (GameObject obj in gespawndeObjecten)
            Destroy(obj);
        gespawndeObjecten.Clear();

        SelectWorld.SetActive(true);
        World.SetActive(false);
    }
    
    //Verwijderd de huidige wereld uit de DB met alle objecten ebrij
        public async void DeleteEnvironment2D()
    {
        IWebRequestReponse webRequestResponse = await environment2DApiClient.DeleteEnvironment(HuidigeWereldId);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Delete environment2D success");
                backToSelectWorld();
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Delete environment2D error: " + errorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }
}    
