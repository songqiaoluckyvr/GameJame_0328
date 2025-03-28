using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    #region Map Parameters
    [Header("Tile Settings")]
    [SerializeField] private GameObject _startTilePrefab;
    [SerializeField] private GameObject _endTilePrefab;
    [SerializeField] private GameObject[] _connectingTilePrefabs;
    [SerializeField] private float _tileLength = 30f;
    [SerializeField] private int _activeTileCount = 3;
    [SerializeField] private bool _debugMode = true;
    #endregion

    #region State Variables
    private List<GameObject> _activeTiles = new List<GameObject>();
    private Transform _playerTransform;
    private int _currentTileIndex = 0;
    private bool _isInitialized = false;
    private bool _hasRemovedStartTile = false;
    private int _tileRotationCount = 0;
    private bool _hasSpawnedEndTile = false;
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
            Debug.LogError("[MapManager] Player not found! Make sure it has the 'Player' tag.");
            return;
        }

        InitializeMap();
    }

    private void Update()
    {
        if (!_isInitialized || _playerTransform == null) return;

        CheckAndRecycleTiles();
    }
    #endregion

    #region Map Generation
    private void InitializeMap()
    {
        if (_startTilePrefab == null || _endTilePrefab == null || _connectingTilePrefabs == null || _connectingTilePrefabs.Length == 0)
        {
            Debug.LogError("[MapManager] Missing tile prefabs!");
            return;
        }

        // Create initial tiles
        for (int i = 0; i < _activeTileCount; i++)
        {
            GameObject tile;
            if (i == 0)
            {
                // First tile is always start tile
                tile = Instantiate(_startTilePrefab);
            }
            else
            {
                // Other tiles are connecting tiles
                tile = Instantiate(_connectingTilePrefabs[Random.Range(0, _connectingTilePrefabs.Length)]);
            }

            // Position the tile
            tile.transform.position = new Vector3(i * _tileLength, 0, 0);
            _activeTiles.Add(tile);
        }

        _isInitialized = true;
        if (_debugMode)
        {
            Debug.Log($"[MapManager] Initialized map with {_activeTileCount} tiles");
        }
    }

    private void CheckAndRecycleTiles()
    {
        // Get the tile index the player is currently on
        int playerTileIndex = Mathf.FloorToInt(_playerTransform.position.x / _tileLength);

        // Check if we need to recycle tiles
        if (playerTileIndex > _currentTileIndex)
        {
            // Player moved right
            if (!_hasRemovedStartTile && playerTileIndex > 1)
            {
                RemoveStartTile();
            }
            else
            {
                RecycleTileToRight();
                _tileRotationCount++;
                
                if (_debugMode)
                {
                    Debug.Log($"[MapManager] Tile rotation count: {_tileRotationCount}");
                }
            }
            _currentTileIndex = playerTileIndex;
        }
        else if (playerTileIndex < _currentTileIndex)
        {
            // Player moved left, only allow if not past start position
            if (playerTileIndex >= 0)
            {
                RecycleTileToLeft();
                _tileRotationCount--;
                
                if (_debugMode)
                {
                    Debug.Log($"[MapManager] Tile rotation count: {_tileRotationCount}");
                }
            }
            _currentTileIndex = playerTileIndex;
        }

        // Check if we should spawn the end tile
        if (!_hasSpawnedEndTile && _tileRotationCount >= 10)
        {
            SpawnEndTile();
        }
    }

    private void RemoveStartTile()
    {
        if (_activeTiles.Count > 0)
        {
            // Remove the start tile
            GameObject startTile = _activeTiles[0];
            _activeTiles.RemoveAt(0);
            Destroy(startTile);

            // Spawn a new connecting tile at the end
            GameObject newTile = Instantiate(_connectingTilePrefabs[Random.Range(0, _connectingTilePrefabs.Length)]);
            float newX = _activeTiles[_activeTiles.Count - 1].transform.position.x + _tileLength;
            newTile.transform.position = new Vector3(newX, 0, 0);
            _activeTiles.Add(newTile);

            _hasRemovedStartTile = true;
            
            if (_debugMode)
            {
                Debug.Log("[MapManager] Start tile removed and replaced with connecting tile");
            }
        }
    }

    private void SpawnEndTile()
    {
        if (_activeTiles.Count > 0)
        {
            // Remove the last tile
            GameObject lastTile = _activeTiles[_activeTiles.Count - 1];
            _activeTiles.RemoveAt(_activeTiles.Count - 1);
            Destroy(lastTile);

            // Spawn the end tile
            GameObject endTile = Instantiate(_endTilePrefab);
            float newX = _activeTiles[_activeTiles.Count - 1].transform.position.x + _tileLength;
            endTile.transform.position = new Vector3(newX, 0, 0);
            _activeTiles.Add(endTile);

            _hasSpawnedEndTile = true;
            
            if (_debugMode)
            {
                Debug.Log("[MapManager] End tile spawned");
            }
        }
    }

    private void RecycleTileToRight()
    {
        if (_activeTiles.Count == 0) return;

        // Get the leftmost tile
        GameObject tileToRecycle = _activeTiles[0];
        _activeTiles.RemoveAt(0);

        // If we've already spawned the end tile, don't recycle anymore
        if (_hasSpawnedEndTile)
        {
            Destroy(tileToRecycle);
            return;
        }

        // Calculate new position (rightmost + tileLength)
        float newX = _activeTiles[_activeTiles.Count - 1].transform.position.x + _tileLength;
        tileToRecycle.transform.position = new Vector3(newX, 0, 0);

        // Add to end of list
        _activeTiles.Add(tileToRecycle);

        if (_debugMode)
        {
            Debug.Log($"[MapManager] Recycled tile to right at x={newX}");
        }
    }

    private void RecycleTileToLeft()
    {
        // Don't allow recycling left if we've removed the start tile
        if (_hasRemovedStartTile) return;

        if (_activeTiles.Count == 0) return;

        // Get the rightmost tile
        GameObject tileToRecycle = _activeTiles[_activeTiles.Count - 1];
        _activeTiles.RemoveAt(_activeTiles.Count - 1);

        // Calculate new position (leftmost - tileLength)
        float newX = _activeTiles[0].transform.position.x - _tileLength;
        tileToRecycle.transform.position = new Vector3(newX, 0, 0);

        // Add to start of list
        _activeTiles.Insert(0, tileToRecycle);

        if (_debugMode)
        {
            Debug.Log($"[MapManager] Recycled tile to left at x={newX}");
        }
    }
    #endregion

    #region Public Methods
    public void ResetMap()
    {
        // Clean up existing tiles
        foreach (GameObject tile in _activeTiles)
        {
            Destroy(tile);
        }
        _activeTiles.Clear();

        // Reset state
        _currentTileIndex = 0;
        _isInitialized = false;
        _hasRemovedStartTile = false;
        _tileRotationCount = 0;
        _hasSpawnedEndTile = false;

        // Reinitialize
        InitializeMap();
    }
    #endregion
} 