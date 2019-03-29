using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Will automatically create the perspective sort layer at import
/// Author: Indie Marc (Marc-Antoine Desbiens)
/// Company: Falling Flames Games
/// </summary>

namespace IndieMarc.TopDown
{
    [InitializeOnLoad]
    public class CreateLayers
    {
        static bool completed = false;

        static CreateLayers()
        {
            EditorApplication.hierarchyWindowChanged += AfterLoad;
        }

        static void AfterLoad()
        {
            if (!completed && DoesAssetExists("Assets/IndieMarc/TopDownDemo/Prefabs/CharacterTopDown.prefab")){
                completed = true;
                CreateSortingLayer("Perspective");
                CreateSortingLayer("Top");
                SetPrefabLayer("Assets/IndieMarc/TopDownDemo/Prefabs/CharacterTopDown.prefab", "Perspective");
                SetPrefabLayer("Assets/IndieMarc/TopDownDemo/Prefabs/Grass1.prefab", "Perspective");
                SetPrefabLayer("Assets/IndieMarc/TopDownDemo/Prefabs/Grass2.prefab", "Perspective");
                SetPrefabLayer("Assets/IndieMarc/TopDownDemo/Prefabs/Key.prefab", "Perspective");
                SetPrefabLayer("Assets/IndieMarc/TopDownDemo/Prefabs/Lever.prefab", "Perspective");
            }
        }

        public static bool CreateSortingLayer(string layerName)
        {
            var serializedObject = new SerializedObject(AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset"));
            var sortingLayers = serializedObject.FindProperty("m_SortingLayers");
            for (int i = 0; i < sortingLayers.arraySize; i++)
                if (sortingLayers.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue.Equals(layerName))
                    return false;
            sortingLayers.InsertArrayElementAtIndex(sortingLayers.arraySize);
            var newLayer = sortingLayers.GetArrayElementAtIndex(sortingLayers.arraySize - 1);
            newLayer.FindPropertyRelative("name").stringValue = layerName;
            newLayer.FindPropertyRelative("uniqueID").intValue = layerName.GetHashCode(); /* some unique number */
            serializedObject.ApplyModifiedProperties();
            Debug.Log("Created sorting layer: " + layerName);
            return true;
        }

        public static void SetPrefabLayer(string path, string layerName)
        {
            GameObject obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;

            // Edit it
            if (obj && obj.GetComponent<SpriteRenderer>())
            {
                SpriteRenderer render = obj.GetComponent<SpriteRenderer>();
                if (render.sortingLayerName != layerName)
                {
                    Debug.Log(path + " sorting layer changed from "
                        + render.sortingLayerName
                        + " to " + layerName);

                    render.sortingLayerName = layerName;
                    EditorUtility.SetDirty(obj);
                }
            }
        }

        public static bool DoesAssetExists(string path)
        {
            GameObject obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            return obj != null;
        }
    }

}