using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using Assets.Scripts;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown characterDropdown;
        [SerializeField] private UnityEngine.UI.RawImage ballo;
        [SerializeField] private UnityEngine.UI.RawImage willie;
        [SerializeField] private TMP_Dropdown colorBlindDropdown;
        [SerializeField] private UnityEngine.UI.RawImage colorBlindImage;

        private void Start()
        {
            if (characterDropdown != null)
                characterDropdown.onValueChanged.AddListener(OnCharacterChanged);

            // Zet dropdown op opgeslagen keuze
            int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);
            characterDropdown.value = selected;
            OnCharacterChanged(selected);

            // Colorblind dropdown
            if (colorBlindDropdown != null)
                colorBlindDropdown.onValueChanged.AddListener(OnColorBlindChanged);

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
                SetAlpha(willie, 150f/255f);
            }
            else if (index == 1) // Willie
            {
                SetAlpha(ballo, 150f/255f);
                SetAlpha(willie, 1f);
            }
            else // Andere
            {
                SetAlpha(ballo, 150f/255f);
                SetAlpha(willie, 150f/255f);
            }

            // Update alle Karakter componenten in de scene
            foreach (var karakter in FindObjectsOfType<Karakter>())
            {
                karakter.SetActiveKarakter();
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
            {
                SceneManager.LoadScene(previousScene);
            }
            else
            {
                // Fallback: laad eerste scene als er geen vorige bekend is
                SceneManager.LoadScene(0);
            }
        }

        private void OnColorBlindChanged(int index)
        {
            PlayerPrefs.SetInt("ColorBlindSetting", index);
            PlayerPrefs.Save();
            if (colorBlindImage == null) return;

            if (index == 0)
            {
                colorBlindImage.gameObject.SetActive(false);
            }
            else if (index == 1)
            {
                colorBlindImage.gameObject.SetActive(true);
            }
            else
            {
                colorBlindImage.gameObject.SetActive(false);
            }
        }
    }
}
