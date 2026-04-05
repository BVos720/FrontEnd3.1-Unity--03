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

        if (gameProgress != null && gameProgress.LevelProgress < LEVEL_NUMBER)
        {
            gameProgress.LevelProgress = LEVEL_NUMBER;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }
    }

    public async void GaNaarLevelOverzicht()
    {
        if (gameProgress != null && gameProgress.LevelProgress < LEVEL_NUMBER)
        {
            gameProgress.LevelProgress = LEVEL_NUMBER;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }

        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level5Object != null)
            level5Object.SetActive(false);

        if (levelLoader != null)
            levelLoader.RefreshCompletionIndicators();
    }
}
