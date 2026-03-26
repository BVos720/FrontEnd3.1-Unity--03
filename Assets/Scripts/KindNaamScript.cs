using UnityEngine;
using TMPro;
using MySecureBackend.WebApi.Models;
using System;

public class KindNaamScript : MonoBehaviour
{
    public KindApiClient kindApiClient;

    async void Start()
    {
        // Haal de opgeslagen KindID op
        string kindIDString = PlayerPrefs.GetString("kindID", "");

        if (string.IsNullOrEmpty(kindIDString))
        {
            Debug.LogError("Geen kindID gevonden in PlayerPrefs!");
            return;
        }

        if (!Guid.TryParse(kindIDString, out Guid kindID))
        {
            Debug.LogError("KindID is niet geldig!");
            return;
        }

        // Haal de kind data op van de API
        IWebRequestReponse webRequestResponse = await kindApiClient.GetById(kindID);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                // Parse de Kind data
                Kind kind = JsonUtility.FromJson<Kind>(dataResponse.Data);

                // Zoek alle TextMeshProUGUI elementen en vervang placeholders
                TextMeshProUGUI[] allTextElements = FindObjectsOfType<TextMeshProUGUI>();
                foreach (TextMeshProUGUI textElement in allTextElements)
                {
                    if (textElement.text.Contains("KindNaamPlaceholder"))
                    {
                        textElement.text = textElement.text.Replace("KindNaamPlaceholder", kind.Naam);
                        Debug.Log("Placeholder vervangen in: " + textElement.gameObject.name);
                    }
                }

                Debug.Log("Kind naam geladen: " + kind.Naam);
                break;

            case WebRequestError errorResponse:
                Debug.LogError("Fout bij ophalen kind data: " + errorResponse.ErrorMessage);
                break;

            default:
                Debug.LogError("Onbekende response type");
                break;
        }
    }
}
