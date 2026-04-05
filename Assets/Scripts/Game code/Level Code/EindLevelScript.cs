using TMPro;
using UnityEngine;
using MySecureBackend.WebApi.Models;
using System.Collections.Generic;
using System.Linq;

public class EindLevelScript : MonoBehaviour
{
    public TMP_Text FellicitatieText;
    public GameProgressController gameProgressController;
    public GameObject Eindlevel;
    public GameObject levelMenu;

    private const int LEVEL_NUMBER = 6; 

    private async void OnEnable()
    {
        string kindNaam = PlayerPrefs.GetString("kindNaam", "");
        int punten = 0;

       
        string behandelingIDStr = PlayerPrefs.GetString("behandelingID", "");
        System.Guid.TryParse(behandelingIDStr, out System.Guid behandelingID);

        
        List<GameProgress> allProgress = await gameProgressController.GetAll();
        if (allProgress != null)
        {
            
            var record = allProgress.FirstOrDefault(g => g.BehandelingID == behandelingID);
            if (record != null)
            {
                punten = record.Points;
            }
        }

     
        FellicitatieText.text = $"Gefeliciteerd {kindNaam}!!! Je hebt alle levels voltooid en {punten} goudstukken verdiend. Je bent nu klaar voor de echte behandeling!";

        
        var gameProgress = await gameProgressController.GetOrCreate(0f, 0, LEVEL_NUMBER);
        if (gameProgress != null && gameProgress.LevelProgress < LEVEL_NUMBER)
        {
            gameProgress.LevelProgress = LEVEL_NUMBER;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }
    }

    public void TerugNaarMenu()
    {
        if (Eindlevel != null) Eindlevel.SetActive(false);
        if (levelMenu != null) levelMenu.SetActive(true);
    }
}