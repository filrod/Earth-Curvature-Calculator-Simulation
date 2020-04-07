using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControlls : MonoBehaviour
{
    [SerializeField] private GameObject planet;
    [SerializeField] private GameObject sun;
    [SerializeField] private GameObject plane;
    private GameObject[] sticks;

    private SunRays sunRaysScript;
    private StickPlacement[] stickPlacement;

    public Vector3 PlanetRotationSliders { get; set; } = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        plane.SetActive(false);
        Debug.Log("Sun: "+sun.gameObject.name);
        sunRaysScript = sun.GetComponent<SunRays>();
        Debug.Log("Sun Rays Script: " + sunRaysScript.LightOrigin.ToString());
        sticks = sunRaysScript.sticks;
        Debug.Log("Sticks Arr length: "+sticks.Length);
        PopulateSitckPlacementArr();
        
    }

    private void PopulateSitckPlacementArr()
    {
        this.stickPlacement = new StickPlacement[this.sticks.Length];
        for (int i = 0; i < this.sticks.Length; i++)
        {

            this.stickPlacement[i] = this.sticks[i].GetComponent<StickPlacement>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDeltaY1(float delta)
    {
        sunRaysScript.Deltas[2] = delta * Vector3.up;
    }

    public void SetDeltaY2(float delta)
    {
        sunRaysScript.Deltas[1] = delta * Vector3.up;
    }

    public void SetDeltaY3(float delta)
    {
        sunRaysScript.Deltas[0] = delta * Vector3.up;
    }

    public void SetStick1Lattitude(float lattitude)
    {
        this.stickPlacement[2].lattitude = lattitude;
    }

    public void SetStick2Lattitude(float lattitude)
    {
        this.stickPlacement[1].lattitude = lattitude;
    }

    public void SetStick3Lattitude(float lattitude)
    {
        this.stickPlacement[0].lattitude = lattitude;
    }

    public void SetStick1Longitude(float longitude)
    {
        this.stickPlacement[2].longitude = longitude;
    }

    public void SetStick2Longitude(float longitude)
    {
        this.stickPlacement[1].longitude = longitude;
    }

    public void SetStick3Longitude(float longitude)
    {
        this.stickPlacement[0].longitude = longitude;
    }

    public void XLightOriginUpdate(float x)
    {
        Vector3 oldVal = sunRaysScript.LightOrigin;
        sunRaysScript.LightOrigin = new Vector3(x, oldVal.y, oldVal.z);
    }

    public void YLightOriginUpdate(float y)
    {
        Vector3 oldVal = sun.GetComponent<SunRays>().LightOrigin;
        Debug.Log("yLight: "+ oldVal.y);
        sunRaysScript.LightOrigin = new Vector3(oldVal.x, y, oldVal.z);
        Debug.Log("yLight|| old: " + oldVal.y+ " |  new: "+ sunRaysScript.LightOrigin.y);
    }

    public void ZLightOriginUpdate(float z)
    {
        Vector3 oldVal = sunRaysScript.LightOrigin;
        sunRaysScript.LightOrigin = new Vector3(oldVal.x, oldVal.y, z);
    }

    public void XSliderUpdate(float angleAroundXAxis)
    {
        Debug.Log("Slider: "+angleAroundXAxis);
        this.PlanetRotationSliders = new Vector3(
            angleAroundXAxis,
            this.PlanetRotationSliders.y,
            this.PlanetRotationSliders.z
            );

        planet.transform.localRotation = Quaternion.Euler(
            angleAroundXAxis,
            this.PlanetRotationSliders.y,
            this.PlanetRotationSliders.z
            );
    }

    public void YSliderUpdate(float angleAroundYAxis)
    {
        this.PlanetRotationSliders = new Vector3(
            this.PlanetRotationSliders.x,
            angleAroundYAxis,
            this.PlanetRotationSliders.z
            );

        planet.transform.localRotation = Quaternion.Euler(
            this.PlanetRotationSliders.x,
            angleAroundYAxis,
            this.PlanetRotationSliders.z
            );
    }

    public void ZSliderUpdate(float angleAroundZAxis)
    {
        this.PlanetRotationSliders = new Vector3(
            this.PlanetRotationSliders.x,
            this.PlanetRotationSliders.y,
            angleAroundZAxis
            );

        planet.transform.localRotation = Quaternion.Euler(
            this.PlanetRotationSliders.x,
            this.PlanetRotationSliders.y,
            angleAroundZAxis
            );
    }

    public void SaveData(bool isToggled)
    {
        sunRaysScript.saveData = isToggled;
    }

    public void EnableFlatEarth(bool isToggled)
    {
        if (isToggled)
        {
            plane.SetActive(true);
            planet.SetActive(false);
            foreach (var script in this.stickPlacement)
            {
                if (script.enabled)
                    script.enabled = false;
            }


            Vector3 avStickPos = Vector3.zero;
            foreach (var stick in sticks)
            {
                avStickPos += stick.transform.position;
            }
            avStickPos = 1f / sticks.Length * avStickPos;
            plane.transform.position = avStickPos;

            
        }
        else {
            plane.SetActive(false);
            planet.SetActive(true);
            foreach (var script in this.stickPlacement)
            {
                if (!script.enabled)
                    script.enabled = true;
            }
            
            return;
        }
        
    }

    public void CamCentre(bool focus)
    {
        FindObjectOfType<CameraControls>().focusCameraOnSticks = focus;
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
