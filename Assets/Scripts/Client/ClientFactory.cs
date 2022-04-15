using UnityEngine;

[CreateAssetMenu]
public class ClientFactory : ScriptableObject
{
    [SerializeField] private Client _clientPrefab;

    public Client GetClient(Vector3 position, Transform parent)
    {
        return Instantiate(_clientPrefab, position, Quaternion.identity, parent);
    }
}
