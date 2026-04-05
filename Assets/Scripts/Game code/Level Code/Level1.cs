using MySecureBackend.WebApi.Models;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Level1 : MonoBehaviour
{
    private const int LEVEL_NUMBER = 1;

    [Header("Instellingen")]
    public float wachtTijdPlayKnop = 5f;

    [Header("UI Elementen")]
    public Button volgendeButton;
    public Button terugButton;
    public Button playButton;

    [Header("Video")]
    public GameObject videoObject;

    [Header("GameObject Referenties")]
    public GameObject levelOverzichtObject;
    public GameObject level1Object;
    public GameProgressController gameProgressController;
    public GameProgress gameProgress;
    public GameObject GameTheme;
    public LevelLoader levelLoader;

    private float playKnopTimer = 0f;
    private bool playKnopZichtbaar = false;

    private void OnEnable()
    {
        GameTheme.SetActive(false);
    }

    public async void Start()
    {
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

            gameProgress = await gameProgressController.GetOrCreate(0f, 0, LEVEL_NUMBER);
        }

        if (terugButton != null)
            terugButton.onClick.AddListener(GaNaarLevelOverzicht);
    }

    void Update()
    {
        // play knop timer
        if (!playKnopZichtbaar)
        {
            playKnopTimer += Time.deltaTime;
            if (playKnopTimer >= wachtTijdPlayKnop)
            {
                playKnopZichtbaar = true;
                if (playButton != null)
                    playButton.gameObject.SetActive(true);
            }
        }
    }

    private async void OnVideoFinished(VideoPlayer vp)
    {
        if (gameProgress != null)
        {
            gameProgress.LevelProgress = LEVEL_NUMBER;
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
            videoObject.SetActive(true);
            var videoPlayer = videoObject.GetComponent<VideoPlayer>();
            if (videoPlayer != null)
                videoPlayer.Play();
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
        if (level1Object != null)
            level1Object.SetActive(false);

        if (levelLoader != null)
            levelLoader.RefreshCompletionIndicators();
    }
}
