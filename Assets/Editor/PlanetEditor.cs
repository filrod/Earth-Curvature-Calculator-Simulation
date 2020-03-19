using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor colourEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                // Used instead of an onValidate() method in the Planet script
                planet.GeneratePlanet();  // Update after changes in the editor
            }
        }

        // Manual planet generation button
        if (GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        // Export the mesh for SU2!
        if (GUILayout.Button("Export Planet Mesh"))
        {
            Debug.Log("Exported");
            planet.ExportPlanetMesh();
        }

        // Pass the toggle bool by reference to allow it to update the bool referenced and not the copied val
        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsCustomMenuFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdated, ref planet.colourSettingsCustomMenuFoldout, ref colourEditor);
    }

    // Pass the toggle bool by reference to allow it to update the bool referenced and not the copied val
    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool menuFoldout, ref Editor editor)
    {
        // Never draw unless settings have been given in the editor
        if (settings != null)
        {
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                // Help define where our custom editor begins
                menuFoldout = EditorGUILayout.InspectorTitlebar(menuFoldout, settings);


                // Only draw the custom menu if foldout bool is true 
                // (since this bool is not serialized store it in planet class)
                if (menuFoldout)
                {
                    // Since we dont want this effected (created and destroyed) by the toggle leave we make the editor fields and use a cache
                    CreateCachedEditor(settings, null, ref editor);  // This creates the need for editor fields for colour and shape (null is to keep the standard type editor)
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
