using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the camera moves
    public float edgeThreshold = 10f; // Distance (in pixels) from the edge of the screen to detect mouse movement
    private float minX = -30f; // Minimum x position
    private float maxX = 30f; // Maximum x position

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x = -1;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movement.x = 1;
        }

        // Mouse edge detection
        if (Input.mousePosition.x <= edgeThreshold)
        {
            movement.x = -1;
        }
        else if (Input.mousePosition.x >= Screen.width - edgeThreshold)
        {
            movement.x = 1;
        }

        // Calculate new position
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;

        // Clamp the x position
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // Apply the new position
        transform.position = newPosition;
    }
}
