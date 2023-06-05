//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly BlinkingCooldownComponent blinkingCooldownComponent = new BlinkingCooldownComponent();

    public bool isBlinkingCooldown {
        get { return HasComponent(GameComponentsLookup.BlinkingCooldown); }
        set {
            if (value != isBlinkingCooldown) {
                var index = GameComponentsLookup.BlinkingCooldown;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : blinkingCooldownComponent;

                    AddComponent(index, component);
                } else {
                    RemoveComponent(index);
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherBlinkingCooldown;

    public static Entitas.IMatcher<GameEntity> BlinkingCooldown {
        get {
            if (_matcherBlinkingCooldown == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.BlinkingCooldown);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherBlinkingCooldown = matcher;
            }

            return _matcherBlinkingCooldown;
        }
    }
}
