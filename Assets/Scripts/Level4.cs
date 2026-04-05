using MySecureBackend.WebApi.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level4 : MonoBehaviour
{
    private const int LEVEL_NUMBER = 4;

    [Header("Microfoon Instellingen")]
    public float volumeAmplifier = 3f;
    public float blowThreshold = 0.05f;

    [Header("Ritme Instellingen")]
    public int aantalBlazen = 2;
    public float secondenPerBlaas = 7f;

    [Header("UI Elementen")]
    public Button volgendeButton;
    public Button terugButton;
    public TMP_Text tellerText;
    public TMP_Text instructieTekst;

    [Header("GameObject Referenties")]
    public GameObject levelOverzichtObject;
    public GameObject level4Object;
    public GameProgressController gameProgressController;
    public GameProgress gameProgress;
    public LevelLoader levelLoader;

    private AudioClip microphoneClip;
    private int sampleRate = 44100;
    private float blowDuration = 0f;
    private bool isBlowing = false;
    private bool levelCompleted = false;
    private int voltooideBlazens = 0;

    public async void Start()
    {
        microphoneClip = Microphone.Start(null, true, 30, sampleRate);

        int waitTime = 0;
        while (Microphone.GetPosition(null) <= 0 && waitTime < 100)
            waitTime++;

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

        UpdateUI();
    }

    void Update()
    {
        if (levelCompleted || microphoneClip == null)
            return;

        float currentVolume = GetMicrophoneVolume();

        if (currentVolume > blowThreshold)
        {
            isBlowing = true;
            blowDuration += Time.deltaTime;

            if (blowDuration >= secondenPerBlaas)
            {
                voltooideBlazens++;
                blowDuration = 0f;
                isBlowing = false;
                UpdateUI();

                if (voltooideBlazens >= aantalBlazen)
                    OnLevelCompleted();
            }
        }
        else
        {
            isBlowing = false;
            blowDuration = 0f;
        }
    }

    private void UpdateUI()
    {
        if (tellerText != null)
            tellerText.text = $"{voltooideBlazens}/{aantalBlazen}";

        if (instructieTekst != null)
            instructieTekst.text = $"BLAAS NU {aantalBlazen} KEER\nVOOR {(int)secondenPerBlaas} SECONDEN!";
    }

    private float GetMicrophoneVolume()
    {
        int micPosition = Microphone.GetPosition(null);
        if (micPosition <= 0)
            return 0f;

        int sampleCount = Mathf.Min(4410, micPosition);
        int startPos = Mathf.Max(0, micPosition - sampleCount);
        float[] samples = new float[sampleCount];

        try { microphoneClip.GetData(samples, startPos); }
        catch { return 0f; }

        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
            sum += samples[i] * samples[i];

        float rms = Mathf.Sqrt(sum / samples.Length);
        return Mathf.Clamp01(rms * volumeAmplifier);
    }

    private void OnLevelCompleted()
    {
        levelCompleted = true;

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

    public async void VolgendLevel()
    {
        if (gameProgress != null)
        {
            gameProgress.LevelProgress = LEVEL_NUMBER;
            gameProgress.Points = 5;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }

        levelOverzichtObject.SetActive(true);
        level4Object.SetActive(false);
    }

    public async void GaNaarLevelOverzicht()
    {
        if (gameProgress != null && gameProgress.LevelProgress < 1f)
        {
            gameProgress.LevelProgress = 1f;
            gameProgress.Points = LEVEL_NUMBER;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }

        levelOverzichtObject.SetActive(true);
        level4Object.SetActive(false);

        if (levelLoader != null)
            levelLoader.RefreshCompletionIndicators();
    }

    void OnDisable()
    {
        if (Microphone.IsRecording(null))
            Microphone.End(null);
    }
}
