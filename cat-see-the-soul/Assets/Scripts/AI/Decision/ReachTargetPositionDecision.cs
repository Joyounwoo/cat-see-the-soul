using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ReachTargetPosition")]
public class ReachTargetPositionDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        if (controller.character.GetTargetDistance() < 0.02f*GameManager.GetGameSpeed())
            return true;
        else
            return false;
    }
}
