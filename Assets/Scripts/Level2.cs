using MySecureBackend.WebApi.Models;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level2 : MonoBehaviour
{
    [Header("Instellingen")]
    [Tooltip("Aantal seconden voordat de Play-knop zichtbaar wordt.")]
    public float wachtTijdPlayKnop = 5f;

    [Header("UI Elementen")]
    public Button volgendeButton;
    public Button terugButton;
    public Button playButton;

    [Header("Video")]
    public GameObject videoObject;

    [Header("GameObject Referenties")]
    [Tooltip("Het GameObject van het leveloverzicht.")]
    public GameObject levelOverzichtObject;
    [Tooltip("Het GameObject van Level2 (meestal dit object zelf).")]
    public GameObject level2Object;
    public GameProgressController gameProgressController;
    public GameProgress gameProgress;
    public GameObject GameTheme;

    private float playKnopTimer = 0f;
    private bool playKnopZichtbaar = false;


    private void OnEnable()
    {
        GameTheme.SetActive(false);
    }

    public async void Start()
    {
        // Zorg dat de video gameobject initieel uitgeschakeld is
        if (videoObject != null)
        {
            videoObject.SetActive(false);
            var videoPlayer = videoObject.GetComponent<VideoPlayer>();
            if (videoPlayer != null)
            {
                videoPlayer.playOnAwake = false;
                videoPlayer.loopPointReached += OnVideoFinished;
            }
        }

        // Verberg de play knop initieel
        if (playButton != null)
        {
            playButton.gameObject.SetActive(false);
            playButton.onClick.AddListener(StartVideo);
        }

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
        // Play knop timer
        if (!playKnopZichtbaar)
        {
            playKnopTimer += Time.deltaTime;
            if (playKnopTimer >= wachtTijdPlayKnop)
            {
                playKnopZichtbaar = true;
                if (playButton != null)
                {
                    playButton.gameObject.SetActive(true);
                }
            }
        }
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        if (gameProgress != null)
        {
            gameProgress.LevelProgress = 1f;
            gameProgress.Points = 1;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }

        if (volgendeButton != null)
        {
            volgendeButton.interactable = true;
            var image = volgendeButton.GetComponent<Image>();
            if (image != null)
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
    }

    private void StartVideo()
    {
        if (videoObject != null)
        {
            // Schakel het video gameobject in
            videoObject.SetActive(true);

            var videoPlayer = videoObject.GetComponent<VideoPlayer>();
            if (videoPlayer != null)
                videoPlayer.Play();
        }
    }

    public async void GaNaarLevelOverzicht()
    {
        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level2Object != null)
            level2Object.SetActive(false);
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
        if (level2Object != null)
            level2Object.SetActive(false);
    }
}

