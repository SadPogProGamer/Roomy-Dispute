using NUnit.Framework;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField] 
    private Transform[] _spawnPoints;
    [SerializeField]
    private int _playerCount;

    void Awake()
    {
        for (int playerIndex = 0; playerIndex < _playerCount; playerIndex++)
            Instantiate(_playerPrefab, new Vector3(_spawnPoints[playerIndex].position.x, _playerPrefab.transform.position.y, _spawnPoints[playerIndex].position.z), _spawnPoints[playerIndex].rotation); 
    }
}
