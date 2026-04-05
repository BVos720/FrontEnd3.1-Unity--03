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
    public GameObject Level5;
    public GameObject Level6;
    public GameObject LevelChest;
    public GameObject SettingsMenu;

    [Header("Sounds")]
    public GameObject GameTheme;

    [Header("Controllers")]
    public GameProgressController gameProgressController;

    [Header("Completion Indicators")]
    public GameObject level1Indicator;
    public GameObject level2Indicator;
    public GameObject level3Indicator;
    public GameObject level4Indicator;
    public GameObject level5Indicator;
    public GameObject level6Indicator;


    [Header("Punten")]
    public TextMeshProUGUI puntenText;

    [Header("Error Display")]
    public TextMeshProUGUI errorMessageText;

    private void Start()
    {
        UpdateCompletionIndicators();
    }

    private void OnEnable()
    {
        GameTheme.SetActive(true);
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
            DisplayErrorMessage("Je moet eerst level 2 voltooien voordat je verder kan");
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

    public async void LoadLevel5()
    {
        // Check if Level 4 is complete
        if (!await IsLevelComplete(4))
        {
            DisplayErrorMessage("Je moet eerst level 4 voltooien voordat je verder kan");
            return;
        }

        DeactivateAllLevels();
        if (Level5 != null) Level5.SetActive(true);
    }

    public async void LoadLevel6()
    {
        // Check if Level 5 is complete
        if (!await IsLevelComplete(5))
        {
            DisplayErrorMessage("Je moet eerst level 5 voltooien voordat je verder kan");
            return;
        }

        DeactivateAllLevels();
        if (Level6 != null) Level6.SetActive(true);
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
        if (Level5 != null) Level5.SetActive(false);
        if (Level6 != null) Level6.SetActive(false);
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

        bool allComplete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 6);

        if (!allComplete)
        {
            DisplayErrorMessage("Je moet eerst alle 6 levels voltooien.");
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

        gameProgressController.ClearGameProgressCache();
        List<GameProgress> allProgress = await gameProgressController.GetAll();

        string behandelingIDStr = PlayerPrefs.GetString("behandelingID", "");
        System.Guid.TryParse(behandelingIDStr, out System.Guid behandelingID);

        Debug.Log($"[LevelLoader] behandelingID={behandelingID}, allProgress count={allProgress?.Count ?? -1}");

        if (allProgress == null)
        {
            Debug.LogWarning("[LevelLoader] allProgress is null");
            return;
        }

        foreach (var g in allProgress)
            Debug.Log($"[LevelLoader] record: BehandelingID={g.BehandelingID}, LevelProgress={g.LevelProgress}");

        bool level1Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1);
        bool level2Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 2);
        bool level3Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 3);
        bool level4Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 4);
        bool level5Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 5);
        bool level6Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 6);

        Debug.Log($"[LevelLoader] compleet: 1={level1Complete}, 2={level2Complete}, 3={level3Complete}, 4={level4Complete}, 5={level5Complete}, 6={level6Complete}");

        if (level1Indicator != null) level1Indicator.SetActive(level1Complete);
        if (level2Indicator != null) level2Indicator.SetActive(level2Complete);
        if (level3Indicator != null) level3Indicator.SetActive(level3Complete);
        if (level4Indicator != null) level4Indicator.SetActive(level4Complete);
        if (level5Indicator != null) level5Indicator.SetActive(level5Complete);
        if (level6Indicator != null) level6Indicator.SetActive(level6Complete);

        if (puntenText != null)
        {
            var record = allProgress.FirstOrDefault(g => g.BehandelingID == behandelingID);
            int punten = record != null ? record.Points : 0;
            puntenText.text = $"{punten} punten";
        }
    }

    private void DeactivateAllLevels()
    {
        if (LevelOverzicht != null) LevelOverzicht.SetActive(false);
        if (Level1 != null) Level1.SetActive(false);
        if (Level2 != null) Level2.SetActive(false);
        if (Level3 != null) Level3.SetActive(false);
        if (Level4 != null) Level4.SetActive(false);
        if (Level5 != null) Level5.SetActive(false);
        if (Level6 != null) Level6.SetActive(false);
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

        gameProgressController.ClearGameProgressCache();
        var progress = await gameProgressController.GetAll();

        if (progress == null || progress.Count == 0)
            return false;

        var id = GetBehandelingID();
        return progress.Any(g => g.BehandelingID == id && g.LevelProgress >= levelNumber);
    }

    public void OpenSettings()
    {
        SettingsMenu.SetActive(true);
        var settings = SettingsMenu.GetComponentInChildren<Assets.Scripts.Settings>();
        if (settings != null) settings.LoadedFromScene = "LevelOverZicht";
        LevelOverzicht.SetActive(false);

    }
}
