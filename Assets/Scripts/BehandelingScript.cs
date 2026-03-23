using MySecureBackend.WebApi.Models;
using System;
using TMPro;
using UnityEngine;

public class BehandelingScript : MonoBehaviour
{
    public GameObject BehandelingScherm;
    public GameObject LevelSelect;
    public TMP_InputField ArtsNaamInput;
    public TMP_InputField BehandelingDatumInput;
    public TMP_Dropdown BehandelingType;
    public BehandelingApiClient behandelingApiClient;
    public Behandeling behandeling;

    public async void CreateBehandeling()
    {
        Behandeling behandeling = new Behandeling(BehandelingType.options[BehandelingType.value].text, DateTime.Parse(BehandelingDatumInput.text), ArtsNaamInput.text);
                IWebRequestReponse webRequestResponse = await behandelingApiClient.Create(behandeling);

                switch (webRequestResponse)
                {
                    case WebRequestData<string> dataResponse:
                       Debug.Log("Create behandeling success");
                        BehandelingScherm.SetActive(false);
                        LevelSelect.SetActive(true);
                break;
                    case WebRequestError errorResponse:
                        string errorMessage = errorResponse.ErrorMessage;
                        Debug.Log("Create behandeling error: " + errorMessage);
                        // TODO: Handle error scenario. Show the errormessage to the user.
                        break;
                    default:
                        throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
                }

    }

}
