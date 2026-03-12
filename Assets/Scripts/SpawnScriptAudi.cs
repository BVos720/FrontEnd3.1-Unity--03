using UnityEngine;

public class SpawnScriptAudi : MonoBehaviour
{
    public GameObject AudiQuatro;
    public int prefabID;

    public void Spawn()
    {
        GameObject gespawnd = Instantiate(AudiQuatro, Vector3.zero, Quaternion.identity, WorldManager.Instance.World.transform);

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
        //Object word aan een lijst toegevoegd zodat de objecten op een plek te managen zijn
        WorldManager.Instance.RegistreerObject(gespawnd);

        
        Draggable draggable = gespawnd.GetComponent<Draggable>();
        if (draggable != null)
        {
            draggable.objectData = newObject;
            draggable.apiClient = WorldManager.Instance.object2DApiClient;
            draggable.CreateObject2D();
        }

        Debug.Log("Audi gespawned");
    }
}
