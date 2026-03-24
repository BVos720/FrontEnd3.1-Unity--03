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
    public GameObject OverzichtScherm;
    public GameObject LoginScherm;
    public TMP_Dropdown KinderSelectDropdown;
    public Transform saveScrollContent;
    public GameObject SettingScherm;

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

    private void ClearSaveContent()
    {
        foreach (Transform child in saveScrollContent)
            Destroy(child.gameObject);
    }

    private async Awaitable OnKindChanged(int index)
    {
        Kind kind = kinderen[index];
        BalooText.text = $"Welkom, {kind.Naam}. Klaar om te leren?";
        ClearSaveContent();

        IWebRequestReponse behandelingResponse = await behandelingApiClient.GetAll();
        switch (behandelingResponse)
        {
            case WebRequestData<string> dataResponse:
                List<Behandeling> alleBehandelingen = JsonConvert.DeserializeObject<List<Behandeling>>(dataResponse.Data);
                behandelingen = alleBehandelingen.Where(b => b.KindID == kind.KindID).ToList();
                BehandelingSelect.ClearOptions();
                BehandelingSelect.onValueChanged.RemoveAllListeners();
                if (behandelingen.Count > 0)
                {
                    BehandelingSelect.AddOptions(behandelingen.Select(b => b.Type).ToList());
                    BehandelingSelect.onValueChanged.AddListener(async (i) => await OnBehandelingChanged(i));
                    Debug.Log("Behandelingen opgehaald voor kind: " + behandelingen.Count);
                    await OnBehandelingChanged(0);
                }
                else
                {
                    Debug.Log("Geen behandelingen gevonden voor dit kind.");
                }
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Get behandelingen error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + behandelingResponse.GetType());
        }
    }

    private async Awaitable OnBehandelingChanged(int index)
    {
        Behandeling behandeling = behandelingen[index];

        IWebRequestReponse gameProgressResponse = await gameProgressApiClient.GetAll();
        switch (gameProgressResponse)
        {
            case WebRequestData<string> dataResponse:
                List<GameProgress> alleProgresses = JsonConvert.DeserializeObject<List<GameProgress>>(dataResponse.Data);
                gameProgresses = alleProgresses.Where(g => g.BehandelingID == behandeling.BehandelingID).ToList();
                ClearSaveContent();
                Debug.Log("Saves opgehaald voor behandeling: " + gameProgresses.Count);
                foreach (GameProgress gp in gameProgresses)
                {
                    GameObject entry = new GameObject("SaveEntry");
                    entry.transform.SetParent(saveScrollContent, false);
                    TextMeshProUGUI saveText = entry.AddComponent<TextMeshProUGUI>();
                    saveText.text = $"Punten: {gp.Points}\nVoortgang: {(gp.LevelProgress * 100f):F0}%";
                }
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Get game progress error: " + errorResponse.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + gameProgressResponse.GetType());
        }
    }

    public void Logout()
    {
        OverzichtScherm.SetActive(false);
        LoginScherm.SetActive(true);
    }

    public void OpenSettings()
    {
        SettingScherm.SetActive(true);
        OverzichtScherm.SetActive(false);


    }
}
