using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRay : MonoBehaviour
{
    private RaycastHit rayHit;
    private Light sunRay;
    private bool hit;
    private string previousObjectHit;

    //[SerializeField] private Planet Earth;
    [SerializeField] private Vector3 lightOrigin = new Vector3(0f, 260.3f, -14.2f);
    [SerializeField] private Vector3 lightDirection;
    [SerializeField] private float maxRayLength = 100f;
    [SerializeField] private bool drawRays = true;
    [SerializeField] private GameObject stick;

    // Start is called before the first frame update
    void Start()
    {
        sunRay = GetComponent<Light>();
        lightDirection = new Vector3(0f, 260.3f, -14.2f) - Vector3.up * 254.926f;
    }

    // Update is called once per frame
    void Update()
    {

        sunRay.transform.SetPositionAndRotation(this.lightOrigin, Quaternion.Euler(this.lightDirection));
        Vector3 dir = this.lightDirection;


        hit = Physics.Raycast(lightOrigin, sunRay.transform.rotation * Vector3.forward, out rayHit, maxRayLength);

        if (this.drawRays)
            Debug.DrawLine(lightOrigin, lightOrigin + this.maxRayLength * (sunRay.transform.rotation * Vector3.forward), Color.red, 0.05f, true);
        if (hit)
        {
            Debug.Log("Hit recorded! Has hit " + rayHit.collider.gameObject.tag + ". Coords: " + rayHit.point.ToString());
            if (this.previousObjectHit != null)
            {
                if (!rayHit.collider.gameObject.CompareTag(this.previousObjectHit) && this.previousObjectHit != "Planet")
                {
                    Debug.Log("Shadow length = "+(rayHit.point- stick.transform.position).ToString());
                }
            }
            this.previousObjectHit = rayHit.collider.gameObject.tag;
        }
        else
        {
            this.previousObjectHit = null;
        }
    }
}
