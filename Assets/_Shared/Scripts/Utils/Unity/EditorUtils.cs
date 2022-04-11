#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Enginooby.Utils {
  public static class EditorUtils {
    public static void StopPlayMode() {
      EditorApplication.isPlaying = false;
      // Application.Quit();
    }

    public static bool IsInteger(this SerializedProperty property) =>
      property.propertyType == SerializedPropertyType.Integer;

    public static bool IsReferenceType(this SerializedProperty property) =>
      property.propertyType == SerializedPropertyType.ObjectReference;

    public static bool DisplayDialog(string message, string title = "", string ok = "Yes", string cancel = "No") =>
      EditorUtility.DisplayDialog(title, message, ok, cancel);

    public static List<T> GetAssetsWithScript<T>(string path) where T : MonoBehaviour {
      var assets = new List<T>();
      // UTIL: AssetUtils - FindPrefabs
      var guids = AssetDatabase.FindAssets("t:Prefab", new[] {path});

      foreach (var t in guids) {
        // UTIL: AssetUtils - GetAssetByGUID
        var assetPath = AssetDatabase.GUIDToAssetPath(t);
        var assetGo = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        // UTIL: List add if contain component
        if (assetGo.TryGetComponent(out T component)) assets.Add(component);
      }

      return assets;
    }

    public static T[] GetScriptableObjectsOf<T>() where T : ScriptableObject {
      //FindAssets uses tags check documentation for more info
      var guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
      var a = new T[guids.Length];
      for (var i = 0; i < guids.Length; i++) //probably could get optimized 
      {
        var path = AssetDatabase.GUIDToAssetPath(guids[i]);
        a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
      }

      return a;
    }

    [MenuItem("Enginooby/Show All Hidden GameObjects")]
    private static void ShowAll() {
      var allGameObjects = Object.FindObjectsOfType<GameObject>();
      foreach (var go in allGameObjects)
        switch (go.hideFlags) {
          case HideFlags.HideAndDontSave:
            go.hideFlags = HideFlags.DontSave;
            break;
          case HideFlags.HideInHierarchy:
          case HideFlags.HideInInspector:
            go.hideFlags = HideFlags.None;
            break;
        }
    }
  }
}
#endif