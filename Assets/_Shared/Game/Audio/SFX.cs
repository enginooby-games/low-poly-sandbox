using System;
using System.Collections.Generic;
using Enginooby.Audio;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Prototype {
  [Serializable]
  [InlineProperty]
  public class SFX {
    // TODO: Search function
    // https://odininspector.com/attributes/searchable-attribute#searchable-perks

    // TODO: Hide full name type (Enginooby.Audio...)
    [ValueDropdown(nameof(GetCollections))] [SerializeField] [HideLabel] [InlineButton(nameof(Preview), "▶")]
    private AudioClipVariantId _sfx;

    private List<AudioClipVariantId> GetCollections => AudioManager.Instance.ContextContract.AudioIds;

    public void Play() {
      AudioManager.Instance.PlayOneShot(_sfx);
    }

    private void Preview() {
      AudioManager.Instance.GetAudioVariant(_sfx).Preview();
    }
  }
}