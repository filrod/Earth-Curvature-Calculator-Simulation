using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatEarth : MonoBehaviour
{
    [SerializeField] private GameObject stick1;
    [SerializeField] private GameObject stick2;
    [SerializeField] private GameObject stick3;

    private Quaternion[] oldStickRotationsFromRoundEarth;

    private void OnEnable()
    {
        oldStickRotationsFromRoundEarth = new Quaternion[3];
        oldStickRotationsFromRoundEarth[0] = stick1.transform.rotation;
        oldStickRotationsFromRoundEarth[1] = stick2.transform.rotation;
        oldStickRotationsFromRoundEarth[2] = stick3.transform.rotation;
    }

    private void Update()
    {
        if (this.isActiveAndEnabled)
        {
            Vector3 right = stick2.transform.position - stick1.transform.position;
            Vector3 forward = stick2.transform.position - stick3.transform.position;

            Vector3 up = Vector3.Cross(forward, right);

            // Check if correct direction
            GameObject flatEarth = FindObjectOfType<FlatEarth>().gameObject;
            GameObject sun = FindObjectOfType<Light>().gameObject;
            Vector3 towardsLight = sun.transform.position - flatEarth.transform.position;

            bool upIsPositiveY = (up.y * towardsLight.y) >= 0;

            if (!upIsPositiveY) up *= -1;

            //if (this.GetComponent<GameObject>() != null)
            flatEarth.transform.rotation = Quaternion.LookRotation(forward, up);
            stick1.transform.rotation = flatEarth.transform.rotation;
            stick2.transform.rotation = flatEarth.transform.rotation;
            stick3.transform.rotation = flatEarth.transform.rotation;
            //else
            //    Debug.LogWarning("No gameobject attached to script");
        }
    }

    private void OnDisable()
    {
        stick1.transform.rotation = oldStickRotationsFromRoundEarth[0];
        stick2.transform.rotation = oldStickRotationsFromRoundEarth[1];
        stick3.transform.rotation = oldStickRotationsFromRoundEarth[2];
    }
}
