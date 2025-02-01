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
        GameObject spawnedObject = Instantiate(request.prefab, request.position, request.quaternion);
        LaunchObject(spawnedObject);
    }

    private void LaunchObject(GameObject objectToLaunch)
    {
        Rigidbody objectRigidbody = objectToLaunch.GetComponent<Rigidbody>();

        if (objectRigidbody == null)
        {
            objectToLaunch.AddComponent<Rigidbody>();
            objectRigidbody = objectToLaunch.GetComponent<Rigidbody>();
            objectRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        MeshCollider collider = objectToLaunch.GetComponent<MeshCollider>();
        if (collider == null)
        {
            Mesh mesh = MeshUtility.GetMeshFromGameObject(objectToLaunch);
            if (mesh != null)
            {
                collider = objectToLaunch.AddComponent<MeshCollider>();
                collider.sharedMesh = mesh;
                collider.convex = true;
            }
        }

        if (objectRigidbody != null && collider != null)
        {
            float yAngle = Random.Range(0, 360);
            float xAngle = Random.Range(20, 60);
            float forceMagnitude = Random.Range(3, 7);

            float yRad = yAngle * Mathf.Deg2Rad;
            float xRad = xAngle * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(
                Mathf.Cos(yRad) * Mathf.Cos(xRad),
                Mathf.Sin(xRad),
                Mathf.Sin(yRad) * Mathf.Cos(xRad));

            objectRigidbody.AddForce(direction * forceMagnitude, ForceMode.Impulse);
        }
    }

    public GameObject SpawnObjectImmediately(GameObject prefab, Vector3 position, Quaternion quaternion)
    {
        return Instantiate(prefab, position, quaternion);
    }
}
