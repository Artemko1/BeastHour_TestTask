//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class InputContext {

    public InputEntity lmbInputEntity { get { return GetGroup(InputMatcher.LmbInput).GetSingleEntity(); } }

    public bool isLmbInput {
        get { return lmbInputEntity != null; }
        set {
            var entity = lmbInputEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isLmbInput = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class InputEntity {

    static readonly LmbInputComponent lmbInputComponent = new LmbInputComponent();

    public bool isLmbInput {
        get { return HasComponent(InputComponentsLookup.LmbInput); }
        set {
            if (value != isLmbInput) {
                var index = InputComponentsLookup.LmbInput;
                if (value) {
                    var componentPool = GetComponentPool(index);
                    var component = componentPool.Count > 0
                            ? componentPool.Pop()
                            : lmbInputComponent;

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
public sealed partial class InputMatcher {

    static Entitas.IMatcher<InputEntity> _matcherLmbInput;

    public static Entitas.IMatcher<InputEntity> LmbInput {
        get {
            if (_matcherLmbInput == null) {
                var matcher = (Entitas.Matcher<InputEntity>)Entitas.Matcher<InputEntity>.AllOf(InputComponentsLookup.LmbInput);
                matcher.componentNames = InputComponentsLookup.componentNames;
                _matcherLmbInput = matcher;
            }

            return _matcherLmbInput;
        }
    }
}
