using UnityEngine;


public class SimpleTriggerTest : MonoBehaviour
{
    public GhostRaceManager gameController;
    public GameObject playerCar;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Объект {other.gameObject.name} пересёк триггер.");

        if (other.transform.root.gameObject == playerCar)
        {
            Debug.Log("Игрок пересёк финишный триггер.");
            gameController.OnPlayerFinish();
        }
        else
        {
            Debug.Log($"В триггер вошёл объект с тегом: {other.gameObject.tag}");
        }
    }


}
