using MySecureBackend.WebApi.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level4 : MonoBehaviour
{
    private const int LEVEL_NUMBER = 4;

    [Header("UI Elementen")]
    public Button volgendeButton;
    public Button terugButton;

    [Header("Blaas Counter")]
    public GameObject counter0;
    public GameObject counter1;
    public GameObject counter2;
    public GameObject uitlegTekst;
    public GameObject voltooidTekst;
    public TMP_Text countdownText;

    [Header("Microfoon Instellingen")]
    public float volumeAmplifier = 3f;
    public float blowThreshold = 0.05f;

    [Header("Ritme Instellingen")]
    public int aantalBlazen = 2;
    public float secondenPerBlaas = 7f;

    [Header("Bubbels & Geluid")]
    public ParticleSystem bubbleParticles;
    public AudioSource BubbleSound;
    public GameObject Gametheme;

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

    private void OnEnable()
    {
        if (Gametheme != null)
            Gametheme.SetActive(false);
    }

    public async void Start()
    {
        microphoneClip = Microphone.Start(null, true, 30, sampleRate);

        int waitTime = 0;
        while (Microphone.GetPosition(null) <= 0 && waitTime < 100)
            waitTime++;

        if (uitlegTekst != null) uitlegTekst.SetActive(true);
        if (voltooidTekst != null) voltooidTekst.SetActive(false);
        if (countdownText != null) countdownText.text = "";
        UpdateProgressUI();

        if (volgendeButton != null)
        {
            volgendeButton.gameObject.SetActive(false);
            gameProgress = await gameProgressController.GetOrCreate(0f, 0, LEVEL_NUMBER);
        }

        if (terugButton != null)
            terugButton.onClick.AddListener(GaNaarLevelOverzicht);

        if (bubbleParticles != null)
            bubbleParticles.Stop();
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
                if (bubbleParticles != null) bubbleParticles.Play();
                if (BubbleSound != null) BubbleSound.Play();
            }

            blowDuration += Time.deltaTime;

            int secondsLeft = Mathf.CeilToInt(secondenPerBlaas - blowDuration);
            if (countdownText != null) countdownText.text = secondsLeft.ToString();

            if (blowDuration >= secondenPerBlaas)
            {
                voltooideBlazens++;
                blowDuration = 0f;
                isBlowing = false;

                Debug.Log($"[Level4] Blow voltooid! voltooideBlazens={voltooideBlazens}, aantalBlazen={aantalBlazen}");

                if (countdownText != null) countdownText.text = "";
                UpdateProgressUI();

                if (bubbleParticles != null) bubbleParticles.Stop();
                if (BubbleSound != null) BubbleSound.Stop();

                if (voltooideBlazens >= aantalBlazen)
                {
                    Debug.Log("[Level4] Level voltooid!");
                    OnLevelCompleted();
                }
            }
        }
        else
        {
            if (isBlowing)
            {
                if (bubbleParticles != null) bubbleParticles.Stop();
                if (BubbleSound != null) BubbleSound.Stop();
            }

            isBlowing = false;
            blowDuration = 0f;
            if (countdownText != null) countdownText.text = "";
        }
    }

    private void UpdateProgressUI()
    {
        Debug.Log($"[Level4] UpdateProgressUI - voltooideBlazens={voltooideBlazens}, counter0={counter0 != null}, counter1={counter1 != null}, counter2={counter2 != null}");
        if (counter0 != null) counter0.SetActive(voltooideBlazens == 0);
        if (counter1 != null) counter1.SetActive(voltooideBlazens == 1);
        if (counter2 != null) counter2.SetActive(voltooideBlazens == 2);
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

        if (uitlegTekst != null) uitlegTekst.SetActive(false);
        if (voltooidTekst != null) voltooidTekst.SetActive(true);

        if (gameProgress != null)
        {
            gameProgress.LevelProgress = LEVEL_NUMBER;
            gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }

        if (volgendeButton != null)
            volgendeButton.gameObject.SetActive(true);
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
        if (gameProgress != null && gameProgress.LevelProgress < LEVEL_NUMBER)
        {
            gameProgress.LevelProgress = LEVEL_NUMBER;
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
