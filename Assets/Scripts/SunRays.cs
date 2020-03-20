using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRays : MonoBehaviour
{
    [HideInInspector] public Vector3 meanPositionOfStickTops = Vector3.zero;

    private RaycastHit rayHit;
    private Light sunRay;
    private bool hit;

    //[SerializeField] private Planet Earth;
    [SerializeField] private Vector3 lightOrigin = new Vector3(0f, 260.3f, -14.2f);
    [SerializeField] Vector3[] deltas;
    [SerializeField] private float maxRayLength = 100f;
    [SerializeField] private bool drawRays = true;
    [SerializeField] private bool drawHitPoints = true;
    [SerializeField] private GameObject[] sticks;

    // Start is called before the first frame update
    void Start()
    {
        sunRay = GetComponent<Light>();
        var stickScripts = FindObjectsOfType<StickPlacement>();
        sticks = new GameObject[stickScripts.Length];
        deltas = new Vector3[stickScripts.Length];
        int i = 0;
        foreach (var script in stickScripts)
        {
            sticks[i] = script.gameObject;
            deltas[i] = 0.05f * Vector3.up;
            i++;
        }
        SunViewingDir();
    }

    // Update is called once per frame
    void Update()
    {

        SunViewingDir();
        CastRays();
    }

    void SunViewingDir()
    {
        meanPositionOfStickTops = Vector3.zero;
        foreach (GameObject stick in this.sticks)
        {
            meanPositionOfStickTops += stick.transform.position + stick.GetComponent<Collider>().bounds.extents.y * Vector3.up;
        }
        meanPositionOfStickTops /= sticks.Length;

        sunRay.transform.SetPositionAndRotation(this.lightOrigin, Quaternion.LookRotation(meanPositionOfStickTops - lightOrigin));
    }

    /// <summary>
    /// Cast sunrays to measure shadow lengths for each shadow cast by a stick
    /// </summary>
    private void CastRays()
    {
        int i = 0;
        foreach (GameObject stick in this.sticks)
        {
            // Get stick top
            Vector3 stickTop = stick.transform.position + stick.GetComponent<Collider>().bounds.extents.y * Vector3.up;

            // Get ray origin
            Vector3 offset = stickTop - meanPositionOfStickTops;
            Vector3 rayOrigin  = lightOrigin + offset;
            

            // Ensure the ray is long enough
            float minLengthNeeded = Mathf.Abs((rayOrigin - stickTop).magnitude);
            if ( maxRayLength <= minLengthNeeded )
            {
                maxRayLength = minLengthNeeded + 3f;
            }

            // Cast a ray that shoots just above the tip of the stick
            hit = Physics.Raycast(rayOrigin + deltas[i], sunRay.transform.rotation * Vector3.forward, out rayHit, maxRayLength);

            // If user wishes to draw the rays
            if (this.drawRays)
            {
                if (hit)
                {
                    Debug.DrawLine(rayOrigin + deltas[i], rayHit.point, Color.green, 0.05f, true);
                    DrawHitPoints(this.drawHitPoints, 0.2f * stick.transform.localScale.x);
                    Debug.Log("Hit recorded! Has hit " + rayHit.collider.gameObject.tag + ". Coords: " + rayHit.point.ToString());
                }
                else
                { 
                    Debug.DrawLine(rayOrigin + deltas[i], rayOrigin + deltas[i] + this.maxRayLength * (sunRay.transform.rotation * Vector3.forward), Color.red, 0.05f, true);
                }
            }
            i++;
        }
    }

    /// <summary>
    /// Draws red spheres's where the sun raycast recorded a hit.
    /// 
    /// Gets an instance of the hitobject from the HitsPool inmplementation of
    /// the GenericObjectPool.
    /// 
    /// </summary>
    /// <param name="draw"></param>
    /// <param name="size"></param>
    void DrawHitPoints(bool draw, float size)
    {
        if (!draw) return;

        var hitMarker = HitsPool.Instance.Get();  
        hitMarker.transform.position = rayHit.point;
        hitMarker.transform.localScale = size * new Vector3(1f, 1f, 1f);
        hitMarker.gameObject.SetActive(true);
    }
}
