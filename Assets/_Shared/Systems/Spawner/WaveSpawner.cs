using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Core {
  public interface IWaveUnit { }

  [Serializable]
  [InlineProperty]
  public class WaveDelay : IWaveUnit {
    public RandomFloat Value = new(0, 0);
  }

  [Serializable]
  [InlineProperty]
  public class WaveEntity<T> : IWaveUnit where T : MonoBehaviour {
    public RandomInt Loop = new(1, 1);
    public RandomFloat DelayBetweenLoop = new(1, 1);
    public RandomFloat DelayBetweenEntity = new(1, 1);
    [LabelText("Entities")] public List<T> Entities = new();
  }

  public class WavePattern<T> : ScriptableObject where T : MonoBehaviour {
    [field: SerializeReference] public List<IWaveUnit> Units { get; private set; } = new();

    [Button]
    public void UpdatePatternName() {
      var waveName = new StringBuilder();
      var firstEntityWavePassed = false;

      foreach (var unit in Units) {
        if (unit.GetType().Is<WaveEntity<T>>()) {
          var entityUnit = unit as WaveEntity<T>;
          for (var i = 0; i < entityUnit!.Loop; i++) {
            foreach (var entity in entityUnit.Entities) {
              if (i == 0) waveName.Append(entity.name);
            }
          }

          waveName.Append(entityUnit.Loop);
          if (firstEntityWavePassed) {
            waveName.Append("-");
          }

          firstEntityWavePassed = true;
        }
      }
      // UTIL
#if UNITY_EDITOR
      var assetPath = AssetDatabase.GetAssetPath(this);
      AssetDatabase.RenameAsset(assetPath, waveName.ToString());
      AssetDatabase.SaveAssets();
#endif
    }
  }

  // TODO: Events, pool
  // TODO: Global editing SO params
  public class WaveSpawner<T> : MonoBehaviour where T : MonoBehaviour {
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] [InlineEditor] private List<WavePattern<T>> _patterns = new();

    private void Start() {
      StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine() {
      foreach (var pattern in _patterns) {
        foreach (var unit in pattern.Units) {
          if (unit.GetType().Is<WaveDelay>()) {
            yield return new WaitForSeconds(((WaveDelay) unit).Value);
          }

          if (unit.GetType().Is<WaveEntity<T>>()) {
            var entityUnit = unit as WaveEntity<T>;
            for (var i = 0; i < entityUnit!.Loop; i++) {
              foreach (var entity in entityUnit.Entities) {
                Instantiate(entity, _spawnPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(entityUnit.DelayBetweenEntity);
              }

              yield return new WaitForSeconds(entityUnit.DelayBetweenLoop);
            }
          }
        }
      }
    }
  }
}