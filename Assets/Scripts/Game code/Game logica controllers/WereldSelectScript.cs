using UnityEngine;

public class WereldSelectScript : MonoBehaviour
{
    public GameObject TargetWorld;
    public GameObject CurrentWorld;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Hello. I've started!");
    }

    
    public void WorldSelect()
    {
        TargetWorld.SetActive(true);
        CurrentWorld.SetActive(false);
    }
}
