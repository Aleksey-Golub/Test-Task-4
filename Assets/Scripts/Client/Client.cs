using System;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour, IUpdatable
{
    [SerializeField] private ClientView _view;
    [SerializeField] private float _speed = 1f;
    
    private List<MealType> _order = new List<MealType>();
    private OrderPoint _orderPoint;
    private Transform _endPoint;
    private ClientState _state;

    public bool IsServed => _order.Count == 0;
    public event Action<Client> OrderPointReached;
    public event Action<Client, OrderPoint> OrderCompleted;

    public void Init(OrderData order, MealFactory mealFactory)
    {
        _order.AddRange(order.Meals);
        _view.Init(_order, mealFactory);

        _state = ClientState.Spawned;
    }

    public void CustomUpdate(float deltaTime)
    {
        switch (_state)
        {
            case ClientState.Spawned:
                break;
            case ClientState.Arraving:
                if (MoveToOrderPoint(deltaTime))
                {
                    OrderPointReached?.Invoke(this);
                    _view.ShowOrder();
                    _state = ClientState.WaitingForOrder;
                }
                break;
            case ClientState.WaitingForOrder:
                if (_order.Count == 0)
                    HandleCompletingOrder();
                break;
            case ClientState.Waning:
                MoveToEndPoint(deltaTime);
                break;
            default:
                break;
        }
    }

    public void CompleteOrder()
    {
        _order.Clear();

        HandleCompletingOrder();
    }

    public bool TryReceive(MealType type)
    {
        for (int i = 0; i < _order.Count; i++)
        {
            if (_order[i] == type)
            {
                _order.Remove(_order[i]);
                _view.UpdateOrder(_order);
                return true;
            }
        }
        return false;
    }

    public void LetIn(OrderPoint point, Transform endPoint)
    {
        _orderPoint = point;
        _endPoint = endPoint;
        
        _state = ClientState.Arraving;
    }

    private void HandleCompletingOrder()
    {
        _view.HideOrder();
        OrderCompleted?.Invoke(this, _orderPoint);
        _state = ClientState.Waning;
    }

    private void MoveToEndPoint(float deltaTime)
    {
        MoveTo(_endPoint.position, deltaTime);
    }

    private bool MoveToOrderPoint(float deltaTime)
    {
        MoveTo(_orderPoint.Position, deltaTime);
        return transform.position == _orderPoint.Position;
    }

    private void MoveTo(Vector3 position, float deltaTime)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, deltaTime * _speed);
    }
}
