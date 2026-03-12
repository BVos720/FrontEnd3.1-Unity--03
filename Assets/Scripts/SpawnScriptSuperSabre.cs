using UnityEngine;

public class SpawnScriptSuperSabre : MonoBehaviour
{
    public GameObject F100;
    public int prefabID;

    public void SpawnF100()
    {
        GameObject gespawnd = Instantiate(F100, Vector3.zero, Quaternion.identity, WorldManager.Instance.World.transform);

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
        //Object word toegevoegd aan een lijst zodat deze makkelijker te managen zijn 
        WorldManager.Instance.RegistreerObject(gespawnd);

        Draggable draggable = gespawnd.GetComponent<Draggable>();
        if (draggable != null)
        {
            draggable.objectData = newObject;
            draggable.apiClient = WorldManager.Instance.object2DApiClient;
            draggable.CreateObject2D();
        }

        Debug.Log("F100 gespawned");
    }
}
