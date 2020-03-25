using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRays : MonoBehaviour
{

    [HideInInspector] public Vector3 meanPositionOfStickTops = Vector3.zero;

    private RaycastHit rayHit;
    private Light sunRay;
    private bool hit;
    public bool saveData = false;
    public string[] dataCSV = new string[16];

    //[SerializeField] private Planet Earth;
    [SerializeField] private Vector3 lightOrigin = new Vector3(0f, 260.3f, -14.2f);
    public Vector3 LightOrigin { get { return this.lightOrigin; } set { this.lightOrigin = new Vector3(value.x, value.y, value.z); } }
    [SerializeField] private Vector3[] deltas;
    public Vector3[] Deltas { get{ return this.deltas; } set { this.deltas = value; } }
    [SerializeField] private float maxRayLength = 100f;
    [SerializeField] private bool drawRays = true;
    [SerializeField] private bool drawHitPoints = true;
    [SerializeField] public GameObject[] sticks;

    
    void Awake()
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
        // Write first 2 entries to the CSV Array
        if (FindObjectOfType<Planet>() != null)
        {
            dataCSV[0] = FindObjectOfType<Planet>().gameObject.transform.rotation.ToString();
            dataCSV[dataCSV.Length-1] = "Round Planet Model";
        }
        else
        {
            dataCSV[0] = FindObjectOfType<FlatEarth>().gameObject.transform.rotation.ToString();
            dataCSV[dataCSV.Length - 1] = "Flat Earth Model";
        }

        dataCSV[1] = this.lightOrigin.ToString();

        bool[] allHitPlanet = { false, false, false };

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
            rayOrigin += deltas[i];
            // Cast a ray that shoots just above the tip of the stick
            hit = Physics.Raycast(rayOrigin, sunRay.transform.rotation * Vector3.forward, out rayHit, maxRayLength);

            // If user wishes to draw the rays
            if (this.drawRays)
            {
                if (hit)
                {
                    Debug.DrawLine(rayOrigin, rayHit.point, Color.green, 0.05f, true);
                    DrawHitPoints(this.drawHitPoints, 0.2f * stick.transform.localScale.x);
                    Debug.Log("Hit recorded! Has hit " + rayHit.collider.gameObject.tag + ". Coords: " + rayHit.point.ToString());

                    // set the allHitPlanet variable bool
                    allHitPlanet[i] = rayHit.collider.gameObject.CompareTag("Planet");

                    // Write data to CSV array
                    int i_columnStart;
                    if (stick.name == "Stick 1")
                        i_columnStart = 3;
                    else if (stick.name == "Stick 2")
                        i_columnStart = 7;
                    else if (stick.name == "Stick 3")
                        i_columnStart = 11;
                    else
                    {
                        i_columnStart = -1;
                        Debug.LogError("Error: Unexpected name of " + stick.gameObject.name + " in writing csv. Make sure the tag names for the sticks are Stick 1, Stick 2 and Stick 3");
                    }

                    // World Space Stick Position
                    dataCSV[i_columnStart] = stick.gameObject.transform.position.ToString();
                    // Shadow position
                    dataCSV[i_columnStart + 1] = rayHit.point.ToString();
                    // Stick height
                    dataCSV[i_columnStart + 2] = stickTop.ToString();
                    // Ray start position
                    dataCSV[i_columnStart + 3] = rayOrigin.ToString();
                }
                else
                { 
                    Debug.DrawLine(rayOrigin, rayOrigin + this.maxRayLength * (sunRay.transform.rotation * Vector3.forward), Color.red, 0.05f, true);
                }
            }
            i++;
        }

        // Make sure no data is false or null
        foreach (bool hitPlanet in allHitPlanet)
        {
            if (!hitPlanet) return;
        }
        // If you made it here save data
        SaveToCSV();
    }

    public void SaveToCSV()
    {
        if (!this.saveData) return;

        string line = string.Join(";", dataCSV);
        System.IO.File.AppendAllText(@".\Exports\Curvature Data\ShadowHitsAndAllPositions.csv", line+System.Environment.NewLine);
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
