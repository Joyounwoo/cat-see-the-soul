using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/HearSound")]
public class HearSoundDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool isHeared = IsHeared(controller);
        return isHeared;
    }
    
    private bool IsHeared(StateController controller)
    {
        return true;
    }
}
