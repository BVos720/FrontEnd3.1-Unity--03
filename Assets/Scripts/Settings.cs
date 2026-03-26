using System;
using UnityEngine;
using TMPro;
using MySecureBackend.WebApi.Models;

namespace Assets.Scripts
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown characterDropdown;
        [SerializeField] private UnityEngine.UI.RawImage ballo;
        [SerializeField] private UnityEngine.UI.RawImage willie;
        [SerializeField] private TMP_Dropdown colorBlindDropdown;
        [SerializeField] private UnityEngine.UI.RawImage colorBlindImage;
        public SettingsController settingsController;
        public GameObject SettingsScreen;
        public GameObject OverzichtMenu;

        private Guid? _settingsID = null;
        private bool _isLoading = false;

        private async void OnEnable()
        {
            _isLoading = true;

            characterDropdown.onValueChanged.RemoveAllListeners();
            characterDropdown.onValueChanged.AddListener(OnCharacterChanged);

            colorBlindDropdown.onValueChanged.RemoveAllListeners();
            colorBlindDropdown.onValueChanged.AddListener(OnColorBlindChanged);

            if (settingsController != null)
            {
                SettingsData loaded = await settingsController.GetSettings();
                if (loaded != null)
                {
                    _settingsID = loaded.SettingsID;
                    PlayerPrefs.SetInt("SelectedCharacter", loaded.Character);
                    PlayerPrefs.SetInt("ColorBlindSetting", loaded.ColorTheme);
                    if (loaded.KindID != Guid.Empty)
                        PlayerPrefs.SetString("kindID", loaded.KindID.ToString());
                    PlayerPrefs.Save();
                }
            }

            int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);
            characterDropdown.value = selected;
            OnCharacterChanged(selected);

            int colorBlindSelected = PlayerPrefs.GetInt("ColorBlindSetting", 0);
            colorBlindDropdown.value = colorBlindSelected;
            OnColorBlindChanged(colorBlindSelected);

            _isLoading = false;
        }

        private void OnCharacterChanged(int index)
        {
            if (ballo == null || willie == null) return;
            PlayerPrefs.SetInt("SelectedCharacter", index);
            PlayerPrefs.Save();

            if (index == 0) // Ballo
            {
                SetAlpha(ballo, 1f);
                SetAlpha(willie, 150f / 255f);
            }
            else if (index == 1) // Willie
            {
                SetAlpha(ballo, 150f / 255f);
                SetAlpha(willie, 1f);
            }
            else // Andere
            {
                SetAlpha(ballo, 150f / 255f);
                SetAlpha(willie, 150f / 255f);
            }

            foreach (var karakter in FindObjectsOfType<Karakter>())
                karakter.SetActiveKarakter();

            if (!_isLoading) _ = SaveSettings();
        }

        private void OnColorBlindChanged(int index)
        {
            PlayerPrefs.SetInt("ColorBlindSetting", index);
            PlayerPrefs.Save();

            if (colorBlindImage != null)
                colorBlindImage.gameObject.SetActive(index == 1);

            if (!_isLoading) _ = SaveSettings();
        }

        private async Awaitable SaveSettings()
        {
            if (settingsController == null || !_settingsID.HasValue) return;

            string kindIDString = PlayerPrefs.GetString("kindID", "");
            if (!Guid.TryParse(kindIDString, out Guid kindID)) return;

            SettingsData settingsData = new SettingsData(characterDropdown.value, colorBlindDropdown.value)
            {
                SettingsID = _settingsID.Value,
                KindID = kindID
            };

            await settingsController.UpdateItem(_settingsID.Value, settingsData);
        }

        private void SetAlpha(UnityEngine.UI.RawImage img, float alpha)
        {
            var c = img.color;
            c.a = alpha;
            img.color = c;
        }

        public async void GoBack()
        {
            await SaveSettings();
            SettingsScreen.SetActive(false);
            OverzichtMenu.SetActive(true);
        }
    }
}
