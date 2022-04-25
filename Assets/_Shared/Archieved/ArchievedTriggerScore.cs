using Enginooby.Prototype;

public class ArchievedTriggerScore : ArchievedTriggerStat<int> {
  // TIP: Reset() is invoked when Component first added or being Reset (in Inspector)
  // Hence useful for ovveride defaut values of base class 
  public override string StatLabel => "Score amount";

  private void Reset() {
    statValue = 1;
    triggerEvent = TriggerEventType.OnTriggerEnter;
  }

  public override void UpdateStat() {
#if STAT_SCORE
    StatManager.Instance.Score.Add(statValue);
#endif
  }
}