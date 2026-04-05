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
    public GameObject Gametheme;

    private AudioClip microphoneClip;
    private float blowDuration = 0f;
    private const float REQUIRED_BLOW_TIME = 5f;
    private bool isBlowing = false;
    private bool levelCompleted = false;

   
    private string micDevice;

    private void OnEnable()
    {
        Gametheme.SetActive(false);
    }

    public async void Start()
    {
       
        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0];
            int currentSampleRate = AudioSettings.outputSampleRate;
            if (currentSampleRate == 0) currentSampleRate = 44100;

            microphoneClip = Microphone.Start(micDevice, true, 30, currentSampleRate);

            int waitTime = 0;
            while (Microphone.GetPosition(micDevice) <= 0 && waitTime < 100)
                waitTime++;
        }
        else
        {
            Debug.LogWarning("Geen microfoon gevonden door Unity. Gebruik spatiebalk om te testen!");
        }

        if (volgendeButton != null)
        {
            volgendeButton.interactable = false;
            var image = volgendeButton.GetComponent<Image>();
            if (image != null)
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.95f);

            gameProgress = await gameProgressController.GetOrCreate(0f, 0, LEVEL_NUMBER);
        }

        if (countdownText != null)
            countdownText.text = "";

        if (bubbleParticles != null)
            bubbleParticles.Stop();
    }

    void Update()
    {
        if (levelCompleted)
            return;

        float currentVolume = GetMicrophoneVolume();

        if (currentVolume > blowThreshold)
        {
            if (!isBlowing)
            {
                isBlowing = true;
                blowDuration = 0f;
                if (bubbleParticles != null) bubbleParticles.Play();
                if (BubbleSound != null && !BubbleSound.isPlaying) BubbleSound.Play();
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
            if (BubbleSound != null) BubbleSound.Stop();
            if (countdownText != null)
                countdownText.text = "";
        }
    }

    private float GetMicrophoneVolume()
    {
       
        if (Application.isEditor &&
            UnityEngine.InputSystem.Keyboard.current != null &&
            UnityEngine.InputSystem.Keyboard.current.spaceKey.isPressed)
        {
            return 1f;
        }

        if (string.IsNullOrEmpty(micDevice) || microphoneClip == null)
            return 0f;

        int micPosition = Microphone.GetPosition(micDevice);
        if (micPosition <= 0)
            return 0f;

        int sampleCount = Mathf.Min(4410, micPosition);
        int startPos = Mathf.Max(0, micPosition - sampleCount);

        
        if (startPos < 0 || startPos + sampleCount > microphoneClip.samples)
            return 0f;

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

    public async void GaNaarLevelOverzicht()
    {
        if (gameProgress != null && gameProgress.LevelProgress < LEVEL_NUMBER)
        {
            gameProgress.LevelProgress = LEVEL_NUMBER;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }

        if (levelOverzichtObject != null)
            levelOverzichtObject.SetActive(true);
        if (level3Object != null)
            level3Object.SetActive(false);

        if (levelLoader != null)
            levelLoader.RefreshCompletionIndicators();
    }

    public void TerugNaarOverzicht()
    {
        levelOverzichtObject.SetActive(true);
        level3Object.SetActive(false);
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
        level3Object.SetActive(false);
    }

    void OnDisable()
    {
        
        if (!string.IsNullOrEmpty(micDevice) && Microphone.IsRecording(micDevice))
            Microphone.End(micDevice);
    }
}