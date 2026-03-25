using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
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
        public SettingsApiClient settingsApiClient;

        private Guid? _settingsID = null;

        private async void OnEnable()
        {
            characterDropdown.onValueChanged.RemoveAllListeners();
            characterDropdown.onValueChanged.AddListener(OnCharacterChanged);

            colorBlindDropdown.onValueChanged.RemoveAllListeners();
            colorBlindDropdown.onValueChanged.AddListener(OnColorBlindChanged);

            // Probeer settings te laden vanuit de backend
            if (settingsApiClient != null)
            {
                IWebRequestReponse response = await settingsApiClient.GetSettings();
                switch (response)
                {
                    case WebRequestData<string> dataResponse:
                        SettingsData loaded = JsonConvert.DeserializeObject<SettingsData>(dataResponse.Data);
                        if (loaded != null)
                        {
                            _settingsID = loaded.SettingsID;
                            PlayerPrefs.SetInt("SelectedCharacter", loaded.Character);
                            PlayerPrefs.SetInt("ColorBlindSetting", loaded.ColorTheme);
                            PlayerPrefs.Save();
                        }
                        break;
                    case WebRequestError errorResponse:
                        Debug.LogWarning("Kon settings niet laden: " + errorResponse.ErrorMessage);
                        break;
                }
            }

            int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);
            characterDropdown.value = selected;
            OnCharacterChanged(selected);

            int colorBlindSelected = PlayerPrefs.GetInt("ColorBlindSetting", 0);
            colorBlindDropdown.value = colorBlindSelected;
            OnColorBlindChanged(colorBlindSelected);
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

            _ = SaveSettings();
        }

        private void OnColorBlindChanged(int index)
        {
            PlayerPrefs.SetInt("ColorBlindSetting", index);
            PlayerPrefs.Save();

            if (colorBlindImage != null)
                colorBlindImage.gameObject.SetActive(index == 1);

            _ = SaveSettings();
        }

        private async Awaitable SaveSettings()
        {
            if (settingsApiClient == null || !_settingsID.HasValue) return;

            string kindIDString = PlayerPrefs.GetString("kindID", "");
            if (!Guid.TryParse(kindIDString, out Guid kindID)) return;

            SettingsData settingsData = new SettingsData(characterDropdown.value, colorBlindDropdown.value)
            {
                SettingsID = _settingsID.Value,
                KindID = kindID
            };

            IWebRequestReponse webRequestResponse = await settingsApiClient.UpdateItem(_settingsID.Value, settingsData);

            switch (webRequestResponse)
            {
                case WebRequestData<string> dataResponse:
                    Debug.Log("Update settings success");
                    break;
                case WebRequestError errorResponse:
                    Debug.Log("Update settings error: " + errorResponse.ErrorMessage);
                    break;
                default:
                    throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
            }
        }

        private void SetAlpha(UnityEngine.UI.RawImage img, float alpha)
        {
            var c = img.color;
            c.a = alpha;
            img.color = c;
        }

        public void GoBack()
        {
            string previousScene = PlayerPrefs.GetString("PreviousScene", "");
            if (!string.IsNullOrEmpty(previousScene))
                SceneManager.LoadScene(previousScene);
            else
                SceneManager.LoadScene(0);
        }
    }
}
