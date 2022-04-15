using UnityEngine;

public class MealSourceView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private MealFactory _mealFactory;

    public void Init(MealFactory mealFactory)
    {
        _mealFactory = mealFactory;
    }

    public void View(MealType type)
    {
        _spriteRenderer.sprite = _mealFactory.GetSprite(type);
    }
}
