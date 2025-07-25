using UnityEngine;

public static class PolarityFactory
{

    public static Polarity CreatePolarity(PolaritiyType type) => type switch
    {
        PolaritiyType.Positive => new PositivePolarity(),
        PolaritiyType.Negative => new NegativePolarity(),
        PolaritiyType.MegaPositive => new MegaPositivePolarity(),
        PolaritiyType.HighMass => new HighMass(),
        PolaritiyType.Defeat => new DefeatPolarity(),
        PolaritiyType.Bomb => new BombPolarity(),
        _ => new PositivePolarity(),
    };

}