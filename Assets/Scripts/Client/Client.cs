using System;
using UnityEngine;

public class Client : MonoBehaviour, IUpdatable
{
    [SerializeField] private ClientView _view;
    [SerializeField] private float _movementTime = 3.5f;
    
    private float _speed;
    private OrderPoint _orderPoint;
    private Transform _endPoint;
    private ClientState _state;

    public Order Order { get; private set; }
    public event Action<Client> OrderPointReached;
    public event Action<Client, OrderPoint> OrderCompleted;

    public void Init(OrderData order, MealFactory mealFactory)
    {
        Order = new Order(order.Meals);
        _view.Init(Order.Meals, mealFactory);

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
                if (Order.IsServed)
                    HandleCompletingOrder();
                break;
            case ClientState.Waning:
                MoveToEndPoint(deltaTime);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void CompleteOrder()
    {
        Order.Complete();
        HandleCompletingOrder();
    }

    public bool TryReceive(MealType type)
    {
        if (Order.TryReceive(type))
        {
            _view.UpdateOrder(Order.Meals);
            return true;
        }

        return false;
    }

    public void LetIn(OrderPoint point, Transform endPoint)
    {
        _orderPoint = point;
        _endPoint = endPoint;
        _speed = (point.Position - transform.position).magnitude / _movementTime;

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
