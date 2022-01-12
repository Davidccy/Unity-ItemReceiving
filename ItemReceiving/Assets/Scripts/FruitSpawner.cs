using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour {
    public enum FruitType { 
        Fruit2D,
        Fruit3D,
    }

    #region Serialized Fields
    [SerializeField] private PlayerControllerBase _player = null;
    [SerializeField] private FruitType _fruitType = FruitType.Fruit2D;
    [SerializeField] private Fruit2D _fruit2DRes = null;
    [SerializeField] private Fruit3D _fruit3DRes = null;
    [SerializeField] private float _boundX = 0;
    [SerializeField] private float _boundY = 0;
    [SerializeField] private float _spawningPeriod = 0;
    [SerializeField] private int _maxFruitCount = 0;
    [SerializeField] private bool _avoidOnPlayer = false;
    #endregion

    #region Internal Fields
    private List<FruitBase> _spawnedFruitList = new List<FruitBase>();
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

    public float SpawningPeriod {
        get {
            return Mathf.Max(0.1f, _spawningPeriod);
        }
        set {
            _spawningPeriod = Mathf.Max(0.1f, value);
        }
    }

    public int MaxFruitCount {
        get {
            return Mathf.Max(0, _maxFruitCount);
        }
        set {
            _maxFruitCount = Mathf.Max(0, value);
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
        FruitBase f = frArgs.Fruit;
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
            if (!_avoidOnPlayer) {
                posFound = true;
            }
            else {
                if (Vector3.Distance(playerPos, rndPos) >= 1.0f) {
                    posFound = true;
                }
            }
        }

        FruitBase fruit = Instantiate(GetFruitRes(_fruitType), rndPos, GetFruitInitQuaternion(_fruitType), this.transform);
        _spawnedFruitList.Add(fruit);
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

    #region Internal Methods
    private FruitBase GetFruitRes(FruitType ft) {
        if (ft == FruitType.Fruit2D) {
            return _fruit2DRes;
        }

        return _fruit3DRes;
    }

    private Quaternion GetFruitInitQuaternion(FruitType ft) {
        Quaternion q = new Quaternion();
        if (ft == FruitType.Fruit2D) {
            return q;
        }

        q.eulerAngles = new Vector3(-90, 0, 0);
        return q;
    }
    #endregion
}
