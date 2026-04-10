using UnityEngine;
using TMPro;

/// <summary>
/// Voeg dit component toe aan elk TMP_Text element dat dyslexie-vriendelijk font moet krijgen.
/// Het font wordt automatisch gewisseld zodra DyslexiaManager de modus wijzigt.
/// Het OpenDyslexic font hoef je maar één keer in te stellen — op de DyslexiaManager.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class DyslexiaTextAdapter : MonoBehaviour
{
    [Tooltip("Aanpassing van de lettergrootte bij dyslexisch font. 0.8 = 80% van de originele grootte.")]
    [SerializeField] private float fontSizeMultiplier = 0.8f;

    private TMP_Text _text;
    private TMP_FontAsset _originalFont;
    private float _originalFontSize;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        if (_text != null)
        {
            _originalFont = _text.font;
            _originalFontSize = _text.fontSize;
        }
    }

    private void OnEnable()
    {
        DyslexiaManager.OnDyslexiaModeChanged += ApplyFont;

        // Direct de huidige staat toepassen (bijv. na scene-wissel)
        bool huidigeModus = PlayerPrefs.GetInt("DyslexieSetting", 0) == 1;
        ApplyFont(huidigeModus);
    }

    private void OnDisable()
    {
        DyslexiaManager.OnDyslexiaModeChanged -= ApplyFont;
    }

    private void ApplyFont(bool dyslexia)
    {
        if (_text == null) return;

        var dyslexicFont = DyslexiaManager.Instance?.dyslexicFont;

        if (dyslexia && dyslexicFont != null)
        {
            _text.font = dyslexicFont;
            _text.fontSize = _originalFontSize * fontSizeMultiplier;
        }
        else
        {
            if (_originalFont != null)
                _text.font = _originalFont;
            _text.fontSize = _originalFontSize;
        }
    }
}
