using UnityEngine;
using System.Collections;

public class AntidoteManager : MonoBehaviour
{
    #region Spawn Parameters
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _antidotePrefab;
    [SerializeField] private float _spawnHeight = 15f;
    [SerializeField] private float _minSpawnInterval = 3f;
    [SerializeField] private float _maxSpawnInterval = 7f;
    [SerializeField] private float _minSpawnDistance = -5f;  // Behind player
    [SerializeField] private float _maxSpawnDistance = 15f;  // Ahead of player
    [SerializeField] private bool _debugMode = true;
    #endregion

    #region Component References
    private Transform _playerTransform;
    #endregion

    #region State Variables
    private bool _isSpawning = true;
    private Coroutine _spawnCoroutine;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        // Find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("[AntidoteManager] Player not found! Make sure it has the 'Player' tag.");
            return;
        }


        StartSpawning();
    }

    private void OnDisable()
    {
        StopSpawning();
    }
    #endregion

    #region Spawn Methods
    private void StartSpawning()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void StopSpawning()
    {
        _isSpawning = false;
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (_isSpawning)
        {
            SpawnAntidote();
            float interval = Random.Range(_minSpawnInterval, _maxSpawnInterval);
            
            if (_debugMode)
            {
                Debug.Log($"[AntidoteManager] Next antidote spawn in {interval:F1} seconds");
            }
            
            yield return new WaitForSeconds(interval);
        }
    }

    private void SpawnAntidote()
    {
        if (_playerTransform == null || _antidotePrefab == null) return;

        // Calculate random spawn position relative to player
        float randomOffset = Random.Range(_minSpawnDistance, _maxSpawnDistance);
        float spawnX = _playerTransform.position.x + randomOffset;
        Vector3 spawnPosition = new Vector3(spawnX, _spawnHeight, 5f);

        // Spawn antidote
        GameObject antidote = Instantiate(_antidotePrefab, spawnPosition, Quaternion.identity);
        
        // Add Antidote component
        Antidote antidoteComponent = antidote.GetComponent<Antidote>();
        if (antidoteComponent == null)
        {
            antidoteComponent = antidote.AddComponent<Antidote>();
        }

        if (_debugMode)
        {
            Debug.Log($"[AntidoteManager] Spawned antidote at {spawnPosition}, offset from player: {randomOffset:F1}");
        }
    }
    #endregion

    #region Public Methods
    public void ResetManager()
    {
        StopSpawning();
        // Destroy all existing antidotes
        foreach (Antidote antidote in FindObjectsOfType<Antidote>())
        {
            Destroy(antidote.gameObject);
        }
        _isSpawning = true;
        StartSpawning();
    }
    #endregion
} 