using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField] 
    private Transform[] _spawnPoints;
    [SerializeField]
    private Material[] _materials;
    [SerializeField]
    private int _playerCount;

    void Awake()
    {
        _playerCount = Gamepad.all.Count;
        for (int playerIndex = 0; playerIndex < _playerCount; playerIndex++)
            Instantiate(_playerPrefab, new Vector3(_spawnPoints[playerIndex].position.x, _playerPrefab.transform.position.y, _spawnPoints[playerIndex].position.z), _spawnPoints[playerIndex].rotation).GetComponent<MeshRenderer>().material = _materials[playerIndex]; 
    }
}
