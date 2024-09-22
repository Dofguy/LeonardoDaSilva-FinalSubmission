using UnityEngine;
using UnityEditor;

public class NodeCreatorWindow : EditorWindow
{
    [MenuItem("Window/Node Creator")]
    public static void ShowWindow()
    {
        GetWindow<NodeCreatorWindow>("Node Creator");
    }

    private int numRows;
    private int numCols;
    private GameObject nodePrefab;
    private Transform parentNode;

    private void OnGUI()
    {
        GUILayout.Label("Node Creator Settings", EditorStyles.boldLabel);

        numRows = EditorGUILayout.IntField("Number of Rows", numRows);
        numCols = EditorGUILayout.IntField("Number of Columns", numCols);
        nodePrefab = (GameObject)EditorGUILayout.ObjectField("Node Prefab", nodePrefab, typeof(GameObject), false);
        parentNode = (Transform)EditorGUILayout.ObjectField("Parent Node", parentNode, typeof(Transform), true);

        if (GUILayout.Button("Create Nodes"))
        {
            CreateNodes();
        }
    }

    private void CreateNodes()
    {
        if (nodePrefab == null || parentNode == null)
        {
            Debug.LogError("Node Prefab and Parent Node cannot be null.");
            return;
        }

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                GameObject newNode = Instantiate(nodePrefab, new Vector3(j, -i, 0), Quaternion.identity);
                newNode.transform.SetParent(parentNode);
            }
        }
    }
}