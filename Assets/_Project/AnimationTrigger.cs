#if ASSET_QUANTUM_CONSOLE
using QFSW.QC;
#else
using Enginooby.Attribute;
#endif

using Animancer;
using UnityEngine;

// REFACTOR: Add directive and wrapper for Animancer
// + Prioritize the player controller, abandon current clip (fade in) whenever using controller
// + Wrapper for action clip, trigger key, etc

/// <summary>
/// Play serialized any animation clip without setup in the animator.
/// </summary>
[CommandPrefix("a.")]
public class AnimationTrigger : MonoBehaviour {
  // ClipTransition: wrapper for AnimationClip to serialize params in Inspector
  [SerializeField] private ClipTransition _waveClip;
  [SerializeField] private KeyCode _waveTriggerKey;
  [SerializeField] private ClipTransition _danceClip;
  
  [SerializeField] private AnimancerComponent _animancer;
  [SerializeField] private AnimationClip _idleClip;

  // the playing clip before trigger action clip
  private AnimationClip _lastClip;
  private AnimationClip RestoreClip => _lastClip ? _lastClip : _idleClip;
  private bool _isActionPlaying;

  private void Update() {
    if (_waveTriggerKey.IsDown()) {
     Wave();
    }
  }

  [Command(nameof(Wave))]
  private void Wave() => TriggerAnimation(_waveClip);
  
  [Command(nameof(Dance))]
  private void Dance() => TriggerAnimation(_danceClip);

  private void TriggerAnimation(ITransition actionClip) {
    _animancer.enabled = true;

    if (_isActionPlaying) {
      PlayAction(actionClip);
    }
    else {
      _lastClip = _animancer.Animator.GetCurrentAnimatorClipInfo(0)[0].clip;
      PlayActionFromLastClip(actionClip);
    }
  }

  // TODO: Find better solution - replace idle with the current playing animation
  // So that it doesn't snap straight into the main action (suddenly play)
  private void PlayActionFromLastClip(ITransition actionClip) {
    var state = _animancer.Play(RestoreClip, .1f, FadeMode.FromStart);
    state.Time = RestoreClip.length - .5f; // play only last .1s 
    state.Events.OnEnd = () => PlayAction(actionClip);
  }

  private void PlayAction(ITransition actionClip) {
    _isActionPlaying = true;
    var actionState = _animancer.Play(actionClip, 0.25f, FadeMode.FromStart);
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