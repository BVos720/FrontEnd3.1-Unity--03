using MySecureBackend.WebApi.Models;
using TMPro;
using UnityEngine;

public class CreateParentScript : MonoBehaviour
{
    public OuderController ouderController;
    public GameObject OuderRegistratieScherm;
    public GameObject KindRegistratieScherm;
    public TMP_InputField OudernaamInput;

    public async void CreateOuder()
    {
        Ouder ouder = await ouderController.Create(OudernaamInput.text);
        if (ouder != null)
        {
            OuderRegistratieScherm.SetActive(false);
            KindRegistratieScherm.SetActive(true);
        }
    }
}
