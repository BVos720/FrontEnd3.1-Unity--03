using UnityEngine;

namespace Assets.Scripts
{
    public class Karakter : MonoBehaviour
    {
        public GameObject ballo;
        public GameObject willie;

        private void Start()
        {
            SetActiveKarakter();
        }

        // Haal de gekozen karakter op (0 = Ballo, 1 = Willie, 2 = Andere)
        public static int GetSelected()
        {
            return PlayerPrefs.GetInt("SelectedCharacter", 0);
        }

        // Zet alleen het gekozen karakter GameObject actief
        public void SetActiveKarakter()
        {
            int selected = GetSelected();
            if (selected == 0)
            {
                if (ballo != null) ballo.SetActive(true);
                if (willie != null) willie.SetActive(false);
            }
            else if (selected == 1)
            {
                if (ballo != null) ballo.SetActive(false);
                if (willie != null) willie.SetActive(true);
            }
            else // Andere
            {
                if (ballo != null) ballo.SetActive(true);
                if (willie != null) willie.SetActive(true);
            }
        }
    }
}
