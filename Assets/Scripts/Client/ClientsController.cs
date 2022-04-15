using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientsController : MonoBehaviour, IUpdatable
{
    [SerializeField] private ClientFactory _clientFactory;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private OrderPoint[] _orderPoints;
    [SerializeField] private float _clientArrivalInterval = 3f;

    private float _timer = 0;
    private int _nextClientIndex = 0;
    private List<Client> _allClients;
    private List<Client> _waitingForOrder = new List<Client>();

    public event Action<int, int> ClientCountChanged;
    public event Action AllCliensAreServed;

    public void Init(OrderData[] orders, MealFactory mealFactory)
    {
        _allClients = new List<Client>(orders.Length);
        foreach (var o in orders)
        {
            Client newClient = _clientFactory.GetClient(_spawnPoint.position, transform);
            newClient.Init(o, mealFactory);
            newClient.OrderPointReached += OnClientOrderPointReached;
            newClient.OrderCompleted += OnClientOrderCompleted;
            _allClients.Add(newClient);
        }
        ClientCountChanged?.Invoke(_allClients.Count - _nextClientIndex, _allClients.Count);
    }

    public void CustomUpdate(float deltaTime)
    {
        _timer += deltaTime;
        if (NeedToLetInNewClient())
            LetInNewClient();

        foreach (var c in _allClients)
        {
            c.CustomUpdate(deltaTime);
        }

        CheckAllCliensAreServed();
    }

    public bool TryReceive(MealType type)
    {
        for (int i = 0; i < _waitingForOrder.Count; i++)
        {
            if (_waitingForOrder[i].TryReceive(type))
            {
                return true;
            }
        }
        return false;
    }

    public bool CompleteFirstInQueueOrder()
    {
        if (_waitingForOrder.Count == 0)
            return false;

        _waitingForOrder[0].CompleteOrder();
        return true;
    }

    private void CheckAllCliensAreServed()
    {
        for (int j = 0; j < _allClients.Count; j++)
        {
            if (_allClients[j].Order.IsServed == false)
                break;
            else if (j == _allClients.Count - 1)
                AllCliensAreServed?.Invoke();
        }
    }

    private bool NeedToLetInNewClient()
    {
        return _timer >= _clientArrivalInterval && HasFreeOrderPoint(out OrderPoint point) && _nextClientIndex < _allClients.Count;
    }

    private void LetInNewClient()
    {
        OrderPoint point;
        HasFreeOrderPoint(out point);
        point.IsOccupied = true;

        _allClients[_nextClientIndex].LetIn(point, _endPoint);
        _nextClientIndex++;
        ClientCountChanged?.Invoke(_allClients.Count - _nextClientIndex, _allClients.Count);
        _timer -= _clientArrivalInterval;
    }

    private void OnClientOrderPointReached(Client client)
    {
        _waitingForOrder.Add(client);
        client.OrderPointReached -= OnClientOrderPointReached;
    }

    private void OnClientOrderCompleted(Client client, OrderPoint orderPoint)
    {
        _waitingForOrder.Remove(client);
        orderPoint.IsOccupied = false;
    }

    private bool HasFreeOrderPoint(out OrderPoint orderPoint)
    {
        orderPoint = null;
        foreach (var p in _orderPoints)
        {
            if (p.IsOccupied == false)
            {
                orderPoint = p;
                return true;
            }
        }
        return false;
    }
}
