using Assets.Scripts;
using MySecureBackend.WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelOverzichtScript : MonoBehaviour
{
    public TMP_Text BalooText;
    public GameObject OverzichtScherm;
    public GameObject LoginScherm;
    public TMP_Dropdown KinderSelectDropdown;
    public Transform saveScrollContent;
    public GameObject SettingScherm;

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
        var karaktersVoorLoad = FindObjectsOfType<Karakter>(true);
        Debug.Log($"[Overzicht] Karakter componenten gevonden (voor load): {karaktersVoorLoad.Length}, SelectedCharacter in PlayerPrefs: {PlayerPrefs.GetInt("SelectedCharacter", -1)}");
        foreach (var karakter in karaktersVoorLoad)
            karakter.SetActiveKarakter();

        Debug.Log($"[Overzicht] colorBlindOverlay assigned: {colorBlindOverlay != null}, ColorBlindSetting in PlayerPrefs: {PlayerPrefs.GetInt("ColorBlindSetting", -1)}");
        if (colorBlindOverlay != null)
            colorBlindOverlay.SetActive(PlayerPrefs.GetInt("ColorBlindSetting", 0) == 1);

        if (settingsController != null)
        {
            SettingsData loaded = await settingsController.GetSettings();
            if (loaded != null)
            {
                Debug.Log($"[Overzicht] Settings geladen - Character: {loaded.Character}, ColorTheme: {loaded.ColorTheme}");
                PlayerPrefs.SetInt("SelectedCharacter", loaded.Character);
                PlayerPrefs.SetInt("ColorBlindSetting", loaded.ColorTheme);
                if (loaded.KindID != System.Guid.Empty)
                    PlayerPrefs.SetString("kindID", loaded.KindID.ToString());
                PlayerPrefs.Save();

                var karaktersNaLoad = FindObjectsOfType<Karakter>(true);
                Debug.Log($"[Overzicht] Karakter componenten gevonden (na load): {karaktersNaLoad.Length}");
                foreach (var karakter in karaktersNaLoad)
                    karakter.SetActiveKarakter();

                if (colorBlindOverlay != null)
                    colorBlindOverlay.SetActive(loaded.ColorTheme == 1);
            }
            else
            {
                Debug.LogWarning("[Overzicht] Deserialisatie van settings mislukt (null)");
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
        PlayerPrefs.Save();
        BalooText.text = $"Welkom, {kind.Naam}. Klaar om te leren?";
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

                    UnityEngine.UI.Button button = entry.AddComponent<UnityEngine.UI.Button>();
                    button.onClick.AddListener(async () => await OnBehandelingSelected(captured));

                    GameObject textObj = new GameObject("Text");
                    textObj.transform.SetParent(entry.transform, false);
                    TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
                    text.text = $"{behandeling.Type} - {behandeling.Datum:dd-MM-yyyy}";
                }
                Debug.Log("Behandelingen geladen: " + behandelingen.Count);
            }
            else
            {
                Debug.Log("Geen behandelingen gevonden voor dit kind.");
            }
        }
    }

    private async Awaitable OnBehandelingSelected(Behandeling behandeling)
    {
        ClearSaveContent();

        List<GameProgress> alleProgresses = await gameProgressController.GetAll();
        if (alleProgresses != null)
        {
            gameProgresses = alleProgresses.Where(g => g.BehandelingID == behandeling.BehandelingID).ToList();
            Debug.Log("Saves opgehaald voor behandeling: " + gameProgresses.Count);
            foreach (GameProgress gp in gameProgresses)
            {
                GameObject entry = new GameObject("SaveEntry");
                entry.transform.SetParent(saveScrollContent, false);
                TextMeshProUGUI saveText = entry.AddComponent<TextMeshProUGUI>();
                saveText.text = $"Punten: {gp.Points}\nVoortgang: {(gp.LevelProgress * 100f):F0}%";
            }
        }
    }

    public void Logout()
    {
        OverzichtScherm.SetActive(false);
        LoginScherm.SetActive(true);
    }

    public void OpenSettings()
    {
        SettingScherm.SetActive(true);
        OverzichtScherm.SetActive(false);
    }
}
