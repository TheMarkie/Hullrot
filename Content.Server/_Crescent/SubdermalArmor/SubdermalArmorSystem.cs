using Content.Server.Cargo.Systems;
using Content.Shared._Crescent.SubdermalArmor;
using Robust.Shared.Prototypes;
using Content.Shared.Damage.Prototypes;

namespace Content.Server._Crescent.SubdermalArmor;

/// <inheritdoc/>
public sealed class ArmorSystem : SharedSubdermalArmorSystem
{
    [Dependency] private readonly IPrototypeManager _protoManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SubdermalArmorComponent, PriceCalculationEvent>(GetArmorPrice);
    }

    private void GetArmorPrice(EntityUid uid, SubdermalArmorComponent component, ref PriceCalculationEvent args)
    {
        foreach (var modifier in component.Modifiers.Coefficients)
        {
            var damageType = _protoManager.Index<DamageTypePrototype>(modifier.Key);
            args.Price += component.PriceMultiplier * damageType.ArmorPriceCoefficient * 100 * (1 - modifier.Value);
        }

        foreach (var modifier in component.Modifiers.FlatReduction)
        {
            var damageType = _protoManager.Index<DamageTypePrototype>(modifier.Key);
            args.Price += component.PriceMultiplier * damageType.ArmorPriceFlat * modifier.Value;
        }
    }
}
