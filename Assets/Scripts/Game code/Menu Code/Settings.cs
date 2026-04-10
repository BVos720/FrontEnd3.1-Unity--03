using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MySecureBackend.WebApi.Models;
using UnityEngine.Localization.Settings;

namespace Assets.Scripts
{
    public class Settings : MonoBehaviour
    {
        [Header("Scenes")]
        public GameObject SettingsScreen;
        public GameObject OverzichtMenu;
        public GameObject LevelOverzicht;

        [Header("Dropdowns")]
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private TMP_Dropdown colorBlindDropdown;

        [Header("Toggles")]
        [SerializeField] private Toggle dyslexieToggle;

        [Header("Images")]
        [SerializeField] private UnityEngine.UI.RawImage ballo;
        [SerializeField] private UnityEngine.UI.RawImage willie;
        [SerializeField] private UnityEngine.UI.RawImage colorBlindImage;

        [Header("Controllers")]
        public SettingsController settingsController;

        [Header("Other")]
        private Guid? _settingsID = null;
        private bool _isLoading = false;
        public string LoadedFromScene { get; set; }

        private async void OnEnable()
        {
            _isLoading = true;

            if (colorBlindDropdown != null)
            {
                colorBlindDropdown.onValueChanged.RemoveAllListeners();
                colorBlindDropdown.onValueChanged.AddListener(OnColorBlindChanged);
            }

            if (languageDropdown != null)
            {
                languageDropdown.onValueChanged.RemoveAllListeners();
                languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
            }

            if (dyslexieToggle != null)
            {
                dyslexieToggle.onValueChanged.RemoveAllListeners();
                dyslexieToggle.onValueChanged.AddListener(OnDyslexieChanged);
            }

            if (settingsController != null)
            {
                SettingsData loaded = await settingsController.GetSettings();
                if (loaded != null)
                {
                    _settingsID = loaded.SettingsID;
                    PlayerPrefs.SetInt("SelectedCharacter", loaded.Character);
                    PlayerPrefs.SetInt("ColorBlindSetting", loaded.ColorTheme);
                    PlayerPrefs.SetInt("SelectedLanguage", loaded.Taal);
                    PlayerPrefs.SetInt("DyslexieSetting", loaded.Dyslexie ? 1 : 0);
                    if (loaded.KindID != Guid.Empty)
                        PlayerPrefs.SetString("kindID", loaded.KindID.ToString());
                    PlayerPrefs.Save();

                    // Sync managers met de zojuist geladen backend-instellingen
                    ColorBlindnessManager.Instance?.RefreshFromPrefs();
                    DyslexiaManager.Instance?.RefreshFromPrefs();
                }
            }

            // Initialize character UI transparency based on current selection
            int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);
            if (selected == 0)
            {
                SetAlpha(ballo, 1f);
                SetAlpha(willie, 0.59f);
            }
            else if (selected == 1)
            {
                SetAlpha(ballo, 0.59f);
                SetAlpha(willie, 1f);
            }

            if (colorBlindDropdown != null)
            {
                int colorBlindSelected = PlayerPrefs.GetInt("ColorBlindSetting", 0);
                colorBlindDropdown.value = colorBlindSelected;
                OnColorBlindChanged(colorBlindSelected);
            }

            if (languageDropdown != null)
            {
                int languageSelected = PlayerPrefs.GetInt("SelectedLanguage", 0);
                languageDropdown.value = languageSelected;
                OnLanguageChanged(languageSelected);
            }

            if (dyslexieToggle != null)
            {
                bool dyslexieSelected = PlayerPrefs.GetInt("DyslexieSetting", 0) == 1;
                dyslexieToggle.isOn = dyslexieSelected;
            }

            _isLoading = false;
        }

        private void OnLanguageChanged(int index)
        {
            // Volgorde: 0 = Nederlands, 1 = Engels, 2 = Duits
            string[] localeCodes = { "nl", "en", "de" };
            if (index >= 0 && index < localeCodes.Length)
            {
                var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCodes[index]);
                if (locale != null)
                {
                    LocalizationSettings.SelectedLocale = locale;
                }
            }
            PlayerPrefs.SetInt("SelectedLanguage", index);
            PlayerPrefs.Save();

            if (!_isLoading) _ = SaveSettings();
        }

        private void OnColorBlindChanged(int index)
        {
            PlayerPrefs.SetInt("ColorBlindSetting", index);
            PlayerPrefs.Save();

            // Pas het post-processing kleurenblindheid filter toe
            // 0 = Normaal | 1 = Deuteranopie | 2 = Protanopie | 3 = Tritanopie
            ColorBlindnessManager.Instance?.ApplyMode(index);

            if (!_isLoading) _ = SaveSettings();
        }

        private void OnDyslexieChanged(bool isOn)
        {
            PlayerPrefs.SetInt("DyslexieSetting", isOn ? 1 : 0);
            PlayerPrefs.Save();

            DyslexiaManager.Instance?.ApplyMode(isOn);

            if (!_isLoading) _ = SaveSettings();
        }

        private async Awaitable SaveSettings()
        {
            if (settingsController == null || !_settingsID.HasValue) return;

            string kindIDString = PlayerPrefs.GetString("kindID", "");
            if (!Guid.TryParse(kindIDString, out Guid kindID)) return;

            int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
            int colorBlindSetting = colorBlindDropdown != null ? colorBlindDropdown.value : 0;
            int taalSetting = languageDropdown != null ? languageDropdown.value : 0;
            bool dyslexieSetting = dyslexieToggle != null ? dyslexieToggle.isOn : PlayerPrefs.GetInt("DyslexieSetting", 0) == 1;

            SettingsData settingsData = new SettingsData(selectedCharacter, colorBlindSetting, taalSetting, dyslexieSetting)
            {
                SettingsID = _settingsID.Value,
                KindID = kindID
            };

            await settingsController.UpdateItem(_settingsID.Value, settingsData);
        }

        private void SetAlpha(UnityEngine.UI.RawImage img, float alpha)
        {
            if (img == null) return;
            var c = img.color;
            c.a = alpha;
            img.color = c;
        }

        public async void GoBack()
        {
            await SaveSettings();

            switch (LoadedFromScene)
            {
                case "OverzichtMenu":
                    SettingsScreen.SetActive(false);
                    OverzichtMenu.SetActive(true);
                    break;

                case "LevelOverZicht":
                    SettingsScreen.SetActive(false);
                    LevelOverzicht.SetActive(true);
                    break;
                default:
                    SettingsScreen.SetActive(false);
                    OverzichtMenu.SetActive(true);
                    break;
            }


        }
    }
}
