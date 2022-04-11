using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public static class CompilingSymbolUtils {
  private const char DefineSeparator = ';';

  private static readonly List<string> _allDefines = new();

  // batching
  private static readonly List<string> _definesToAdd = new();
  private static readonly List<string> _definesToRemove = new();

  /// <summary>
  /// Since updating defines will recompile and take time, set <paramref name="autoUpdate"/> to false
  /// to manually update using method UpdateDefines() after batching.
  /// </summary>
  public static void Add(bool autoUpdate, params string[] defines) {
    if (autoUpdate) {
      _allDefines.Clear();
      _allDefines.AddRange(GetDefines());
      _allDefines.AddRange(defines.Except(_allDefines));
      SetDefines(_allDefines);
    }
    else {
      _definesToAdd.AddRange(defines);
    }
  }

  public static void Remove(bool autoUpdate, params string[] defines) {
    if (autoUpdate) {
      _allDefines.Clear();
      _allDefines.AddRange(GetDefines().Except(defines));
      SetDefines(_allDefines);
    }
    else {
      _definesToRemove.AddRange(defines);
    }
  }

  public static void AddOrRemove(bool autoUpdate, string define, bool addCondition) {
    if (addCondition)
      Add(autoUpdate, define);
    else
      Remove(autoUpdate, define);
  }

  public static void Clear() {
    _allDefines.Clear();
    SetDefines(_allDefines);
  }

  private static IEnumerable<string> GetDefines() =>
    PlayerSettings.GetScriptingDefineSymbolsForGroup(
      EditorUserBuildSettings.selectedBuildTargetGroup).Split(DefineSeparator).ToList();

  private static void SetDefines(List<string> allDefines) =>
    PlayerSettings.SetScriptingDefineSymbolsForGroup(
      EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(DefineSeparator.ToString(),
        allDefines.ToArray()));

  public static void UpdateDefines() {
    _allDefines.Clear();
    _allDefines.AddRange(GetDefines().Concat(_definesToAdd).Except(_definesToRemove));
    _definesToAdd.Clear();
    _definesToRemove.Clear();
    SetDefines(_allDefines);
  }
}