using UnityEngine;

public static class PolarityFactory
{

    public static Polarity CreatePolarity(PolaritiyType type) => type switch
    {
        PolaritiyType.Positive => new PositivePolarity(),
        PolaritiyType.Negative => new NegativePolarity(),
        PolaritiyType.MegaPositive => new MegaPositivePolarity(),
        _ => new PositivePolarity(),
    };

}