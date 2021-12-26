using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {
    #region Serialized Fields
    [SerializeField] private PlayerController2D _player = null;

    [SerializeField] private Fruit _fruitRes = null;
    [SerializeField] private float _boundX = 0;
    [SerializeField] private float _boundY = 0;
    [SerializeField] private float _spawningPeriod = 0;
    [SerializeField] private int _maxFruitCount = 0;
    #endregion

    #region Internal Fields
    private List<Fruit> _spawnedFruitList = new List<Fruit>();
    private float _remainedTimeToSpawn = -1;
    private bool _doSpawn = false;
    #endregion

    #region Properties
    private float BoundX {
        get {
            return Mathf.Max(0, _boundX);
        }
    }

    private float BoundY {
        get {
            return Mathf.Max(0, _boundY);
        }
    }

    private float SpawningPeriod {
        get {
            return Mathf.Max(0.1f, _spawningPeriod);
        }
    }

    private float MaxFruitCount {
        get {
            return Mathf.Max(0, _maxFruitCount);
        }
    }
    #endregion

    #region Mono Behaviour Hooks
    private void OnEnable() {
        EventManager.Instance.Register(EventID.FRUIT_RECEIVED, OnFruitReceived);
    }

    private void OnDisable() {
        EventManager.Instance.Register(EventID.FRUIT_RECEIVED, OnFruitReceived);
    }

    private void Update() {
        _remainedTimeToSpawn -= Time.deltaTime;

        _doSpawn = false;
        if (_remainedTimeToSpawn <= 0) {
            _doSpawn = true;
            _remainedTimeToSpawn = SpawningPeriod;
        }

        if (_doSpawn) {
            DoSpawn();
        }
    }
    #endregion

    #region Event Handlings
    private void OnFruitReceived(BaseEventArgs args) {
        FruitReceivedEventArgs frArgs = args as FruitReceivedEventArgs;

        Fruit f = frArgs.Fruit;
        bool removed = _spawnedFruitList.Remove(f);
        Destroy(f.gameObject);
    }
    #endregion

    #region Internal Methods
    private void DoSpawn() {
        if (_spawnedFruitList.Count >= MaxFruitCount) {
            return;
        }

        int retryCount = 10;
        bool posFound = false;

        Vector3 playerPos = _player.transform.position;
        Vector3 posCenter = this.transform.position;
        Vector3 rndPos = Vector3.zero;

        while (!posFound) {
            if (retryCount <= 0) {
                return;
            }

            retryCount -= 1;

            float rndPosX = Random.Range(posCenter.x - BoundX, posCenter.x + BoundX);
            float rndPosY = Random.Range(posCenter.y - BoundY, posCenter.y + BoundY);
            rndPos = new Vector3(rndPosX, rndPosY, posCenter.z);

            // NOTE:
            // Prevent spawn fruit on player
            if (Vector3.Distance(playerPos, rndPos) >= 1.0f) {
                posFound = true;
            }
        }

        Fruit newFruit = Instantiate(_fruitRes, rndPos, new Quaternion());
        _spawnedFruitList.Add(newFruit);
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmos() {
        Vector3 pos = this.transform.position;
        Vector3 spawnRangeLU = new Vector3(pos.x - BoundX, pos.y + BoundY, pos.z); // Point left up
        Vector3 spawnRangeLD = new Vector3(pos.x - BoundX, pos.y - BoundY, pos.z); // Point left down
        Vector3 spawnRangeRU = new Vector3(pos.x + BoundX, pos.y + BoundY, pos.z); // Point right up
        Vector3 spawnRangeRD = new Vector3(pos.x + BoundX, pos.y - BoundY, pos.z); // Point right down

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(spawnRangeLU, spawnRangeLD); // Left line
        Gizmos.DrawLine(spawnRangeRU, spawnRangeRD); // Right line
        Gizmos.DrawLine(spawnRangeLU, spawnRangeRU); // Up line
        Gizmos.DrawLine(spawnRangeLD, spawnRangeRD); // Down line

        Gizmos.DrawIcon(pos, "Apple");
    }
    #endregion
}
