using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    // Store custom editor foldout toggle bool here to allow its valuie not to be lost in PlanetEditor.cs
    [HideInInspector]
    public bool shapeSettingsCustomMenuFoldout;
    [HideInInspector]
    public bool colourSettingsCustomMenuFoldout;

    // Auto update option from the editor
    public bool autoUpdate = true;

    // Set a terrain resolution
    [Range(2, 256)]
    public int resolution = 10;

    // Planet Colour and shape settings
    public ShapeSettings shapeSettings;
    public ColourSettings colourSettings;

    // Make a shape generator object for the planet's radius size
    ShapeGenerator shapeGenerator;

    // Mesh filters used when displaying the terrain faces [make serialized but hide in the inspector]
    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    Terrain_Face[] terrainFaces;


    // Initialize the meshFilters, one for each side to total 6
    void Initialize()
    {
        // Initialize shape generator
        shapeGenerator = new ShapeGenerator(shapeSettings);

        // Ony initialize if meshFilters has not been populated
        if (meshFilters == null || meshFilters.Length == 0)
            meshFilters = new MeshFilter[6];
        terrainFaces = new Terrain_Face[6];

        // set-up local directions
        Vector3[] directions = {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        // Cycle through the mesh faces
        for (int i = 0; i < 6; i++)
        {
            // Only create a mesh object if the entry for meshFilters is null
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                // Add mesh renderer
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(
                    Shader.Find("Standard")
                    ); // Make a default material and make it use the default shader
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            terrainFaces[i] = new Terrain_Face(
                shapeGenerator,
                meshFilters[i].sharedMesh, 
                resolution, 
                directions[i]
                );
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    // Gernerate then mesh
    void GenerateMesh()
    {
        foreach (Terrain_Face face in terrainFaces)
        {
            face.constructMesh();
        }
    }

    /*public static Mesh BuildMeshFromFilters( string name, params UnityEngine.Mesh[] baseMeshs)
    {
        LinkedList<Facet> facets = new LinkedList<Facet>();

        
        foreach (var mesh in baseMeshs)
        {
            Vector3 normal = (mesh.normals[mesh.triangles[i]] +
                                     mesh.normals[mesh.triangles[i + 1]] +
                                     mesh.normals[mesh.triangles[i + 2]]) / 3.0f;
            var oneMesh = mesh.Mesh;
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 v1 = mesh.vertices[mesh.triangles[i]] + container.Translation;
                //Vertex v2 = 
            }
        }
        return new Mesh(name, facets);
    }*/

    /// <summary>
    /// Export the planet mesh for SU2
    /// </summary>
    public void ExportPlanetMesh()
    {
        int totalLinesForObjFile = 0;
        int totalTrianglIndecies = 0;
        foreach (MeshFilter mf in meshFilters)
        {
            totalLinesForObjFile += mf.sharedMesh.vertices.Length;
            totalLinesForObjFile += mf.sharedMesh.triangles.Length;
            //
            totalTrianglIndecies += mf.sharedMesh.triangles.Length;
        }

        string[] meshInStrArray = new string[totalLinesForObjFile];
        string[] triangleArr = new string[totalTrianglIndecies];
        int i = 0;
        foreach (MeshFilter mf in meshFilters)
        {
            foreach (Vector3 vertex in mf.sharedMesh.vertices)
            {
                meshInStrArray[i] = "v " + vertex.x.ToString("R") + " " + vertex.y.ToString("R") + " " + vertex.z.ToString("R");
                i++;
            }

        }
        int resolutionMultiplier = 0; // To take into account the separation of the mesh into mesh filters. This upscales the indicies of the triangle arrays for later meshfilters
        foreach (MeshFilter mf in meshFilters)
        {
            for (int j = 2; j < mf.sharedMesh.triangles.Length; j += 3)
            {
                meshInStrArray[i] = "f " + (mf.sharedMesh.triangles[j - 2] + 1+ resolutionMultiplier).ToString() + " " + (mf.sharedMesh.triangles[j - 1] + 1+ resolutionMultiplier).ToString() + " " + (mf.sharedMesh.triangles[j] + 1+ resolutionMultiplier).ToString();
                i++;
            }
            resolutionMultiplier += mf.sharedMesh.vertices.Length;

        }
        System.IO.File.WriteAllLines(@".\Exports\testingMeshExport.obj", meshInStrArray);
    }

        /// <summary>
        /// Export the planet mesh for SU2
        /// </summary>
        public void ExportPlanetMeshin6()
    {
        int k = 0;
        foreach (MeshFilter mf in meshFilters)
        {
                
            int totalLinesForObjFile = mf.sharedMesh.vertices.Length + mf.sharedMesh.triangles.Length;
            string[] meshInStrArray = new string[totalLinesForObjFile];
            int i = 0;
            foreach (Vector3 vertex in mf.sharedMesh.vertices)
            {
                meshInStrArray[i] = "v " + vertex.x.ToString("R") + " " + vertex.y.ToString("R") + " " + vertex.z.ToString("R");
                i++;
            }
            for (int j = 2; j < mf.sharedMesh.triangles.Length; j += 3)
            {
                meshInStrArray[i] = "f " + (mf.sharedMesh.triangles[j - 2]+1).ToString() + " " + (mf.sharedMesh.triangles[j - 1]+1).ToString() + " " + (mf.sharedMesh.triangles[j]+1).ToString();
                i++;
            }
            System.IO.File.WriteAllLines( string.Format(@".\Exports\testingMeshExportFaceNumber{0}.obj",k.ToString()),  meshInStrArray);
            k++;

        }
        /*int k = 0;
        foreach (MeshFilter mf in meshFilters)
        {
            string kthStr = "";
            for (int j = 0; j < mf.sharedMesh.triangles.Length; j ++)
            {
                kthStr = kthStr + mf.sharedMesh.triangles[j].ToString() + " ";
            }
            triangleArr[k] = kthStr;
            k++;

        }
        System.IO.File.WriteAllLines(@"C:\Users\Hong Guo Group\Documents\testingTrianglesMeshExport.txt", triangleArr);
        */
    }

    // Set the colour
    void GenerateColours()
    {
        foreach (MeshFilter mf in meshFilters)
        {
            mf.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.planetColour;
        }
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    public void OnColourSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }
}