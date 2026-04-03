using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public static class PlaceholderReplacer
{
    public static void ReplaceKindNaam(string kindNaam)
    {
        TextMeshProUGUI[] allTextElements = Object.FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI textElement in allTextElements)
        {
            if (textElement.text.IndexOf("KindNaamPlaceholder", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                textElement.text = Regex.Replace(
                    textElement.text, "KindNaamPlaceholder", kindNaam, RegexOptions.IgnoreCase);
                Debug.Log($"Placeholder vervangen in: {textElement.gameObject.name} \u2192 '{textElement.text}'");
            }
        }
    }
}
