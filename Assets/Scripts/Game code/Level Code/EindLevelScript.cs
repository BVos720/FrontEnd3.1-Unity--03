using TMPro;
using UnityEngine;
using MySecureBackend.WebApi.Models;
using System.Linq;
using System.Collections.Generic;

public class EindLevelScript : MonoBehaviour
{
    public TMP_Text FellicitatieText;
    public GameProgressController gameProgressController;

    private async void OnEnable()
    {
        string kindNaam = PlayerPrefs.GetString("kindNaam", "");
        string behandelingIDStr = PlayerPrefs.GetString("behandelingID", "");
        System.Guid.TryParse(behandelingIDStr, out System.Guid behandelingID);

        int punten = 0;
        List<GameProgress> allProgress = await gameProgressController.GetAll();
        if (allProgress != null)
        {
            var record = allProgress.FirstOrDefault(g => g.BehandelingID == behandelingID);
            if (record != null)
                punten = record.Points;
        }

        FellicitatieText.text = $"Gefeliciteerd {kindNaam}!!! Je hebt alle levels voltooid en {punten} goudstukken verdient. Je bent nu klaar voor de echte behandeling!";
    }
}
