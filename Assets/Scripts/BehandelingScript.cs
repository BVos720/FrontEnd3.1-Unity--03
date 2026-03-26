using Assets.Scripts;
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
    public BehandelingController behandelingController;
    public SettingsController settingsController;

    public async void CreateBehandeling()
    {
        string type = BehandelingType.options[BehandelingType.value].text;
        DateTime datum = DateTime.Parse(BehandelingDatumInput.text);
        Behandeling behandeling = await behandelingController.Create(type, datum, ArtsNaamInput.text);
        if (behandeling != null)
        {
            BehandelingScherm.SetActive(false);
            LevelSelect.SetActive(true);
        }
    }

    public async void CreateSettings()
    {
        await settingsController.Create(new SettingsData(0, 0));
    }
}
