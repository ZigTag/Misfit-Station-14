using Content.Shared._Misfit.Species.Components;
using Content.Shared.Damage;
using Content.Shared.FixedPoint;
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
    [Dependency] private readonly SharedHumanoidAppearanceSystem _humanoid = default!;

    private static readonly FixedPoint2 TotalHealth = 200;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<EtherealColorComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<EtherealColorComponent, DamageChangedEvent>(OnDamageChanged);
    }

    private void OnComponentStartup(EntityUid uid, EtherealColorComponent component, ComponentStartup args)
    {
        if (!_pointLight.TryGetLight(uid, out var lightComp))
            return;

        if (!_entity.TryGetComponent<HumanoidAppearanceComponent>(uid, out var appearanceComp))
            return;

        _pointLight.SetColor(uid, appearanceComp.SkinColor, lightComp);

        component.InitialColor = appearanceComp.SkinColor;
    }

    private void OnDamageChanged(EntityUid uid, EtherealColorComponent component, DamageChangedEvent args)
    {
        var scalar = Math.Clamp(((TotalHealth - args.Damageable.TotalDamage) / TotalHealth).Float(), 0, 1);
        Color newColor = new (Color.White.RGBA - (Color.White.RGBA - component.InitialColor.RGBA) * scalar);
        _humanoid.SetSkinColor(uid, newColor);
        _pointLight.SetColor(uid, newColor);
    }
}
