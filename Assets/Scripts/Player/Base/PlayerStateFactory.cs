public class PlayerStateFactory
{
    private PlayerController context;
    public PlayerStateFactory(PlayerController currentContext)
    {
        context = currentContext;
    }

    public PlayerBaseState CombatState()
    {
        return new CombatState(context, this);
    }
}
