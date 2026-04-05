using UnityEngine;

public class MuziekScript : MonoBehaviour
{
    public AudioSource GameTheme;
    private void OnEnable()
    {
        GameTheme.enabled = true;
    }

}
