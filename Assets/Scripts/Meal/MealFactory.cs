using System;
using UnityEngine;

[CreateAssetMenu]
public class MealFactory : ScriptableObject
{
    [SerializeField] private Sprite _orangeJuice;
    [SerializeField] private Sprite _pancakes;
    [SerializeField] private Sprite _schnitzel;
    [SerializeField] private Sprite _banana;

    public Sprite GetSprite(MealType type)
    {
        switch (type)
        {
            case MealType.OrangeJuice:
                return _orangeJuice;
            case MealType.Pancakes:
                return _pancakes;
            case MealType.Schnitzel:
                return _schnitzel;
            case MealType.Banana:
                return _banana;
            default:
                throw new NotImplementedException();
        }
    }
}
