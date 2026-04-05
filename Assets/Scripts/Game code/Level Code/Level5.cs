using MySecureBackend.WebApi.Models;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Level5 : MonoBehaviour
{
    private const int LEVEL_NUMBER = 5;

    [Header("UI Elementen")]
    public Button terugButton;

    [Header("GameObject Referenties")]
    public GameObject levelOverzichtObject;
    public GameObject level5Object;
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
            gameProgress.LevelProgress = LEVEL_NUMBER;
            gameProgress.Points = LEVEL_NUMBER;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }
    }

    public async void GaNaarLevelOverzicht()
    {
        Debug.Log("[Level5] GaNaarLevelOverzicht called");

        // Ensure gameProgress is updated with completion status before navigating back
        if (gameProgress != null)
        {
            Debug.Log($"[Level5] Ensuring Level 5 is marked complete - LevelProgress: {gameProgress.LevelProgress}, Points: {gameProgress.Points}");

            // Only update if not already complete
            if (gameProgress.LevelProgress < LEVEL_NUMBER)
            {
                Debug.Log($"[Level5] LevelProgress not set to {LEVEL_NUMBER}, updating now");
                gameProgress.LevelProgress = LEVEL_NUMBER;
                gameProgress.Points = LEVEL_NUMBER;
                bool updateSuccess = await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
                Debug.Log($"[Level5] Update before navigation - success: {updateSuccess}");
            }
            else
            {
                Debug.Log($"[Level5] LevelProgress already at {LEVEL_NUMBER}, no update needed");
            }
        }

        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level5Object != null)
            level5Object.SetActive(false);

        // Refresh completion indicators
        if (levelLoader != null)
            levelLoader.RefreshCompletionIndicators();
    }
}
