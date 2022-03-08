using UnityEngine;
using VikingCrewTools.UI; // TODO: Use directive and move script to library
#if ASSET_QUANTUM_CONSOLE
using QFSW.QC;
#else
using Enginooby.Attribute; // TODO: Avoid clean import in conditional compiling by IDE
#endif

// TODO: Add speech modes - thinking, angry, etc

public class SpeechManager : MonoBehaviourSingleton<SpeechManager> {
    private Transform _player;

    private void Start() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    [Command("say")]
    public void SayByPlayer(string text) {
        SpeechBubbleManager.Instance.AddSpeechBubble(_player, text);
    }
    
    public static void Say(string text, Transform speaker) {
        SpeechBubbleManager.Instance.AddSpeechBubble(speaker, text);
    }
}
