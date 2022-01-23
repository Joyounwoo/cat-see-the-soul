using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Walk")]
public class WalkAction : Action
{
    public override void Act(StateController controller)
    {
        controller.character.MoveToPosition(controller.character.targetPosition, 1);
    }
}
