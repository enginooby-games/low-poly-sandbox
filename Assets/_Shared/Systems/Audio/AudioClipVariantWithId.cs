using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Audio {
  [Serializable]
  [InlineProperty]
  public class AudioClipVariantWithId {
    [ShowInInspector] [DisplayAsString] [SerializeField]
    private AudioClipVariantId _id;

    [SerializeField] [HideLabel] [InlineEditor]
    private AudioClipVariantSO _variantSo;

    public AudioClipVariantWithId(AudioClipVariantId id) => _id = id;
    public AudioClipVariantId Id => _id;
    public AudioClipVariantSO VariantSo => _variantSo;
  }
}