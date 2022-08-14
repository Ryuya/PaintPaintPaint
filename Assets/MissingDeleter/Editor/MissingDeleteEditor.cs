using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(MissingDelete))]

public class MissingDeleteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MissingDelete missingDelete = target as MissingDelete;

        if (GUILayout.Button("StartDelete"))
        {
            MissingDeleter(missingDelete.analysingObject);
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(missingDelete.analysingObject);
        }

    }

    private void MissingDeleter(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(child.gameObject);
            if (child.transform.childCount != 0)
            {
                MissingDeleter(child.gameObject);
            }
        }
    }
}
