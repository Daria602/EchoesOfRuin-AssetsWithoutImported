using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    // Singleton package
    private static CameraManager instance;
    public static CameraManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Too many CameraManager instances");
        }
        instance = this;
    }
    


    [SerializeField] private Camera sceneCamera;
    public Camera GetSceneCamera()
    {
        return sceneCamera;
    }

    [SerializeField] private Transform cameraRig;
    public Transform GetCameraRig()
    {
        return cameraRig;
    }

    // For movement
    [SerializeField] private float cameraMovementSpeed;
    [SerializeField] private float cameraMovementTime;
    private Vector3 newPosition;

    // For rotation
    [SerializeField] private float rotationAmount;
    [SerializeField] private float cameraRotationTime;
    private Quaternion newRotation;

    // For zoom
    [SerializeField] private float zoomSpeed;
    private float zoomAmount = 1f;
    private Vector3 zoomVector;
    private Vector3 newZoom;

    private void Move()
    {
        
        int[] moveDirections = InputManager.GetInstance().CameraMoveInput();
        if (moveDirections[0] == 1)
        {
            newPosition += cameraRig.forward * cameraMovementSpeed; // move forward
        }
        else if (moveDirections[1] == 1)
        {
            newPosition += cameraRig.forward * -cameraMovementSpeed; // move back
        }
        if (moveDirections[2] == 1)
        {
            newPosition += cameraRig.right * -cameraMovementSpeed; // move left
        }
        else if (moveDirections[3] == 1)
        {
            newPosition += cameraRig.right * cameraMovementSpeed; // move right
        }

        cameraRig.position = Vector3.Lerp(cameraRig.position, newPosition, Time.deltaTime * cameraMovementTime);

    }

    private void Rotate()
    {
        int[] rotateDirection = InputManager.GetInstance().CameraRotateInput();

        if (rotateDirection[0] == 1)
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        else if (rotateDirection[1] == 1)
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        cameraRig.rotation = Quaternion.Lerp(cameraRig.rotation, newRotation, Time.deltaTime * cameraRotationTime);
    }

    private void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //Get the input axis
        
        // Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        // Limit the scroll up/down to certain zoomAmount values (min for zoom in, max for zoom out)
        zoomAmount = Mathf.Clamp(zoomAmount - scrollInput, -1.6f, 1.4f);
        // New targetPosition Vector to calculate position according to zoom amount
        zoomVector = newZoom + new Vector3(0, 3, -2) * zoomAmount;
        // Lerp
        sceneCamera.transform.localPosition = Vector3.Lerp(sceneCamera.transform.localPosition, zoomVector, Time.deltaTime * cameraMovementTime);
        //Debug.Log("Camera local posiiton: " + sceneCamera.transform.localPosition);
        //Debug.Log("Zoom vector: " + zoomVector);
        //Debug.Log("Time times movement time: " + Time.deltaTime * cameraMovementTime);
        //Debug.Log("Lerp: " + Vector3.Lerp(sceneCamera.transform.localPosition, zoomVector, Time.deltaTime * cameraMovementTime));
    }


    private void Start()
    {
        // Rotation & position of the camera should be at the location of the rig object.
        newPosition = cameraRig.position;
        newRotation = cameraRig.rotation;
        newZoom = sceneCamera.transform.localPosition;
    }

    private void Update()
    {
        Move();
        Rotate();
        Zoom();
    }

}
