//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly BlinkStartComponent blinkStartComponent = new BlinkStartComponent();

    public bool isBlinkStart {
        get { return HasComponent(GameComponentsLookup.BlinkStart); }
        set {
            if (value != isBlinkStart) {
                var index = GameComponentsLookup.BlinkStart;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : blinkStartComponent;

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

    static Entitas.IMatcher<GameEntity> _matcherBlinkStart;

    public static Entitas.IMatcher<GameEntity> BlinkStart {
        get {
            if (_matcherBlinkStart == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.BlinkStart);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherBlinkStart = matcher;
            }

            return _matcherBlinkStart;
        }
    }
}
