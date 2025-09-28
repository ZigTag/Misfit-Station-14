using Content.Shared._Misfit.Species.Components;
using Content.Shared.Humanoid;
using Linguini.Syntax.Ast;

namespace Content.Shared._Misfit.Species.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class EtherealColorSystem : EntitySystem
{
    [Dependency] private readonly IEntityManager _entity = default!;
    [Dependency] private readonly SharedPointLightSystem _pointLight = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<EtherealColorComponent, ComponentStartup>(OnComponentStartup);
    }

    private void OnComponentStartup(EntityUid uid, EtherealColorComponent component, ComponentStartup args)
    {
        if (!_pointLight.TryGetLight(uid, out var lightComp))
            return;

        if (!_entity.TryGetComponent<HumanoidAppearanceComponent>(uid, out var appearanceComp))
            return;

        _pointLight.SetColor(uid, appearanceComp.SkinColor, lightComp);
    }
}
