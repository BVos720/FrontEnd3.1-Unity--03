using MySecureBackend.WebApi.Models;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
//
public class Level2 : MonoBehaviour
{
    private const int LEVEL_NUMBER = 2;

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
    public LevelLoader levelLoader;

    private float playKnopTimer = 0f;
    private bool playKnopZichtbaar = false;
    private float volgendeKnopTimer = 0f;
    private bool volgendeKnopAktief = false;


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

            gameProgress = await gameProgressController.GetOrCreate(0f, 0, LEVEL_NUMBER);
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

        // Volgende knop timer (10 seconden na play button klik)
        if (volgendeKnopAktief)
        {
            volgendeKnopTimer += Time.deltaTime;
            if (volgendeKnopTimer >= 10f)
            {
                volgendeKnopAktief = false;
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

    private async void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log($"[Level2] OnVideoFinished called - gameProgress != null: {gameProgress != null}");
        if (gameProgress != null)
        {
            Debug.Log($"[Level2] Marking Level 2 as complete - Setting LevelProgress to 1.0 and Points to {LEVEL_NUMBER}");
            gameProgress.LevelProgress = 1f;
            gameProgress.Points = LEVEL_NUMBER;
            bool updateSuccess = await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
            Debug.Log($"[Level2] UpdateItem success: {updateSuccess}");
        }
        else
        {
            Debug.LogWarning("[Level2] OnVideoFinished called but gameProgress is null!");
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

            // Start de volgende knop timer (10 seconden)
            volgendeKnopAktief = true;
            volgendeKnopTimer = 0f;
        }
    }

    public async void GaNaarLevelOverzicht()
    {
        Debug.Log("[Level2] GaNaarLevelOverzicht called");

        // Ensure gameProgress is updated with completion status before navigating back
        if (gameProgress != null)
        {
            Debug.Log($"[Level2] Ensuring Level 2 is marked complete - LevelProgress: {gameProgress.LevelProgress}, Points: {gameProgress.Points}");

            // Only update if not already complete
            if (gameProgress.LevelProgress < 1f)
            {
                Debug.Log("[Level2] LevelProgress not set to 1.0, updating now");
                gameProgress.LevelProgress = 1f;
                gameProgress.Points = LEVEL_NUMBER;
                bool updateSuccess = await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
                Debug.Log($"[Level2] Update before navigation - success: {updateSuccess}");
            }
            else
            {
                Debug.Log("[Level2] LevelProgress already at 1.0, no update needed");
            }
        }

        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level2Object != null)
            level2Object.SetActive(false);

        // Refresh completion indicators
        if (levelLoader != null)
            levelLoader.RefreshCompletionIndicators();
    }
}

