#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Enginooby.Utils {
  public static class AssetUtils {
    /// <example>Assets/Scripts/</example>
    public static string GetFolderPathOfAsset(ScriptableObject asset) {
      var scriptAsset = MonoScript.FromScriptableObject(asset);
      var scriptPath = AssetDatabase.GetAssetPath(scriptAsset);
      // UTIL: remove everything from last "/"
      var index = scriptPath.LastIndexOf("/", StringComparison.Ordinal);
      // if (index >= 0) 
      return scriptPath[..(index + 1)];
    }
  }
}
#endif