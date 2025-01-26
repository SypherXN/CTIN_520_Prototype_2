using UnityEngine;

public class DeleteObjectOnClick : MonoBehaviour
{
    public string targetTag = "Target"; // Tag to identify which objects to check
    public LayerMask occlusionLayers; // Layers that should occlude the object

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        // Check for left mouse click
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            // Check if the object has the correct tag
            if (gameObject.CompareTag(targetTag))
            {
                // Check if the object is within the camera's view
                if (IsObjectVisible())
                {
                    // If the object is visible, check if it's occluded
                    if (!IsOccluded())
                    {
                        // Delete the object if it's visible and not occluded
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private bool IsObjectVisible()
    {
        // Check if the object is within the camera's frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds);
    }

    private bool IsOccluded()
    {
        // Cast a ray from the camera to the object to check if it is occluded
        RaycastHit hit;
        Vector3 direction = transform.position - mainCamera.transform.position;

        // Perform the raycast, check if something hits the object, and ensure it's not from an occlusion layer
        if (Physics.Raycast(mainCamera.transform.position, direction, out hit))
        {
            // If the ray hits something, it could be occluded by the layer defined in occlusionLayers
            return ((1 << hit.collider.gameObject.layer) & occlusionLayers) != 0;
        }

        return false; // No occlusion detected
    }
}
