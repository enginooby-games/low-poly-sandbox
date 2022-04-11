using Enginooby.Attribute;
using UnityEngine;

namespace Enginooby.Core {
  public class EventTest : MonoBehaviour {
    public Event OnGameOver;

    private void Start() {
      OnGameOver += () => { print("OnGameOver"); };
    }

    [Button]
    private void TriggerGameOver() {
      OnGameOver?.Invoke();
    }

    public void SayHi() => print("Hi");
  }
}