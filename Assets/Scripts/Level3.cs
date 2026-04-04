using MySecureBackend.WebApi.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level3 : MonoBehaviour
{
    private const int LEVEL_NUMBER = 3;

    [Header("UI Elementen")]
    public Button volgendeButton;
    public Button terugButton;
    public TMP_Text countdownText;

    [Header("Microfoon Instellingen")]
    public float volumeAmplifier = 3f;
    public float blowThreshold = 0.05f;

    [Header("Bubbels")]
    public ParticleSystem bubbleParticles;

    [Header("GameObject Referenties")]
    public GameObject levelOverzichtObject;
    public GameObject level3Object;
    public GameProgressController gameProgressController;
    public GameProgress gameProgress;
    public LevelLoader levelLoader;

    [Header("Sounds")]
    public AudioSource BubbleSound;

    private AudioClip microphoneClip;
    private int sampleRate = 44100;
    private float blowDuration = 0f;
    private const float REQUIRED_BLOW_TIME = 5f;
    private bool isBlowing = false;
    private bool levelCompleted = false;

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

        if (countdownText != null)
            countdownText.text = "";
    }

    void Update()
    {
        if (levelCompleted || microphoneClip == null)
            return;

        float currentVolume = GetMicrophoneVolume();

        if (currentVolume > blowThreshold)
        {
            if (!isBlowing)
            {
                isBlowing = true;
                blowDuration = 0f;
                if (bubbleParticles != null) bubbleParticles.Play();
                BubbleSound.Play();
            }

            blowDuration += Time.deltaTime;

            int secondsLeft = Mathf.CeilToInt(REQUIRED_BLOW_TIME - blowDuration);
            if (countdownText != null)
                countdownText.text = secondsLeft.ToString();

            if (blowDuration >= REQUIRED_BLOW_TIME)
                OnBlowDetected();
        }
        else
        {
            if (isBlowing && bubbleParticles != null) bubbleParticles.Stop();
            isBlowing = false;
            blowDuration = 0f;
            BubbleSound.Stop();
            if (countdownText != null)
                countdownText.text = "";
            
        }
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

    private async void OnBlowDetected()
    {
        levelCompleted = true;

        if (gameProgress != null)
        {
            gameProgress.LevelProgress = 1f;
            gameProgress.Points = LEVEL_NUMBER;
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

    public async void GaNaarLevelOverzicht()
    {
        if (gameProgress != null && gameProgress.LevelProgress < 1f)
        {
            gameProgress.LevelProgress = 1f;
            gameProgress.Points = LEVEL_NUMBER;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }

        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level3Object != null)
            level3Object.SetActive(false);

        if (levelLoader != null)
            levelLoader.RefreshCompletionIndicators();
    }

    void OnDisable()
    {
        if (Microphone.IsRecording(null))
            Microphone.End(null);
    }
}
