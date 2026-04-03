using TMPro;
using UnityEngine;

public class EindLevelScript : MonoBehaviour
{
    public TMP_Text FellicitatieText;
    public GameProgressController gameProgressController;

    private const int LEVEL_NUMBER = 5;

    private async void OnEnable()
    {
        string kindNaam = PlayerPrefs.GetString("kindNaam", "");
        FellicitatieText.text = $"Gefeliciteerd {kindNaam}, je hebt alle levels gehaald!";

        var gameProgress = await gameProgressController.GetOrCreate(0f, LEVEL_NUMBER, LEVEL_NUMBER);
        if (gameProgress != null && gameProgress.LevelProgress < 1f)
        {
            gameProgress.LevelProgress = 1f;
            await gameProgressController.UpdateItem(gameProgress.GameProgressID, gameProgress);
        }
    }
}
