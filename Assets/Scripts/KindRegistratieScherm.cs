using MySecureBackend.WebApi.Models;
using System;
using TMPro;
using UnityEngine;

public class KindRegistratieScherm : MonoBehaviour
{
    public GameObject kindRegistratieScherm;
    public GameObject SelectBehandelingScherm;
    public KindController kindController;
    public TMP_InputField KindNaamInput;
    public TMP_InputField KindLeeftijdInput;

    public async void CreateKind()
    {
        Kind kind = await kindController.Create(KindNaamInput.text, Convert.ToInt32(KindLeeftijdInput.text));
        if (kind != null)
        {
            PlayerPrefs.SetString("kindID", kind.KindID.ToString());
            SelectBehandelingScherm.SetActive(true);
            kindRegistratieScherm.SetActive(false);
        }
    }
}
