using UnityEngine;

/// <summary>
/// Class to be attached to a stick
/// </summary>
public class StickPlacement : MonoBehaviour
{
    public ShapeSettings planetShapeSettings;

    [SerializeField] [Tooltip("Adjust lattitude of stick ")]
    [Range(-90f, 90f)] private float lattitude=90f;

    [SerializeField] [Tooltip("Adjust longitude of stick ")]
    [Range(-180f, 180f)] private float longitude=0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = new Vector3(
            Cos(lattitude) * Sin(longitude),
            Sin(lattitude),
            Cos(lattitude) * Cos(longitude)
            );
        RaycastHit rayCastHitPlanetMesh;
        bool hit = false;
        hit = Physics.Raycast((250f*1.5f)*dir, -dir, out rayCastHitPlanetMesh, 250f * 3f);
        if (hit)
        {
            if (rayCastHitPlanetMesh.collider.gameObject.CompareTag("Planet"))
            {
                this.transform.position = rayCastHitPlanetMesh.point - Vector3.zero;
                transform.rotation = Quaternion.LookRotation(dir);
                transform.Rotate(new Vector3(90f, 0f, 0f));
                //Debug.Log("Hit planet at " + rayCastHitPlanetMesh.point.ToString());
                Debug.DrawLine(Vector3.zero, rayCastHitPlanetMesh.point, Color.blue, 0.05f);
            }
            else
            {
                //Debug.Log("Hit " + rayCastHitPlanetMesh.collider.gameObject.tag);
                Debug.DrawLine(Vector3.zero, rayCastHitPlanetMesh.point, Color.blue, 0.05f);
                /*RaycastHit obstructedHitInfo = rayCastHitPlanetMesh;
                hit = Physics.Raycast(obstructedHitInfo.point, dir, out rayCastHitPlanetMesh, 250f * 1.5f);
                if (hit)
                {
                    if (rayCastHitPlanetMesh.collider.gameObject.CompareTag("Planet"))
                    {
                        this.transform.position = rayCastHitPlanetMesh.point - Vector3.zero;
                        Debug.DrawLine(Vector3.zero, rayCastHitPlanetMesh.point, Color.blue, 0.05f);
                    }
                }*/
            }
        }
    }

    /// <summary>
    /// A cosine function for degrees
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private float Cos(float angle)
    {
        return Mathf.Cos(Mathf.Deg2Rad * angle);
    }

    /// <summary>
    /// A sine function for degrees
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private float Sin(float angle)
    {
        return Mathf.Sin(Mathf.Deg2Rad * angle);
    }
}
