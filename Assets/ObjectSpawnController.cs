using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnRequest
{
    public GameObject prefab;
    public Vector3 position;
    public Quaternion quaternion;

    public SpawnRequest(GameObject prefab, Vector3 position, Quaternion quaternion)
    {
        this.prefab = prefab;
        this.position = position;
        this.quaternion = quaternion;
    }
}

public class ObjectSpawnController : MonoBehaviour
{
    public static ObjectSpawnController GetController()
    {
        GameObject objectSpawnController = GameObject.Find("ObjectSpawnController");
        return objectSpawnController.GetComponent<ObjectSpawnController>();
    }

    public List<SpawnRequest> spawnRequests = new List<SpawnRequest>();

    void Update()
    {
        if (spawnRequests.Count == 0)
        {
            return;
        }

        SpawnRequest nextRequest = spawnRequests[0];
        spawnRequests = spawnRequests.Skip(1).ToList();
        SpawnObject(nextRequest);
    }

    public void MakeRequest(GameObject prefab, Vector3 position, Quaternion quaternion)
    {
        SpawnRequest request = new SpawnRequest(prefab, position, quaternion);
        spawnRequests.Add(request);
    }

    void SpawnObject(SpawnRequest request)
    {
        Instantiate(request.prefab, request.position, request.quaternion);
    }

    public GameObject SpawnObjectImmediately(GameObject prefab, Vector3 position, Quaternion quaternion)
    {
        return Instantiate(prefab, position, quaternion);
    }
}
