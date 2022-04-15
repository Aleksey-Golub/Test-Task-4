using System.Collections.Generic;

public class Order
{
    private List<MealType> _meals;

	public bool IsServed => _meals.Count == 0;
	public IReadOnlyList<MealType> Meals => _meals;

	public Order(MealType[] order)
	{
		_meals = new List<MealType>(order.Length);
		_meals.AddRange(order);
	}

	public void Complete()
	{
		_meals.Clear();
	}

	internal bool TryReceive(MealType type)
	{
		for (int i = 0; i < _meals.Count; i++)
		{
			if (_meals[i] == type)
			{
				_meals.Remove(_meals[i]);
				
				return true;
			}
		}
		return false;
	}
}
