using UnityEngine;
using UnityEngine.EventSystems;

public class CameraPlayer : MonoBehaviour

{
    public Transform cameraTransform;
    public float cameraMovementSpeed;
    public float cameraMovementTime;
    public float rotationAmount;
    public float zoomSpeed;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    private float zoomAmount = 1f;
    private float scrollInput;
    private Vector3 zoomVector;

    private void Start()
    {
        // Rotation & position of the camera should be at the location of the rig object.
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    private void Update()
    {
        Move();
        Rotate();
        Zoom();
    }

    private void Zoom()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //else
        //{
            //Get the input axis
            scrollInput = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            //Limit the scroll up/down to certain zoomAmount values (min for zoom in, max for zoom out)
            zoomAmount = Mathf.Clamp(zoomAmount - scrollInput, -1.6f, 1.4f);
            //New targetPosition Vector to calculate position according to zoom amount
            zoomVector = newZoom + new Vector3(0, 3, -2) * zoomAmount;
            //Lerp
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomVector, Time.deltaTime * cameraMovementTime);
            //Debug.Log("Zoom amount is at " + zoomAmount);
        //}
    }

    private void Move()
    {

        if (Input.GetKey(KeyCode.W))
        {
            newPosition += (transform.forward * cameraMovementSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            newPosition += transform.right * -cameraMovementSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPosition += transform.forward * -cameraMovementSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPosition += transform.right * cameraMovementSpeed;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * cameraMovementTime);

    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * cameraMovementTime);
    }

}
