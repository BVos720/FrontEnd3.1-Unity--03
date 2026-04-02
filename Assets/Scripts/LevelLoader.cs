using UnityEngine;
using MySecureBackend.WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject LevelOverzicht;
    public GameObject Overzicht;
    public GameObject Level1;
    public GameObject Level2;
    public GameObject Level3;
    public GameObject Level4;
    public GameObject LevelChest;
    public GameProgressController gameProgressController;

    [Header("Completion Indicators")]
    public GameObject level1Indicator;
    public GameObject level2Indicator;
    public GameObject level3Indicator;
    public GameObject level4Indicator;

    private void Start()
    {
        UpdateCompletionIndicators();
    }

    private void OnEnable()
    {
        UpdateCompletionIndicators();
    }

    public void LoadLevel1()
    {
        DeactivateAllLevels();
        if (Level1 != null) Level1.SetActive(true);
        Debug.Log("Level 1 geactiveerd");
    }

    public async void LoadLevel2()
    {
        // Check if Level 1 is complete
        if (!await IsLevelComplete(1))
        {
            Debug.LogWarning("Level 2 is locked! Complete Level 1 first.");
            return;
        }

        DeactivateAllLevels();
        if (Level2 != null) Level2.SetActive(true);
        Debug.Log("Level 2 geactiveerd");
    }

    public async void LoadLevel3()
    {
        // Check if Level 2 is complete
        if (!await IsLevelComplete(2))
        {
            Debug.LogWarning("Level 3 is locked! Complete Level 2 first.");
            return;
        }

        DeactivateAllLevels();
        if (Level3 != null) Level3.SetActive(true);
        Debug.Log("Level 3 geactiveerd");
    }

    public async void LoadLevel4()
    {
        // Check if Level 3 is complete
        if (!await IsLevelComplete(3))
        {
            Debug.LogWarning("Level 4 is locked! Complete Level 3 first.");
            return;
        }

        DeactivateAllLevels();
        if (Level4 != null) Level4.SetActive(true);
        Debug.Log("Level 4 geactiveerd");
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
            Debug.LogError("GameProgressController is niet toegewezen!");
            return;
        }

        List<GameProgress> allProgress = await gameProgressController.GetAll();
        if (allProgress == null || allProgress.Count == 0)
        {
            Debug.LogWarning("Geen spelvoortgang gevonden. Je moet eerst alle levels voltooien.");
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
            Debug.LogWarning($"Je moet eerst alle 4 levels voltooien. Voltooid: L1={level1Complete}, L2={level2Complete}, L3={level3Complete}, L4={level4Complete}");
            return;
        }

        // Alle levels voltooid - laad de kist
        DeactivateAllLevels();
        if (LevelChest != null) 
        {
            LevelChest.SetActive(true);
            Debug.Log("Level Kist geactiveerd");
        }
        else
        {
            Debug.LogWarning("LevelChest GameObject is niet toegewezen!");
        }
    }

    private async void UpdateCompletionIndicators()
    {
        if (gameProgressController == null)
        {
            Debug.LogWarning("[LevelLoader] gameProgressController is null");
            return;
        }

        List<GameProgress> allProgress = await gameProgressController.GetAll();
        if (allProgress == null)
        {
            Debug.LogWarning("[LevelLoader] allProgress is null");
            return;
        }

        Debug.Log($"[LevelLoader] UpdateCompletionIndicators - Total GameProgress records: {allProgress.Count}");

        string behandelingIDStr = PlayerPrefs.GetString("behandelingID", "");
        System.Guid.TryParse(behandelingIDStr, out System.Guid behandelingID);
        Debug.Log($"[LevelLoader] Current BehandelingID: {behandelingID}");

        // Log all records for debugging
        foreach (var gp in allProgress)
        {
            Debug.Log($"[LevelLoader] GameProgress - BehandelingID: {gp.BehandelingID}, LevelProgress: {gp.LevelProgress}, Points: {gp.Points}");
        }

        // Check if each specific level is completed (Points field stores the level number)
        bool level1Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 1);
        bool level2Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 2);
        bool level3Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 3);
        bool level4Complete = allProgress.Any(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f && g.Points == 4);

        Debug.Log($"[LevelLoader] Completion status - L1: {level1Complete}, L2: {level2Complete}, L3: {level3Complete}, L4: {level4Complete}");
        Debug.Log($"[LevelLoader] Indicators assigned - L1: {level1Indicator != null}, L2: {level2Indicator != null}, L3: {level3Indicator != null}, L4: {level4Indicator != null}");

        // Update indicators - show checkmark GameObject for completed levels
        if (level1Indicator != null)
        {
            level1Indicator.SetActive(level1Complete);
            Debug.Log($"[LevelLoader] level1Indicator set to: {level1Complete}");
        }
        else
            Debug.LogWarning("[LevelLoader] level1Indicator is null!");

        if (level2Indicator != null)
        {
            level2Indicator.SetActive(level2Complete);
            Debug.Log($"[LevelLoader] level2Indicator set to: {level2Complete}");
        }
        else
            Debug.LogWarning("[LevelLoader] level2Indicator is null!");

        if (level3Indicator != null)
        {
            level3Indicator.SetActive(level3Complete);
            Debug.Log($"[LevelLoader] level3Indicator set to: {level3Complete}");
        }
        else
            Debug.LogWarning("[LevelLoader] level3Indicator is null!");

        if (level4Indicator != null)
        {
            level4Indicator.SetActive(level4Complete);
            Debug.Log($"[LevelLoader] level4Indicator set to: {level4Complete}");
        }
        else
            Debug.LogWarning("[LevelLoader] level4Indicator is null!");
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
}
