using UnityEngine;
using UnityEngine.UI;

public class InfoSwitch : MonoBehaviour
{
    [Header("Level 5")]
    public GameObject level5Object;
    public GameObject level5InfoObject;
    public Button naarInfo5Button;
    public Button terugNaarLevel5Button;

    [Header("Level 6")]
    public GameObject level6Object;
    public GameObject level6InfoObject;
    public Button naarInfo6Button;
    public Button terugNaarLevel6Button;

    private void Start()
    {
        if (naarInfo5Button != null)
            naarInfo5Button.onClick.AddListener(NaarInfo5);

        if (terugNaarLevel5Button != null)
            terugNaarLevel5Button.onClick.AddListener(TerugNaarLevel5);

        if (naarInfo6Button != null)
            naarInfo6Button.onClick.AddListener(NaarInfo6);

        if (terugNaarLevel6Button != null)
            terugNaarLevel6Button.onClick.AddListener(TerugNaarLevel6);
    }

    public void NaarInfo5()
    {
        if (level5Object != null)
            level5Object.SetActive(false);
        if (level5InfoObject != null)
            level5InfoObject.SetActive(true);
    }

    public void TerugNaarLevel5()
    {
        if (level5InfoObject != null)
            level5InfoObject.SetActive(false);
        if (level5Object != null)
            level5Object.SetActive(true);
    }

    public void NaarInfo6()
    {
        if (level6Object != null)
            level6Object.SetActive(false);
        if (level6InfoObject != null)
            level6InfoObject.SetActive(true);
    }

    public void TerugNaarLevel6()
    {
        if (level6InfoObject != null)
            level6InfoObject.SetActive(false);
        if (level6Object != null)
            level6Object.SetActive(true);
    }
}
