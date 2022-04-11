using System;
using System.Collections.Generic;
using Enginooby.Utils;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;

#else
using Enginooby.Attribute;
#endif

namespace Enginooby.Core {
  // TODO: generic, return value
  /// <summary>
  ///   Wrapper of UnityEvent for GUI binding (better designing) and delegate for script binding (better performance).
  /// </summary>
  [Serializable]
  [InlineProperty]
  public class Event {
    private string _eventName;

    private string EventName {
      get {
        if (_eventName == null || _eventName.IsEmpty()) return "Event";
        return _eventName.ToSentenceUpperCase();
      }
    }

    public Event(string eventName) {
      _eventName = eventName;
      Listeners = new List<Action>();
    }

    [ToggleGroup(nameof(Enabled), "$" + nameof(EventName))] [SerializeField]
    public bool Enabled = true;

    // TODO: Wrap SerializableEvent from SerializableCallback asset
    // ? Use UltEvent (UnityEvent is for fallback)
    [ToggleGroup(nameof(Enabled), "$" + nameof(EventName))] [SerializeField] [HideLabel]
    private UnityEvent _unityEvent = new();

    // ? Display script binding listeners in the inspector
    private event Action Action;

    public List<Action> Listeners;

    // REFACTOR: Use add/remove accessors?
    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/add
    public static Event operator +(Event @event, Action listener) {
      if (@event.Listeners.IsNullOrEmpty()) @event.Listeners = new List<Action>();

      // TIP: Unsubscribe event before subscribing to prevent double-subscription in unexpected situations
      @event -= listener;
      @event.Action += listener;
      @event.Listeners.Add(listener);
      return @event;
    }

    public static Event operator -(Event @event, Action listener) {
      if (@event.Listeners.IsNullOrEmpty()) @event.Listeners = new List<Action>();

      @event.Action -= listener;
      @event.Listeners.Remove(listener);
      return @event;
    }

    /// <summary>
    /// Return number of persistent listeners (GUI-binding, which is assigned in the inspector)
    /// </summary>
    public int InspectorBindingCount => _unityEvent.GetPersistentEventCount();

    // TODO: Auto unsubscribe on Disable of MonoBehaviour or exit app
    public void RemoveListeners() {
      if (Listeners.IsNullOrEmpty()) Listeners = new List<Action>();

      foreach (var listener in Listeners) Action -= listener;
    }

    public void Invoke() {
      if (!Enabled) return;

      _unityEvent?.Invoke();
      Action?.Invoke();
    }

    public void Invoke(bool @if) {
      if (@if) Invoke();
    }
  }
}