using Animancer;
using UnityEngine;

// REFACTOR: Add directive and wrapper for Animancer

/// <summary>
/// Play serialized any animation clip without setup in the animator.
/// </summary>
public class AnimationTrigger : MonoBehaviour {
  [SerializeField] private AnimancerComponent _animancer;
  [SerializeField] private AnimationClip _clip;
  [SerializeField] private AnimationClip _idleClip;
  [SerializeField] private KeyCode _triggerKey;

  private void Update() {
    if (_triggerKey.IsDown()) {
      _animancer.enabled = true;
      var state =  _animancer.Play(_clip, .5f);
      state.Events.OnEnd = DisableAnimancer;
    }
  }

  private void PlayIdleClip() {
    // Now that the action is done, go back to idle. But instead of snapping to the new animation instantly,
    // tell it to fade gradually over 0.25 seconds so that it transitions smoothly.
    _animancer.Play(_idleClip, 0.25f).Events.OnEnd = DisableAnimancer ;
  }

  // TODO: Find better solution
  // disable Animancer so that UCC can take over
  private void DisableAnimancer() => _animancer.enabled = false;
}