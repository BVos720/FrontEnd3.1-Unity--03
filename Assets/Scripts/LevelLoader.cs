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

    public void LoadLevel2()
    {
        DeactivateAllLevels();
        if (Level2 != null) Level2.SetActive(true);
        Debug.Log("Level 2 geactiveerd");
    }

    public void LoadLevel3()
    {
        DeactivateAllLevels();
        if (Level3 != null) Level3.SetActive(true);
        Debug.Log("Level 3 geactiveerd");
    }

    public void LoadLevel4()
    {
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

        // Controleer of Level 1, 2, 3 en 4 allemaal zijn voltooid (LevelProgress = 1)
        var completedLevels = allProgress.Where(g => g.LevelProgress >= 1f).ToList();

        if (completedLevels.Count < 4)
        {
            Debug.LogWarning($"Je moet eerst alle 4 levels voltooien. Voltooid: {completedLevels.Count}/4");
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
        if (gameProgressController == null) return;

        List<GameProgress> allProgress = await gameProgressController.GetAll();
        if (allProgress == null) return;

        string behandelingIDStr = PlayerPrefs.GetString("behandelingID", "");
        System.Guid.TryParse(behandelingIDStr, out System.Guid behandelingID);

        var completedLevels = allProgress
            .Where(g => g.BehandelingID == behandelingID && g.LevelProgress >= 1f)
            .ToList();

        // Update indicators - show checkmark GameObject for completed levels
        if (level1Indicator != null)
            level1Indicator.SetActive(completedLevels.Count >= 1);

        if (level2Indicator != null)
            level2Indicator.SetActive(completedLevels.Count >= 2);

        if (level3Indicator != null)
            level3Indicator.SetActive(completedLevels.Count >= 3);

        if (level4Indicator != null)
            level4Indicator.SetActive(completedLevels.Count >= 4);
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
}
