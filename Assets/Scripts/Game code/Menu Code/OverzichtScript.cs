using Assets.Scripts;
using MySecureBackend.WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;

public class LevelOverzichtScript : MonoBehaviour
{
    public GameObject OverzichtScherm;
    public GameObject LoginScherm;
    public TMP_Dropdown KinderSelectDropdown;
    public Transform saveScrollContent;
    public GameObject SettingScherm;

    public GameObject LevelOverzichtScherm;
    public GameObject GameTheme;
    public Sprite behandelingButtonSprite;
    public TMPro.TMP_FontAsset behandelingButtonFont;
    public BehandelingController behandelingController;
    public KindController kindController;
    public GameProgressController gameProgressController;
    public SettingsController settingsController;
    public GameObject colorBlindOverlay;

    private List<Kind> kinderen;
    private List<Behandeling> behandelingen;
    private List<GameProgress> gameProgresses;

    private async void OnEnable()
    {
        GameTheme.SetActive(true);
        var karaktersVoorLoad = FindObjectsOfType<Karakter>(true);
        Debug.Log($"[Overzicht] Karakter componenten gevonden (voor load): {karaktersVoorLoad.Length}, SelectedCharacter in PlayerPrefs: {PlayerPrefs.GetInt("SelectedCharacter", -1)}");
        foreach (var karakter in karaktersVoorLoad)
        {
            int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);
            if (selected == 0)
                karakter.SetBallo();
            else if (selected == 1)
                karakter.SetWillie();
        }

        Debug.Log($"[Overzicht] colorBlindOverlay assigned: {colorBlindOverlay != null}, ColorBlindSetting in PlayerPrefs: {PlayerPrefs.GetInt("ColorBlindSetting", -1)}");
        if (colorBlindOverlay != null)
            colorBlindOverlay.SetActive(PlayerPrefs.GetInt("ColorBlindSetting", 0) == 1);

        if (settingsController != null)
        {
            SettingsData loaded = await settingsController.GetSettings();
            if (loaded != null)
            {
                Debug.Log($"[Overzicht] Settings geladen - Character: {loaded.Character}, ColorTheme: {loaded.ColorTheme}, Taal: {loaded.Taal}");
                PlayerPrefs.SetInt("SelectedCharacter", loaded.Character);
                PlayerPrefs.SetInt("ColorBlindSetting", loaded.ColorTheme);
                PlayerPrefs.SetInt("SelectedLanguage", loaded.Taal);
                if (loaded.KindID != System.Guid.Empty)
                    PlayerPrefs.SetString("kindID", loaded.KindID.ToString());
                PlayerPrefs.Save();

                string[] localeCodes = { "nl", "en", "de" };
                if (loaded.Taal >= 0 && loaded.Taal < localeCodes.Length)
                {
                    var locale = UnityEngine.Localization.Settings.LocalizationSettings.AvailableLocales.GetLocale(localeCodes[loaded.Taal]);
                    if (locale != null)
                        UnityEngine.Localization.Settings.LocalizationSettings.SelectedLocale = locale;
                }

                var karaktersNaLoad = FindObjectsOfType<Karakter>(true);
                Debug.Log($"[Overzicht] Karakter componenten gevonden (na load): {karaktersNaLoad.Length}");
                foreach (var karakter in karaktersNaLoad)
                {
                    if (loaded.Character == 0)
                        karakter.SetBallo();
                    else if (loaded.Character == 1)
                        karakter.SetWillie();
                }

                if (colorBlindOverlay != null)
                    colorBlindOverlay.SetActive(loaded.ColorTheme == 1);
            }
            else
            {
                Debug.LogWarning("[Overzicht] Settings laden mislukt");
            }
        }
        else
        {
            Debug.LogWarning("[Overzicht] settingsController is null!");
        }

        KinderSelectDropdown.onValueChanged.RemoveAllListeners();

