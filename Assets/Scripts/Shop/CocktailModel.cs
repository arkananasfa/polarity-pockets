using UnityEngine;

[CreateAssetMenu(fileName = "CoctailModel", menuName = "Scriptable Objects/Coctail")]
public class CocktailModel : ScriptableObject
{
    public Sprite icon;
    public string name;
    public string description;
    public int price;
    public CocktailType type;
}

public enum CocktailType
{
    ReversePolarity,
    PocketPlus,
    Teleport,
    ChaosBomb
}