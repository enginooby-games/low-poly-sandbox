using UnityEngine;

namespace Enginooby.Audio {
  [RequireComponent(typeof(AudioSource))]
  public class AudioSourceOperator : ComponentOperator<AudioSource> {
    [SerializeField] private AudioClipContext _audioClipContext;

    public AudioSource AudioSource { get; private set; }

    protected override void Awake() {
      AudioSource = GetComponent<AudioSource>();
    }
  }
}