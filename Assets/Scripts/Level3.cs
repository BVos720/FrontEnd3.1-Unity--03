using MySecureBackend.WebApi.Models;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Level3 : MonoBehaviour
{
    private const int LEVEL_NUMBER = 3;

    [Header("Instellingen")]
    [Tooltip("Aantal seconden voordat de Volgende-knop zichtbaar wordt.")]
    public float wachtTijd = 10f;

    [Header("UI Elementen")]
    public Button volgendeButton;
    public Button terugButton;

    [Header("GameObject Referenties")]
    [Tooltip("Het GameObject van het leveloverzicht.")]
    public GameObject levelOverzichtObject;
    [Tooltip("Het GameObject van Level3 (meestal dit object zelf).")]
    public GameObject level3Object;
    public GameProgressController gameProgressController;
    public GameProgress gameProgress;
    public GameObject GameTheme;
    [Tooltip("Snorkelmasker overlay dat verschijnt na de leestijd.")]
    public LevelLoader levelLoader;


    private float timer = 0f;
    private bool knopActief = false;




    public async void Start()
    {
        if (volgendeButton != null)
        {
            volgendeButton.interactable = false;
            var image = volgendeButton.GetComponent<Image>();
            if (image != null)
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.95f);

            gameProgress = await gameProgressController.GetOrCreate(0f, 0, LEVEL_NUMBER);
        }

        if (terugButton != null)
            terugButton.onClick.AddListener(GaNaarLevelOverzicht);
    }

    void Update()
    {
        if (!knopActief)
        {
            timer += Time.deltaTime;
            if (timer >= wachtTijd)
            {
                knopActief = true;

                // Mark level as complete when timer finishes
                if (gameProgress != null)
                {
                    gameProgress.LevelProgress = 1f;
                    gameProgress.Points = LEVEL_NUMBER;
                    gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
                }


                if (volgendeButton != null)
                {
                    volgendeButton.interactable = true;
                    var image = volgendeButton.GetComponent<Image>();
                    if (image != null)
                        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
                }
            }
        }
    }

    public async void GaNaarLevelOverzicht()
    {
        Debug.Log("[Level3] GaNaarLevelOverzicht called");

        // Ensure gameProgress is updated with completion status before navigating back
        if (gameProgress != null)
        {
            Debug.Log($"[Level3] Ensuring Level 3 is marked complete - LevelProgress: {gameProgress.LevelProgress}, Points: {gameProgress.Points}");

            // Only update if not already complete
            if (gameProgress.LevelProgress < 1f)
            {
                Debug.Log("[Level3] LevelProgress not set to 1.0, updating now");
                gameProgress.LevelProgress = 1f;
                gameProgress.Points = LEVEL_NUMBER;
                bool updateSuccess = await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
                Debug.Log($"[Level3] Update before navigation - success: {updateSuccess}");
            }
            else
            {
                Debug.Log("[Level3] LevelProgress already at 1.0, no update needed");
            }
        }

        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level3Object != null)
            level3Object.SetActive(false);

        // Refresh completion indicators
        if (levelLoader != null)
            levelLoader.RefreshCompletionIndicators();
    }
}
