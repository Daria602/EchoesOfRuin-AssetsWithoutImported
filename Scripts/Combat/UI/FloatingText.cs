using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    Transform mainCam;
    Transform worldSpaceCanvas;
    public Vector3 offset;
    Transform unit;
    TextMeshProUGUI combatText;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = CameraManager.GetInstance().GetSceneCamera().transform;
        worldSpaceCanvas = GameObject.Find("WorldSpaceCanvas").transform;
        unit = transform.parent;
        combatText = gameObject.GetComponent<TextMeshProUGUI>();
        combatText.text = Constants.GetInstance().GetRandomCombatText();
        transform.SetParent(worldSpaceCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.position); // look at the camera
        transform.position = unit.position + offset;
    }
}
