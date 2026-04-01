using MySecureBackend.WebApi.Models;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Level3 : MonoBehaviour
{
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
    public GameObject snorkelMaskOverlay;
    

    private float timer = 0f;
    private bool knopActief = false;


    private void OnEnable()
    {
        GameTheme.SetActive(false);
        if (snorkelMaskOverlay != null)
            snorkelMaskOverlay.SetActive(false);
    }

    public async void Start()
    {
        if (volgendeButton != null)
        {
            volgendeButton.interactable = false;
            var image = volgendeButton.GetComponent<Image>();
            if (image != null)
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.95f);
            volgendeButton.onClick.AddListener(GaNaarVolgendeLevel);

            gameProgress = await gameProgressController.Create(0f, 0);
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

                if (snorkelMaskOverlay != null)
                    snorkelMaskOverlay.SetActive(true);

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
        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level3Object != null)
            level3Object.SetActive(false);
        await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
    }

    public async void GaNaarVolgendeLevel()
    {
        if (gameProgress != null)
        {
            gameProgress.LevelProgress = 1f;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }

        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level3Object != null)
            level3Object.SetActive(false);
    }
}
