using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class MicTest : MonoBehaviour
{
    private AudioClip microphoneClip;
    private AudioSource audioSource;
    private int sampleRate = 44100;
    private float blowThreshold = 0.05f; // Volume threshold voor blazen detectie (lager = gevoeliger)
    private float blowDuration = 0f;
    private const float REQUIRED_BLOW_TIME = 5f; // 5 seconden
    private bool isBlowing = false;
    private int lastAudioPosition = 0;
    private float debugLogTimer = 0f;
    private const float DEBUG_LOG_INTERVAL = 0.5f; // 500 milliseconden

    public float volumeAmplifier = 3f; // Amplifieer het volume (hoger = gevoeliger)
    public Image volumeBar; // Drag je Image hier in de Inspector
    public GameObject[] objectsToShow;
    public GameObject[] objectsToHide;
    public bool oneTimeToggle = true;      // true = alleen 1x togglen
    public float resetAfterSeconds = 0f;   // 0 = geen reset (anders reset na X seconden)
    private bool alreadyToggled = false;

    public UnityEvent OnBlowEvent;

    void Start()
    {
        // Start microfoon opname
        microphoneClip = Microphone.Start(null, true, 30, sampleRate);

        // Wacht tot microfoon klaar is
        int waitTime = 0;
        while (Microphone.GetPosition(null) <= 0 && waitTime < 100)
        {
            waitTime++;
        }

        Debug.Log("Microfoon gestart. Blaas in de microfoon...");
        Debug.Log($"Microfoon clip lengte: {microphoneClip.length}s, Sample rate: {sampleRate}");
    }

    void Update()
    {
        if (microphoneClip == null)
            return;

        // Analyseer microfoon audio
        float currentVolume = GetMicrophoneVolume();

        // Update UI volume bar
        if (volumeBar != null)
        {
            volumeBar.fillAmount = currentVolume;
        }

        // Check of er wordt geblazen
        if (currentVolume > blowThreshold)
        {
            if (!isBlowing)
            {
                isBlowing = true;
                blowDuration = 0f;
            }

            blowDuration += Time.deltaTime;

            // Check of 5 seconden bereikt is
            if (blowDuration >= REQUIRED_BLOW_TIME)
            {
                OnBlowDetected();
                blowDuration = 0f;
            }
        }
        else
        {
            // Reset blazen timer als volume onder drempel valt
            if (isBlowing)
            {
                Debug.Log($"Blazen gestopt. Duur was: {blowDuration:F1}s");
            }
            isBlowing = false;
            blowDuration = 0f;
        }
    }

    private float GetMicrophoneVolume()
    {
        if (microphoneClip == null)
            return 0f;

        int micPosition = Microphone.GetPosition(null);

        if (micPosition < 0 || micPosition == 0)
            return 0f;

        // Lees de laatste 4410 samples (100ms bij 44100Hz)
        int sampleCount = Mathf.Min(4410, micPosition);
        int startPos = Mathf.Max(0, micPosition - sampleCount);

        float[] samples = new float[sampleCount];

        try
        {
            microphoneClip.GetData(samples, startPos);
        }
        catch
        {
            return 0f;
        }

        // Bereken RMS (Root Mean Square) voor volume niveau
        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }

        float rms = Mathf.Sqrt(sum / samples.Length);

        // Amplifieer het volume voor betere zichtbaarheid
        float amplifiedRms = rms * volumeAmplifier;

        // Zorg dat het niet hoger wordt dan 1
        return Mathf.Clamp01(amplifiedRms);
    }

    private void OnBlowDetected()
    {
        Debug.Log("★ BLAZEN GEDETECTEERD! 5 seconden blazen bereikt! ★");

        if (oneTimeToggle && alreadyToggled) return;

        // hide
        if (objectsToHide != null)
        {
            foreach (var g in objectsToHide) if (g != null) g.SetActive(false);
        }

        // show
        if (objectsToShow != null)
        {
            foreach (var g in objectsToShow) if (g != null) g.SetActive(true);
        }

        alreadyToggled = true;

        if (resetAfterSeconds > 0f)
            StartCoroutine(ResetToggleAfterSeconds(resetAfterSeconds));

        OnBlowEvent?.Invoke();
    }

    private System.Collections.IEnumerator ResetToggleAfterSeconds(float secs)
    {
        yield return new WaitForSeconds(secs);

        // revert
        if (objectsToHide != null)
        {
            foreach (var g in objectsToHide) if (g != null) g.SetActive(true);
        }
        if (objectsToShow != null)
        {
            foreach (var g in objectsToShow) if (g != null) g.SetActive(false);
        }

        alreadyToggled = false;
    }

    void OnDisable()
    {
        // Stop microfoon wanneer script wordt uitgeschakeld
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
        }
    }
}
