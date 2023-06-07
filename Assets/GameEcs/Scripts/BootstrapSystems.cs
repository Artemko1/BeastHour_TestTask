public class BootstrapSystems : Feature
{
    public BootstrapSystems(Contexts contexts)
    {
        Add(new BootstrapSystem(contexts));
    }
}