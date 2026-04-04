using UnityEngine;
using MySecureBackend.WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts;

public class LevelLoader : MonoBehaviour
{
    [Header("Scenes")]
    public GameObject LevelOverzicht;
    public GameObject Overzicht;
    public GameObject Level1;
    public GameObject Level2;
    public GameObject Level3;
    public GameObject Level4;
    public GameObject LevelChest;
    public GameObject SettingsMenu;

    [Header("Controllers")]
    public GameProgressController gameProgressController;

    [Header("Completion Indicators")]
    public GameObject level1Indicator;
    public GameObject level2Indicator;
    public GameObject level3Indicator;
    public GameObject level4Indicator;


    [Header("Error Display")]
    public TextMeshProUGUI errorMessageText;

    private void Start()
    {
        UpdateCompletionIndicators();
    }

    private void OnEnable()
    {
        UpdateCompletionIndicators();
    }

    public void RefreshCompletionIndicators()
    {
        UpdateCompletionIndicators();
    }

    private void DisplayErrorMessage(string message)
    {
        if (errorMessageText != null)
            errorMessageText.text = message;
        Debug.LogWarning(message);
    }

    public void LoadLevel1()
    {
        DeactivateAllLevels();
        if (Level1 != null) Level1.SetActive(true);
    }

    public async void LoadLevel2()
    {
        // Check if Level 1 is complete
        if (!await IsLevelComplete(1))
        {
            DisplayErrorMessage("Je moet eerst level 1 voltooien voordat je verder kan");
            return;
        }

        DeactivateAllLevels();
        if (Level2 != null) Level2.SetActive(true);
    }

    public async void LoadLevel3()
    {
        // Check if Level 2 is complete
        if (!await IsLevelComplete(2))
        {
            DisplayErrorMessage("LJe moet eerst level 2 voltooien voordat je verder kan");
            return;
        }

        DeactivateAllLevels();
        if (Level3 != null) Level3.SetActive(true);
    }

    public async void LoadLevel4()
    {
        // Check if Level 3 is complete
        if (!await IsLevelComplete(3))
        {
            DisplayErrorMessage("Je moet eerst level 3 voltooien voordat je verder kan");
            return;
        }

        DeactivateAllLevels();
        if (Level4 != null) Level4.SetActive(true);
    }

    public void GoBackToOverzicht()
    {
        if (LevelOverzicht != null) LevelOverzicht.SetActive(false);
        ActivateOverzichtMenu();
    }

    private void ActivateOverzichtMenu()
    {
        if (LevelOverzicht != null) LevelOverzicht.SetActive(false);
        if (Level1 != null) Level1.SetActive(false);
        if (Level2 != null) Level2.SetActive(false);
        if (Level3 != null) Level3.SetActive(false);
        if (Level4 != null) Level4.SetActive(false);
        if (LevelChest != null) LevelChest.SetActive(false);
        Overzicht.SetActive(true);
    }

    public async void LoadLevelChest()
    {
        // Controleer of alle vorige levels zijn voltooid
        if (gameProgressController == null)
        {
            DisplayErrorMessage("GameProgressController is niet toegewezen!");
            return;
        }

        List<GameProgress> allProgress = await gameProgressController.GetAll();
        if (allProgress == null || allProgress.Count == 0)
        {
            DisplayErrorMessage("Geen spelvoortgang gevonden. Je moet eerst alle levels voltooien.");
            return;
        }

        string behandelingIDStr = PlayerPrefs.GetString("behandelingID", "");
        System.Guid.TryParse(behandelingIDStr, out System.Guid behandelingID);

        // Controleer of Level 1, 2, 3 en 4 allemaal zijn voltooid (LevelProgress = 1) voor deze behandeling
        // Check specific levels using the Points field which stores the level number
        bool level1Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 1);
        bool level2Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 2);
        bool level3Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 3);
        bool level4Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 4);

        if (!level1Complete || !level2Complete || !level3Complete || !level4Complete)
        {
            DisplayErrorMessage($"Je moet eerst alle 4 levels voltooien. Voltooid: L1={level1Complete}, L2={level2Complete}, L3={level3Complete}, L4={level4Complete}");
            return;
        }

        // Alle levels voltooid - laad de kist
        DeactivateAllLevels();
        if (LevelChest != null) 
        {
            LevelChest.SetActive(true);
        }
        else
        {
            DisplayErrorMessage("LevelChest GameObject is niet toegewezen!");
        }
    }

    private async void UpdateCompletionIndicators()
    {
        if (gameProgressController == null)
        {
            Debug.LogWarning("[LevelLoader] gameProgressController is niet toegewezen");
            return;
        }

        List<GameProgress> allProgress = await gameProgressController.GetAll();
        if (allProgress == null)
        {
            Debug.LogWarning("[LevelLoader] allProgress is null");
            return;
        }

        string behandelingIDStr = PlayerPrefs.GetString("behandelingID", "");
        System.Guid.TryParse(behandelingIDStr, out System.Guid behandelingID);

        // Check if each specific level is completed (Points field stores the level number)
        bool level1Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 1);
        bool level2Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 2);
        bool level3Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 3);
        bool level4Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 4);

        // Update indicators - show checkmark GameObject for completed levels
        if (level1Indicator != null)
            level1Indicator.SetActive(level1Complete);
        else
            Debug.LogWarning("[LevelLoader] level1Indicator is niet toegewezen!");

        if (level2Indicator != null)
            level2Indicator.SetActive(level2Complete);
        else
            Debug.LogWarning("[LevelLoader] level2Indicator is niet toegewezen!");

        if (level3Indicator != null)
            level3Indicator.SetActive(level3Complete);
        else
            Debug.LogWarning("[LevelLoader] level3Indicator is niet toegewezen!");

        if (level4Indicator != null)
            level4Indicator.SetActive(level4Complete);
        else
            Debug.LogWarning("[LevelLoader] level4Indicator is niet toegewezen!");
    }

    private void DeactivateAllLevels()
    {
        if (LevelOverzicht != null) LevelOverzicht.SetActive(false);
        if (Level1 != null) Level1.SetActive(false);
        if (Level2 != null) Level2.SetActive(false);
        if (Level3 != null) Level3.SetActive(false);
        if (Level4 != null) Level4.SetActive(false);
        if (LevelChest != null) LevelChest.SetActive(false);
    }

    private System.Guid GetBehandelingID()
    {
        System.Guid.TryParse(PlayerPrefs.GetString("behandelingID", ""), out System.Guid id);
        return id;
    }

    private async System.Threading.Tasks.Task<bool> IsLevelComplete(int levelNumber)
    {
        if (gameProgressController == null)
            return false;

        var progress = await gameProgressController.GetAll();
        if (progress?.Count == 0)
            return false;

        var id = GetBehandelingID();
        return progress.Any(g => g.BehandelingID == id && g.LevelProgress >= 1f && g.Points == levelNumber);
    }

    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
        var settings = SettingsMenu.GetComponentInChildren<Assets.Scripts.Settings>();
        if (settings != null) settings.LoadedFromScene = "LevelOverZicht";
        LevelOverzicht.SetActive(false);

    }
}
