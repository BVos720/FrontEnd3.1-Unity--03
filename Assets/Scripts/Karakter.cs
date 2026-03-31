using UnityEngine;

namespace Assets.Scripts
{
    public class Karakter : MonoBehaviour
    {
        public GameObject ballo;
        public GameObject willie;
        public UnityEngine.UI.RawImage balloUI;
        public UnityEngine.UI.RawImage willieUI;

        private void Start()
        {
            int selected = PlayerPrefs.GetInt("SelectedCharacter", 0);
            if (selected == 0)
                SetBallo();
            else if (selected == 1)
                SetWillie();
        }

        // Activate Ballo and deactivate Willie
        public void SetBallo()
        {
            if (ballo != null) ballo.SetActive(true);
            if (willie != null) willie.SetActive(false);

            SetAlpha(balloUI, 1f);
            SetAlpha(willieUI, 0.59f);

            PlayerPrefs.SetInt("SelectedCharacter", 0);
            PlayerPrefs.Save();
        }

        // Activate Willie and deactivate Ballo
        public void SetWillie()
        {
            if (ballo != null) ballo.SetActive(false);
            if (willie != null) willie.SetActive(true);

            SetAlpha(balloUI, 0.59f);
            SetAlpha(willieUI, 1f);

            PlayerPrefs.SetInt("SelectedCharacter", 1);
            PlayerPrefs.Save();
        }

        private void SetAlpha(UnityEngine.UI.RawImage img, float alpha)
        {
            if (img == null) return;

            // Ensure the image is active so it can be seen
            img.gameObject.SetActive(true);

            var c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }
}
