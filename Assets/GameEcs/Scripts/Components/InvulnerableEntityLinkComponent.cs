using Entitas;


// Ссылка на сущность с инфой о неуязвимости. Компонент должен быть добавлен к персонажу
[Game]
public sealed class InvulnerableEntityLinkComponent : IComponent
{
    public GameEntity Value;
}