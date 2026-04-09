using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorBlindnessManager : MonoBehaviour
{
    public static ColorBlindnessManager Instance { get; private set; }

    [Header("Sleep hier je ColorBlindnessMat in!")]
    public Material filterMaterial;

    private static readonly Vector3 Normal_R = new Vector3(1f, 0f, 0f);
    private static readonly Vector3 Normal_G = new Vector3(0f, 1f, 0f);
    private static readonly Vector3 Normal_B = new Vector3(0f, 0f, 1f);

    // =========================================================
    // CORRECTIE-MATRICES (Geen simulatie!)
    // Verschuif onzichtbare kleuren naar het zichtbare spectrum.
    // =========================================================

    // Deuteranopie (Groen is lastig): Groen -> Cyaan (voeg toe aan Blauw)
    private static readonly Vector3 Deuteranopia_R = new Vector3(1f, 0f, 0f);
    private static readonly Vector3 Deuteranopia_G = new Vector3(0f, 1f, 0f);
    private static readonly Vector3 Deuteranopia_B = new Vector3(0f, 0.8f, 1f);

    // Protanopie (Rood is lastig): Rood -> Magenta (voeg toe aan Blauw)
    private static readonly Vector3 Protanopia_R = new Vector3(1f, 0f, 0f);
    private static readonly Vector3 Protanopia_G = new Vector3(0f, 1f, 0f);
    private static readonly Vector3 Protanopia_B = new Vector3(0.8f, 0f, 1f);

    // Tritanopie (Blauw is lastig): Blauw -> Rood/Roze (voeg Blauw toe aan Rood)
    private static readonly Vector3 Tritanopia_R = new Vector3(1f, 0f, 0.8f);
    private static readonly Vector3 Tritanopia_G = new Vector3(0f, 1f, 0f);
    private static readonly Vector3 Tritanopia_B = new Vector3(0f, 0f, 1f);

    private int _currentMode;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Zorg dat we starten met Normaal totdat we de Prefs checken via een login-actie
        if (filterMaterial != null)
        {
            ApplyMode(0);
        }
    }

    public void ApplyMode(int mode)
    {
        _currentMode = mode;

        if (filterMaterial == null) return;

        Vector3 rRow, gRow, bRow;

        switch (mode)
        {
            case 1: // Deuteranopie
                Debug.Log("[ColorBlindnessManager] Filter gezet naar Deuteranopie (1)");
                rRow = Deuteranopia_R;
                gRow = Deuteranopia_G;
                bRow = Deuteranopia_B;
                break;
            case 2: // Protanopie
                Debug.Log("[ColorBlindnessManager] Filter gezet naar Protanopie (2)");
                rRow = Protanopia_R;
                gRow = Protanopia_G;
                bRow = Protanopia_B;
                break;
            case 3: // Tritanopie
                Debug.Log("[ColorBlindnessManager] Filter gezet naar Tritanopie (3)");
                rRow = Tritanopia_R;
                gRow = Tritanopia_G;
                bRow = Tritanopia_B;
                break;
            default: // Normaal
                Debug.Log("[ColorBlindnessManager] Filter gezet naar UIT (0)");
                rRow = Normal_R;
                gRow = Normal_G;
                bRow = Normal_B;
                break;
        }

        filterMaterial.SetVector("_RRow", rRow);
        filterMaterial.SetVector("_GRow", gRow);
        filterMaterial.SetVector("_BRow", bRow);
    }

    public void RefreshFromPrefs()
    {
        _currentMode = PlayerPrefs.GetInt("ColorBlindSetting", 0);
        ApplyMode(_currentMode);
    }
}
