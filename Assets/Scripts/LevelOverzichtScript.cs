using MySecureBackend.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelOverzichtScript : MonoBehaviour
{
    public TMP_Text BalooText;
    public TMP_Dropdown BehandelingSelect;
    public GameObject LevelOverzicht;
    public GameObject LoginScherm;

    public BehandelingApiClient behandelingApiClient;
    public KindApiClient kindApiClient;

    private string kindNaam;
    private List<Behandeling> behandelingen;
    // TODO: gameprogress toevoegen zodra Kind-model dit ondersteunt
    // private List<GameProgress> gameProgressList;

    private async void OnEnable()
    {
        string kindID = PlayerPrefs.GetString("kindID");

        // 1. Kind ophalen en naam opslaan
        IWebRequestReponse kindResponse = await kindApiClient.GetById(Guid.Parse(kindID));
        switch (kindResponse)
        {
            case WebRequestData<string> dataResponse:
                Kind fetchedKind = JsonConvert.DeserializeObject<Kind>(dataResponse.Data);
                kindNaam = fetchedKind.Naam;
                BalooText.text = kindNaam;
                Debug.Log("Get kind success: " + kindNaam);
                break;
            case WebRequestError errorResponse:
                Debug.Log("Get kind by id error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + kindResponse.GetType());
        }

        // 2. Alle behandelingen ophalen en opslaan
        IWebRequestReponse behandelingResponse = await behandelingApiClient.GetAll();
        switch (behandelingResponse)
        {
            case WebRequestData<string> dataResponse:
                behandelingen = JsonConvert.DeserializeObject<List<Behandeling>>(dataResponse.Data);
                Debug.Log("Behandelingen opgehaald: " + behandelingen.Count);
                // TODO: dropdown vullen
                break;
            case WebRequestError errorResponse:
                Debug.Log("Get all behandelingen error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + behandelingResponse.GetType());


            
        }
        BalooText.text = $"Welkom, {kindNaam}. Klaar om te leren?";
    }

    public void Logout()
    {
        LevelOverzicht.SetActive(false);
        LoginScherm.SetActive(true);
    }
}
