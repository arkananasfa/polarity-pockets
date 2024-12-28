using UnityEngine;

public static class ResourcesManager
{
    
    public static ObservableValue<int> Money = new(0);
    public static ObservableValue<int> Tries = new(10);
    
}