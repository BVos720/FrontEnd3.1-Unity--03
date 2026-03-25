using UnityEngine;

public class WereldSelectScript : MonoBehaviour
{
    public GameObject worldObject1;
    public GameObject worldObject2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Hello. I've started!");
    }

    
    public void MoveWorlds()
    {
        worldObject1.SetActive(true);
        worldObject2.SetActive(false);
    }
}
