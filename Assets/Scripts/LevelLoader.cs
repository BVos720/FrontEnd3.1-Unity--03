using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public GameObject LevelOverzicht;
    public GameObject ScenesMetMuziek;
    public GameObject ScenesZonderMuziek;
    public GameObject Level1;
    public GameObject Level2;
    public GameObject Level3;
    public GameObject Level4;
    

    public void LoadLevel1()
    {
        DeactivateAllLevels();
        if (Level1 != null) Level1.SetActive(true);
        Debug.Log("Level 1 activated");
    }

    public void LoadLevel2()
    {
        DeactivateAllLevels();
        if (Level2 != null) Level2.SetActive(true);
        Debug.Log("Level 2 activated");
    }

    public void LoadLevel3()
    {
        DeactivateAllLevels();
        if (Level3 != null) Level3.SetActive(true);
        Debug.Log("Level 3 activated");
    }

    public void LoadLevel4()
    {
        DeactivateAllLevels();
        if (Level4 != null) Level4.SetActive(true);
        Debug.Log("Level 4 activated");
    }

    private void DeactivateAllLevels()
    {
        if (LevelOverzicht != null) LevelOverzicht.SetActive(false);
        if (ScenesMetMuziek != null) ScenesMetMuziek.SetActive(false);
        if (ScenesZonderMuziek != null) ScenesZonderMuziek.SetActive(true);
        if (Level1 != null) Level1.SetActive(false);
        if (Level2 != null) Level2.SetActive(false);
        if (Level3 != null) Level3.SetActive(false);
        if (Level4 != null) Level4.SetActive(false);
    }
}
