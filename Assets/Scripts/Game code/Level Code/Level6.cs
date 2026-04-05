using MySecureBackend.WebApi.Models;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Level6 : MonoBehaviour
{
    private const int LEVEL_NUMBER = 6;

    [Header("UI Elementen")]
    public Button terugButton;

    [Header("GameObject Referenties")]
    public GameObject levelOverzichtObject;
    public GameObject level6Object;
    public GameProgressController gameProgressController;
    public GameProgress gameProgress;
    public GameObject GameTheme;
    public LevelLoader levelLoader;

    private void OnEnable()
    {
        GameTheme.SetActive(false);
    }

    public async void Start()
    {
        if (terugButton != null)
            terugButton.onClick.AddListener(GaNaarLevelOverzicht);

        gameProgress = await gameProgressController.GetOrCreate(0f, 0, LEVEL_NUMBER);
        
        // Mark level as complete immediately
        if (gameProgress != null)
        {
            gameProgress.LevelProgress = 1f;
            gameProgress.Points = LEVEL_NUMBER;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }
    }

    public async void GaNaarLevelOverzicht()
    {
        Debug.Log("[Level6] GaNaarLevelOverzicht called");

        // Ensure gameProgress is updated with completion status before navigating back
        if (gameProgress != null)
        {
            Debug.Log($"[Level6] Ensuring Level 6 is marked complete - LevelProgress: {gameProgress.LevelProgress}, Points: {gameProgress.Points}");

            // Only update if not already complete
            if (gameProgress.LevelProgress < 1f)
            {
                Debug.Log("[Level6] LevelProgress not set to 1.0, updating now");
                gameProgress.LevelProgress = 1f;
                gameProgress.Points = LEVEL_NUMBER;
                bool updateSuccess = await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
                Debug.Log($"[Level6] Update before navigation - success: {updateSuccess}");
            }
            else
            {
                Debug.Log("[Level6] LevelProgress already at 1.0, no update needed");
            }
        }

        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level6Object != null)
            level6Object.SetActive(false);

        // Refresh completion indicators
        if (levelLoader != null)
            levelLoader.RefreshCompletionIndicators();
    }
}
