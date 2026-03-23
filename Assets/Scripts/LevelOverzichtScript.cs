using MySecureBackend.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelOverzichtScript : MonoBehaviour
{
    public TMP_Text BalooText;
    public TMP_Dropdown BehandelingSelect;
    public GameObject LevelOverzicht;
    public GameObject LoginScherm;
    public TMP_Dropdown KinderSelectDropdown;

    public BehandelingApiClient behandelingApiClient;
    public KindApiClient kindApiClient;
    public GameProgressApiClient gameProgressApiClient;

    private List<Kind> kinderen;
    private List<Behandeling> behandelingen;
    private List<GameProgress> gameProgresses;

    private async void OnEnable()
    {
        KinderSelectDropdown.onValueChanged.RemoveAllListeners();

        IWebRequestReponse kindResponse = await kindApiClient.GetAll();
        switch (kindResponse)
        {
            case WebRequestData<string> dataResponse:
                kinderen = JsonConvert.DeserializeObject<List<Kind>>(dataResponse.Data);
                KinderSelectDropdown.ClearOptions();
                KinderSelectDropdown.AddOptions(kinderen.Select(k => k.Naam).ToList());
                KinderSelectDropdown.onValueChanged.AddListener(async (index) => await OnKindChanged(index));
                Debug.Log("Kinderen opgehaald: " + kinderen.Count);
                if (kinderen.Count > 0)
                    await OnKindChanged(0);
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Get all kinderen error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + kindResponse.GetType());
        }
    }

    private async Awaitable OnKindChanged(int index)
    {
        Kind kind = kinderen[index];
        BalooText.text = $"Welkom, {kind.Naam}. Klaar om te leren?";

        IWebRequestReponse behandelingResponse = await behandelingApiClient.GetById(kind.BehandelingID);
        switch (behandelingResponse)
        {
            case WebRequestData<string> dataResponse:
                Behandeling behandeling = JsonConvert.DeserializeObject<Behandeling>(dataResponse.Data);
                behandelingen = new List<Behandeling> { behandeling };
                BehandelingSelect.ClearOptions();
                BehandelingSelect.AddOptions(behandelingen.Select(b => b.Type).ToList());
                BehandelingSelect.onValueChanged.RemoveAllListeners();
                BehandelingSelect.onValueChanged.AddListener(async (i) => await OnBehandelingChanged(i));
                Debug.Log("Behandeling opgehaald: " + behandeling.Type);
                await OnBehandelingChanged(0);
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Get behandeling by id error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + behandelingResponse.GetType());
        }
    }

    private async Awaitable OnBehandelingChanged(int index)
    {
        Behandeling behandeling = behandelingen[index];

        IWebRequestReponse gameProgressResponse = await gameProgressApiClient.GetById(behandeling.GameProgressID);
        switch (gameProgressResponse)
        {
            case WebRequestData<string> dataResponse:
                GameProgress gameProgress = JsonConvert.DeserializeObject<GameProgress>(dataResponse.Data);
                gameProgresses = new List<GameProgress> { gameProgress };
                Debug.Log("GameProgress opgehaald: LevelProgress=" + gameProgress.LevelProgress + ", Points=" + gameProgress.Points);
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Get game progress by id error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + gameProgressResponse.GetType());
        }
    }

    public void Logout()
    {
        LevelOverzicht.SetActive(false);
        LoginScherm.SetActive(true);
    }
}
