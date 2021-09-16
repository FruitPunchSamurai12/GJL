using Pathfinding;

public class Idle : IState
{
    NPC _ai;
    AIPath _aiPath;
    public Idle(NPC ai, AIPath path)
    {
        _ai = ai;
        _aiPath = path;
    }

    public void OnEnter()
    {
        _aiPath.enabled = false;
        _ai.SetAnimatorBool("Move", false);
        _ai.ChangeMaterial(1);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }
}
