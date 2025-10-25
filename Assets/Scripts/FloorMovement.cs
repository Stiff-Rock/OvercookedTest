using UnityEngine;

public class FloorMovement : MonoBehaviour
{
    [SerializeField]
    private float tiltSpeed = 10f;

    private void Update()
    {
        // Tilt to selected direction
        Vector3 tiltDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            tiltDirection += transform.right;

        if (Input.GetKey(KeyCode.D))
            tiltDirection += -transform.forward;

        if (Input.GetKey(KeyCode.S))
            tiltDirection += -transform.right;

        if (Input.GetKey(KeyCode.A))
            tiltDirection += transform.forward;

        // Spin around
        if (Input.GetKey(KeyCode.Q))
            tiltDirection += transform.up;

        if (Input.GetKey(KeyCode.E))
            tiltDirection += -transform.up;

        transform.eulerAngles += tiltDirection.normalized * tiltSpeed * Time.deltaTime;
    }
}
