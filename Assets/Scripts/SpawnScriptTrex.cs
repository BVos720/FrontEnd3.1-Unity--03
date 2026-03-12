using UnityEngine;

public class SpawnScriptTrex : MonoBehaviour
{
    public GameObject Trex;
    public int prefabID;

    public void SpawnVbuck()
    {
        GameObject gespawnd = Instantiate(Trex, Vector3.zero, Quaternion.identity, WorldManager.Instance.World.transform);

        Object2D newObject = new Object2D
        {
            EnviromentGUID = WorldManager.Instance.HuidigeWereldId,
            PrefabID = prefabID,
            PositionX = 0,
            PositionY = 0,
            //De goede schaal van het object word meegegeven voorheen waren objececten niet de juiste schaal
            ScaleX = gespawnd.transform.localScale.x,
            ScaleY = gespawnd.transform.localScale.y,
            RotationZ = 0,
            SortingLayer = 1
        };
        //Objecten worden toegevoegd aan een lijst zodat deze makkelijker te managen zijn 
        WorldManager.Instance.RegistreerObject(gespawnd);

        Draggable draggable = gespawnd.GetComponent<Draggable>();
        if (draggable != null)
        {
            draggable.objectData = newObject;
            draggable.apiClient = WorldManager.Instance.object2DApiClient;
            draggable.CreateObject2D();
        }

        Debug.Log("Trex gespawned");
    }
}
