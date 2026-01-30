using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Script_NPCRoutine))]
public class Script_NPCRoutineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Script_NPCRoutine npcRoutine = (Script_NPCRoutine)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Development Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate Empty Waypoints (3)"))
        {
            Undo.RecordObject(npcRoutine, "Generate Waypoints");

            npcRoutine.path = new WaypointNode[3];

            // Ensure main container exists
            GameObject mainContainer = GameObject.Find("WaypointContainer");
            if (mainContainer == null)
            {
                mainContainer = new GameObject("WaypointContainer");
                Undo.RegisterCreatedObjectUndo(mainContainer, "Create WaypointContainer");
            }

            // Create or find per-NPC container
            string npcFolderName = npcRoutine.name;
            Transform npcSubContainer = mainContainer.transform.Find(npcFolderName);
            if (npcSubContainer == null)
            {
                GameObject subContainer = new GameObject(npcFolderName);
                subContainer.transform.SetParent(mainContainer.transform);
                npcSubContainer = subContainer.transform;
                Undo.RegisterCreatedObjectUndo(subContainer, "Create NPC SubContainer");
            }

            // Create the actual waypoints
            for (int i = 0; i < 3; i++)
            {
                GameObject newPoint = new GameObject($"Waypoint_{i + 1}");
                newPoint.transform.position = npcRoutine.transform.position + Vector3.right * i * 2f;
                newPoint.transform.SetParent(npcSubContainer);

                npcRoutine.path[i] = new WaypointNode
                {
                    point = newPoint.transform,
                    waitTime = 1f,
                    performActionHere = false,
                    animationTrigger = ""
                };

                Undo.RegisterCreatedObjectUndo(newPoint, "Create Waypoint");
            }

            EditorUtility.SetDirty(npcRoutine);
        }
    }
}
