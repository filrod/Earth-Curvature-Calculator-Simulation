using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControls : MonoBehaviour
{
    private SunRays sunRaysScript;
    public bool focusCameraOnSticks = true;
    private void Awake()
    {
        sunRaysScript = FindObjectOfType<SunRays>();
    }
    private void Update()
    {
        Vector3 forward = sunRaysScript.GetMeanPositionOfStickTops();
        forward.Normalize();


        if (Input.GetKey(KeyCode.W))
            this.transform.localPosition += forward;

        if (Input.GetKey(KeyCode.S) && this.transform.position.magnitude >= 253f)
            this.transform.localPosition -= forward;

        if (Input.GetKey(KeyCode.D))
            this.transform.localPosition += this.transform.right;

        if (Input.GetKey(KeyCode.A))
            this.transform.position -= this.transform.right;
        this.transform.localPosition += (sunRaysScript.GetMeanPositionOfStickTops() - this.transform.position).normalized * Input.mouseScrollDelta[1];

        if (focusCameraOnSticks)
            this.transform.rotation = Quaternion.LookRotation(sunRaysScript.GetMeanPositionOfStickTops()-this.transform.position);
    }
        
        
}
