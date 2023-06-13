using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform fullBodyPosition;
    public Transform headOnlyPosition;

    private bool needsToZoomIn;
    private bool needsToZoomOut;
    private float speed = 3f;

    // Update is called once per frame
    void Update()
    {
        if (needsToZoomIn)
        {
            Zoom(headOnlyPosition.position);
            if (transform.position == headOnlyPosition.position)
            {
                needsToZoomIn = false;
            }
        }

        if (needsToZoomOut)
        {
            Zoom(fullBodyPosition.position);
            if (transform.position == fullBodyPosition.position)
            {
                needsToZoomOut = false;
            }
        }

    }

    public void Zoom(Vector3 towards)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, towards, step);
    }

    public void SetNeedsToZoomIn()
    {
        needsToZoomIn = true;
    }

    public void SetNeedsToZoomOut()
    {
        needsToZoomOut = true;
    }

}
