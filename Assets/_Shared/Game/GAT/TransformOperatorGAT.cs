using System.Collections.Generic;

namespace Enginooby.Prototype {
  public class TransformOperatorGAT : GameActionTarget<TransformOperator> {
    protected override List<string> Actions => new List<string> {"Stop", "Destroy", "DestroyGO"}; // REFACTOR

    public override void OnActionPerformed() {
      if (_action == "Stop") {
        TargetReferences.ForEach(e => e.Stop());
      }
    }
  }
}