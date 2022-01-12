using System.Collections;
using UnityEngine;

public class FruitBase : MonoBehaviour {
    public enum FruitType {
        Fruit2D,
        Fruit3D,
    }

    [SerializeField] protected bool _canBeRecieved = false;

    public FruitType FType {
        get;
        protected set;
    }

    public void SetCanBeRecieved(bool canReceive) {
        _canBeRecieved = canReceive;
    }
}
