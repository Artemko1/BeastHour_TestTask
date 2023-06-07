using Entitas.CodeGeneration.Attributes;

[Config, Unique, ComponentName("AiConfig")]
public interface IAiConfig
{
    bool DashEnabled { get; }
    float DashCooldown { get; }
}