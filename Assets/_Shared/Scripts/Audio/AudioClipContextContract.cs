using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Audio {
  // TODO: Implement composite contract (e.g., combine assets in multiple small projects for a big project) 
  // Composite pattern?
  /// <summary>
  ///   [Concurrent extension point]
  ///   Specify which sounds can be played in a project. Kind of a sound interface.
  /// </summary>
  [CreateAssetMenu(fileName = "AudioContextContract_", menuName = "Audio/Context Contract", order = 0)]
  public class AudioClipContextContract : ScriptableObject {
    [SerializeField] [InlineEditor] private List<AudioClipVariantId> _audioIds = new();
    public List<AudioClipVariantId> AudioIds => _audioIds;

    // TODO: Fallback variants for every id;
  }
}