public class FixedUpdateSystems : Feature
{
    public FixedUpdateSystems(Contexts contexts)
    {
        // Physics events
        Add(new HitOnDashTriggerSystem(contexts));
    }
}