using UnityEngine;
using TMPro;
using System;

public class DyslexiaManager : MonoBehaviour
{
    public static DyslexiaManager Instance { get; private set; }

    // UI-elementen kunnen hierop abonneren om direct te weten wanneer dyslexie-modus wijzigt
    public static event Action<bool> OnDyslexiaModeChanged;

    [Header("Sleep hier het OpenDyslexic TMP font in!")]
    public TMP_FontAsset dyslexicFont;

    private bool _isEnabled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Start met uit totdat de prefs geladen worden via login
        _isEnabled = false;
    }

    public void ApplyMode(bool enabled)
    {
        _isEnabled = enabled;
        Debug.Log($"[DyslexiaManager] Dyslexie modus: {(enabled ? "AAN" : "UIT")}");
        OnDyslexiaModeChanged?.Invoke(_isEnabled);
    }

    public void RefreshFromPrefs()
    {
        bool enabled = PlayerPrefs.GetInt("DyslexieSetting", 0) == 1;
        ApplyMode(enabled);
    }
}
