using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject CreateWorld;
    public GameObject LoadWorld;
    public GameObject Login;
    public GameObject CreateUser;
    //Zorgt ervoor dat de juiste scenes bij de game start geladen worden
    void Start()
    {
        CreateUser.SetActive(false);
        CreateWorld.SetActive(false);
        LoadWorld.SetActive(false);
        Login.SetActive(true);
    }   

  
}
