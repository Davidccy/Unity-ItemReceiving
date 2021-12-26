using UnityEngine;

public class BaseEventArgs {
    public EventID ID {
        get;
        protected set;
    }

    public void Dispatch() {
        EventManager.Instance.Dispatch(this);
    }
}

public class FruitReceivedEventArgs : BaseEventArgs {
    public Fruit Fruit;
    public int Amount;
    public Vector3 Position;

    public FruitReceivedEventArgs() {
        ID = EventID.FRUIT_RECEIVED;
    }
}

public class FruitReceivingEffectFinishedEventArgs : BaseEventArgs {
    public FruitReceivingEffectFinishedEventArgs() {
        ID = EventID.FRUIT_RECEIVING_EFFECT_FINISHED;
    }
}
