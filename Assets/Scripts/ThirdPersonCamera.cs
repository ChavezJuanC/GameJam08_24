using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObject;
    [SerializeField] private float rotationSpeed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the direction from the player to the camera's position on the XZ plane
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        // Set the orientation's forward direction to the calculated view direction
        orientation.forward = viewDir;

        // Get input from the player (WASD or arrow keys)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the input direction based on the orientation
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // If there is any input, rotate the player object smoothly towards the input direction
        if (inputDir != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
