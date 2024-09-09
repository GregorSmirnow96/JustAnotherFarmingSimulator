using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spawning;

public class PlayerController : MonoBehaviour
{
    public float speed = 7.0f;
    public Camera mainCamera;
    private Plane ground;
    private Vector3 playerToCameraDelta;
    public GameObject cursorHighlight;
    private MeshRenderer cursorHighlightMeshRenderer;
    public GameObject plantingRangeIndicator;
    private MeshRenderer plantingRangeIndicatorMeshRenderer;
    private float highlightHeight = 0.02f;
    private float maxPlantingDistance = 2.0f;
    private bool planted;
    private Inventory inventory;

    private bool readyToPlant => Input.GetKey(KeyCode.R);
    private Vector3 playerPosition => gameObject.transform.position;
    private Vector3 cameraPosition => mainCamera.transform.position;
    private Vector3 highlightPosition => cursorHighlight.transform.position;

    void Start()
    {
        playerToCameraDelta = cameraPosition - playerPosition;

        GameObject planeObject = GameObject.Find("Ground");
        if (planeObject != null)
        {
            ground = new Plane(planeObject.transform.up, planeObject.transform.position);
        }

        cursorHighlightMeshRenderer = cursorHighlight.GetComponent<MeshRenderer>();
        plantingRangeIndicatorMeshRenderer = plantingRangeIndicator.GetComponent<MeshRenderer>();

        planted = false;

        inventory = GetComponent<Inventory>();
    }

    void Update ()
    {
        HandleWASDMovement();
        HighlightCursorPositionOnGround();
        // HandleLeftClick();
    }

    void HandleWASDMovement()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(xInput, 0, zInput);
        Vector3 delta = input.normalized * Time.deltaTime * speed;
        gameObject.transform.position += delta;
        mainCamera.transform.position = playerPosition + playerToCameraDelta;
    }

    void HighlightCursorPositionOnGround()
    {
        if (readyToPlant && !planted)
        {
            planted = false;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);

            float distance;
            if (ground.Raycast(ray, out distance))
            {
                Vector3 pointOnPlane = ray.GetPoint(distance);
                cursorHighlight.transform.position = new Vector3(
                    pointOnPlane.x,
                    highlightHeight,
                    pointOnPlane.z);

                Vector3 playerHighlightDelta = highlightPosition - playerPosition;
                playerHighlightDelta.y = 0.0f;
                float distanceToPlant = playerHighlightDelta.magnitude;
                float plantDistanceScalar = 1.0f;
                if (distanceToPlant > maxPlantingDistance)
                {
                    plantDistanceScalar = maxPlantingDistance / distanceToPlant;
                    Vector3 offset = playerHighlightDelta * plantDistanceScalar;
                    Vector3 scaledPlantPosition = playerPosition + offset;
                    scaledPlantPosition.y = 0.0f;
                    cursorHighlight.transform.position = scaledPlantPosition;
                }
            }

            cursorHighlightMeshRenderer.enabled = true;
            plantingRangeIndicatorMeshRenderer.enabled = true;
        }
        else
        {
            cursorHighlightMeshRenderer.enabled = false;
            plantingRangeIndicatorMeshRenderer.enabled = false;
        }

        if (!readyToPlant)
        {
            planted = false;
        }
    }
}