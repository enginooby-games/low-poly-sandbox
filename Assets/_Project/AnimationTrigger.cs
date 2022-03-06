using Animancer;
using UnityEngine;

// REFACTOR: Add directive and wrapper for Animancer
// + Prioritize the player controller, abandon current clip (fade in) whenever using controller

/// <summary>
/// Play serialized any animation clip without setup in the animator.
/// </summary>
public class AnimationTrigger : MonoBehaviour {
  [SerializeField] private AnimancerComponent _animancer;
  // ClipTransition: wrapper for AnimationClip to serialize params in Inspector
  [SerializeField] private ClipTransition _actionClip;
  [SerializeField] private AnimationClip _idleClip;
  [SerializeField] private KeyCode _triggerKey;

  // the playing clip before trigger action clip
  private AnimationClip _lastClip;
  private AnimationClip RestoreClip => _lastClip ? _lastClip : _idleClip;
  private bool _isActionPlaying;

  private void Update() {
    if (_triggerKey.IsDown()) {
      _animancer.enabled = true;
      
      if (_isActionPlaying) {
        PlayAction();
      }
      else {
        _lastClip = _animancer.Animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        PlayActionFromLastClip();
      }
    }
  }

  // TODO: Find better solution - replace idle with the current playing animation
  // So that it doesn't snap straight into the main action (suddenly play)
  private void PlayActionFromLastClip() {
    var state = _animancer.Play(RestoreClip, .1f, FadeMode.FromStart);
    state.Time = RestoreClip.length - .5f; // play only last .1s 
    state.Events.OnEnd = PlayAction;
  }

  private void PlayAction() {
    _isActionPlaying = true;
    var actionState = _animancer.Play(_actionClip, 0.25f, FadeMode.FromStart);
    // ether set Time or use FadeMode.FromStart
    // to play from beginning (animation will reset if trigger key during playing)
    // state.Time = 0; 
    actionState.Events.OnEnd = RestoreLastClip;
  }

  private void RestoreLastClip() {
    _isActionPlaying = false;
    var state = _animancer.Play(RestoreClip, 0.5f);
    state.Time = RestoreClip.length - .5f; 
    state.Events.OnEnd = DisableAnimancer;
  }

  // TODO: Find better solution
  // disable Animancer so that UCC can take over
  private void DisableAnimancer() => _animancer.enabled = false;
}