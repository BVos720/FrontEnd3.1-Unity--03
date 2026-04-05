using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class MicTest : MonoBehaviour
{
    private AudioClip microphoneClip;
    private int sampleRate = 44100;
    private float blowThreshold = 0.05f;
    private float blowDuration = 0f;
    private const float REQUIRED_BLOW_TIME = 5f;
    private bool isBlowing = false;

    // FIX: Variabele voor het specifieke apparaat
    private string micDevice;

    public float volumeAmplifier = 3f;
    public Image volumeBar;
    public GameObject[] objectsToShow;
    public GameObject[] objectsToHide;
    public GameObject[] permanentObjectsToShow;
    public GameObject[] permanentObjectsToHide;
    public bool oneTimeToggle = true;
    public float resetAfterSeconds = 0f;
    private bool alreadyToggled = false;
    private bool permanentShowSet = false;
    private bool permanentHideSet = false;

    public UnityEvent OnBlowEvent;

    void Start()
    {
        // FIX: Dynamische hardware initialisatie voorkomt de C++ FMOD lock error
        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0];
            int currentSampleRate = AudioSettings.outputSampleRate;
            if (currentSampleRate == 0) currentSampleRate = 44100;

            microphoneClip = Microphone.Start(micDevice, true, 30, currentSampleRate);

            int waitTime = 0;
            while (Microphone.GetPosition(micDevice) <= 0 && waitTime < 100)
            {
                waitTime++;
            }

            Debug.Log($"Microfoon gestart op {micDevice}. Sample rate: {currentSampleRate}");
        }
        else
        {
            Debug.LogWarning("Geen microfoon gevonden! Gebruik de spatiebalk in de Editor.");
        }
    }

    void Update()
    {
        // Update UI volume bar (werkt nu ook met de spatiebalk)
        float currentVolume = GetMicrophoneVolume();

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

            if (blowDuration >= REQUIRED_BLOW_TIME)
            {
                OnBlowDetected();
                blowDuration = 0f;
            }
        }
        else
        {
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
        // FIX: Spatiebalk fallback (Nieuwe Input Systeem)
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

        // FIX: Voorkom dat we data opvragen buiten de perken van de clip (Dit voorkomt ook de C++ crash)
        if (startPos < 0 || startPos + sampleCount > microphoneClip.samples)
            return 0f;

        float[] samples = new float[sampleCount];

        try
        {
            microphoneClip.GetData(samples, startPos);
        }
        catch
        {
            return 0f;
        }

        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }

        float rms = Mathf.Sqrt(sum / samples.Length);
        return Mathf.Clamp01(rms * volumeAmplifier);
    }

    private void OnBlowDetected()
    {
        Debug.Log("★ BLAZEN GEDETECTEERD! 5 seconden blazen bereikt! ★");

        if (oneTimeToggle && alreadyToggled) return;

        if (objectsToHide != null)
        {
            foreach (var g in objectsToHide) if (g != null) g.SetActive(false);
        }

        if (objectsToShow != null)
        {
            foreach (var g in objectsToShow) if (g != null) g.SetActive(true);
        }

        if (!permanentShowSet && permanentObjectsToShow != null)
        {
            foreach (var g in permanentObjectsToShow) if (g != null) g.SetActive(true);
            permanentShowSet = true;
        }

        if (!permanentHideSet && permanentObjectsToHide != null)
        {
            foreach (var g in permanentObjectsToHide) if (g != null) g.SetActive(false);
            permanentHideSet = true;
        }

        alreadyToggled = true;

        if (resetAfterSeconds > 0f)
            StartCoroutine(ResetToggleAfterSeconds(resetAfterSeconds));

        OnBlowEvent?.Invoke();
    }

    private System.Collections.IEnumerator ResetToggleAfterSeconds(float secs)
    {
        yield return new WaitForSeconds(secs);

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
        // FIX: Gebruik micDevice
        if (!string.IsNullOrEmpty(micDevice) && Microphone.IsRecording(micDevice))
        {
            Microphone.End(micDevice);
        }
    }
}