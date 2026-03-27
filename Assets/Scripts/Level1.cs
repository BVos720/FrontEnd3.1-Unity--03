using MySecureBackend.WebApi.Models;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
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
    [Tooltip("Het GameObject van Level1 (meestal dit object zelf).")]
    public GameObject level1Object;
    public GameObject scenesMetMuziek;
    public GameObject scenesZonderMuziek;
    public GameProgressController gameProgressController;
    public GameProgress gameProgress;

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
            volgendeButton.onClick.AddListener(GaNaarLevelOverzicht);

            gameProgress = await gameProgressController.Create(0, 0);
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
                if (volgendeButton != null)
                {
                    volgendeButton.interactable = true;
                    var image = volgendeButton.GetComponent<Image>();
                    if (image != null)
                        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f); // Volledig zichtbaar
                }
            }
        }
    }

    public void GaNaarLevelOverzicht()
    {
        // Wissel muziek scenes
        if (scenesMetMuziek != null) scenesMetMuziek.SetActive(true);
        if (scenesZonderMuziek != null) scenesZonderMuziek.SetActive(false);

        // Activeer overzicht en deactiveer level
        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level1Object != null)
            level1Object.SetActive(false);
    }
    public async void GaNaarVolgendeLevel()
    {
        if (gameProgress != null)
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
    }
}
