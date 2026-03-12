using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class SelectWorld : MonoBehaviour
{
    public GameObject World;
    public GameObject InlogScherm;
    public GameObject SelectWorldScherm;
    public WorldManager WorldManager;
    public GameObject CreateWorld;


    public Environment2DApiClient environment2DApiClient;

    [Header("Wereld Knoppen")]
    //Knoppen voor het selecteren van werelden, text en zichtbaarheid word aangepast op basis van de werelden
    public GameObject Wereld1Knop;
    public GameObject Wereld2Knop;
    public GameObject Wereld3Knop;
    public GameObject Wereld4Knop;
    public GameObject Wereld5Knop;
    public GameObject CreateWorldButton;

    //Word aangeroepen zodra de scene word geladen en haalt dan de werelden op en past de knoppen aan 
    public async void OnEnable()
    {
        // De lossen knoppen worden in een array gezet zodat ze met een loop kunnen worden geupdate
        GameObject[] wereldKnoppen = new GameObject[] { Wereld1Knop, Wereld2Knop, Wereld3Knop, Wereld4Knop, Wereld5Knop };

        IWebRequestReponse webRequestResponse = await environment2DApiClient.ReadEnvironment2Ds();

        switch (webRequestResponse)
        {
            case WebRequestData<List<Environment2D>> dataResponse:
                List<Environment2D> environment2Ds = dataResponse.Data;
                Debug.Log($"Aantal werelden opgehaald: {environment2Ds.Count}");

                
                for (int i = 0; i < wereldKnoppen.Length; i++)
                {
                    GameObject Knop = wereldKnoppen[i];

                    if (i < environment2Ds.Count)
                    {
                        Environment2D geselecteerdeWereld = environment2Ds[i];

                        Knop.SetActive(true);

                        TMP_Text knopTekst = Knop.GetComponentInChildren<TMP_Text>();
                        if (knopTekst != null)
                        {
                            knopTekst.text = geselecteerdeWereld.Name; 
                        }

                        Button buttonComponent = Knop.GetComponent<Button>();
                        if (buttonComponent != null)
                        {
                            //Verwijdert alle methodes van de onclick van de buttons
                            buttonComponent.onClick.RemoveAllListeners();

                            
                            Environment2D wereldRef = geselecteerdeWereld;

                            //Voegt een nieuwe methode aan de buttons voor onclick zodat de juiste wereld word geladen
                            buttonComponent.onClick.AddListener(() => LaadWereld(wereldRef));
                        }
                    }
                    else
                    {
                        //Knop word verborgen als er geen wereld is voor de knop
                        Knop.SetActive(false);
                    }
                }
                break;

            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Read environment2Ds error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;

            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    //Methode voor het laden van de wereld
    private void LaadWereld(Environment2D wereld)
    {
        Debug.Log($"Wereld geselecteerd: {wereld.Name} met ID: {wereld.Id}");

        SelectWorldScherm.SetActive(false);
        World.SetActive(true);

        //Wereld object word doorgegeven aan de worldmanager zodat bounds en ID beschikbaar zijn
        if (WorldManager.Instance != null)
        {
            WorldManager.Instance.LaadWereldObjecten(wereld.Id, wereld);
        }
    }
    public void ToCreateWorld()
    {
        CreateWorld.SetActive(true);
        SelectWorldScherm.SetActive(false);
    }
   
}