        kinderen = await kindController.GetAll();
        if (kinderen != null)
        {
            KinderSelectDropdown.ClearOptions();
            KinderSelectDropdown.AddOptions(kinderen.Select(k => k.Naam).ToList());
            KinderSelectDropdown.onValueChanged.AddListener(async (index) => await OnKindChanged(index));
            if (kinderen.Count > 0)
                await OnKindChanged(0);
        }
    }

    private void ClearSaveContent()
    {
        foreach (Transform child in saveScrollContent)
            Destroy(child.gameObject);
    }

    private async Awaitable OnKindChanged(int index)
    {
        Kind kind = kinderen[index];
        PlayerPrefs.SetString("kindID", kind.KindID.ToString());
        PlayerPrefs.SetString("kindNaam", kind.Naam);
        PlayerPrefs.Save();

        // Vervang alle KindNaamPlaceholder met de werkelijke naam
        PlaceholderReplacer.ReplaceKindNaam(kind.Naam);

        ClearSaveContent();

        List<Behandeling> alleBehandelingen = await behandelingController.GetAll();
        if (alleBehandelingen != null)
        {
            behandelingen = alleBehandelingen.Where(b => b.KindID == kind.KindID).ToList();
            if (behandelingen.Count > 0)
            {
                foreach (Behandeling behandeling in behandelingen)
                {
                    Behandeling captured = behandeling;
                    GameObject entry = new GameObject("BehandelingEntry");
                    entry.transform.SetParent(saveScrollContent, false);

                    RectTransform entryRect = entry.AddComponent<RectTransform>();
                    entryRect.anchorMin = new Vector2(0f, 1f);
                    entryRect.anchorMax = new Vector2(1f, 1f);
                    entryRect.pivot = new Vector2(0.5f, 1f);
                    entryRect.sizeDelta = new Vector2(0f, 30f);

                    UnityEngine.UI.Image bg = entry.AddComponent<UnityEngine.UI.Image>();
                    if (behandelingButtonSprite != null)
                        bg.sprite = behandelingButtonSprite;
                    else
                        bg.color = new Color(1f, 1f, 1f, 0.85f);

                    UnityEngine.UI.Button button = entry.AddComponent<UnityEngine.UI.Button>();
                    button.targetGraphic = bg;
                    var colors = button.colors;
                    colors.highlightedColor = new Color(0.85f, 0.95f, 1f, 1f);
                    colors.pressedColor = new Color(0.7f, 0.85f, 1f, 1f);
                    button.colors = colors;
                    button.onClick.AddListener(() => OnBehandelingSelected(captured));

                    GameObject textObj = new GameObject("Text");
                    textObj.transform.SetParent(entry.transform, false);
                    TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
                    text.text = behandeling.Type;
                    text.enableAutoSizing = true;
                    text.fontSizeMin = 8f;
                    text.fontSizeMax = 16f;
                    text.fontStyle = TMPro.FontStyles.Bold;
                    text.overflowMode = TMPro.TextOverflowModes.Ellipsis;
                    text.color = Color.black;
                    text.alignment = TMPro.TextAlignmentOptions.Midline;
                    if (behandelingButtonFont != null)
                        text.font = behandelingButtonFont;

                    RectTransform textRect = textObj.GetComponent<RectTransform>();
                    textRect.anchorMin = Vector2.zero;
                    textRect.anchorMax = Vector2.one;
                    textRect.offsetMin = new Vector2(10f, 0f);
                    textRect.offsetMax = new Vector2(-10f, 0f);
                }
                Debug.Log("Behandelingen geladen: " + behandelingen.Count);
            }
            else
            {
                Debug.Log("Geen behandelingen gevonden voor dit kind.");
            }
        }
    }

    private void OnBehandelingSelected(Behandeling behandeling)
    {
        PlayerPrefs.SetString("behandelingID", behandeling.BehandelingID.ToString());
        PlayerPrefs.Save();

        if (LevelOverzichtScherm != null)
            LevelOverzichtScherm.SetActive(true);
        if (OverzichtScherm != null)
            OverzichtScherm.SetActive(false);
    }

    public void Logout()
    {
        OverzichtScherm.SetActive(false);
        LoginScherm.SetActive(true);
    }

    public void OpenSettings()
    {
        SettingScherm.SetActive(true);
        var settings = SettingScherm.GetComponentInChildren<Assets.Scripts.Settings>();
        if (settings != null) settings.LoadedFromScene = "OverzichtMenu";
        OverzichtScherm.SetActive(false);
    }
}