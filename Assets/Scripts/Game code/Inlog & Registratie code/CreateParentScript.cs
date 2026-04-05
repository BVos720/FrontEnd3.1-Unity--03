using MySecureBackend.WebApi.Models;
using System;
using TMPro;
using UnityEngine;

public class CreateParentScript : MonoBehaviour
{
    public OuderController ouderController;
    public GameObject OuderRegistratieScherm;
    public GameObject KindRegistratieScherm;
    public TMP_InputField OudernaamInput;
    public TMP_Text FeedbackText;

    public async void CreateOuder()
    {
        Ouder ouder = await ouderController.Create(OudernaamInput.text);
        if (ouder != null)
        {
            OuderRegistratieScherm.SetActive(false);
            KindRegistratieScherm.SetActive(true);
        }
        else
        {
            Debug.LogError("Failed to create Ouder.");
            FeedbackText.text = "Er is een fout opgetreden bij het aanmaken van de ouder. Probeer het opnieuw.";
        }
    }
}
