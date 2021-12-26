using System.Collections.Generic;

public class EventManager : ISingleton<EventManager> {
    #region Delegate Handlings
    public delegate void EventCallback(BaseEventArgs args);
    #endregion

    #region Internal Fields
    private Dictionary<EventID, List<EventCallback>> _eventCallbackMap = new Dictionary<EventID, List<EventCallback>>();
    #endregion

    #region APIs
    public void Register(EventID eID, EventCallback eCB) {
        if (!_eventCallbackMap.ContainsKey(eID)) {
            _eventCallbackMap.Add(eID, new List<EventCallback>());
        }

        _eventCallbackMap[eID].Add(eCB);
    }

    public void Unregister(EventID eID, EventCallback eCB) {
        if (!_eventCallbackMap.ContainsKey(eID)) {
            return;
        }

        _eventCallbackMap[eID].Remove(eCB);
    }

    public void Dispatch(BaseEventArgs eArgs) {
        EventID eID = eArgs.ID;
        if (eArgs.ID == EventID.NONE) {
            return;
        }

        if (!_eventCallbackMap.ContainsKey(eID)) {
            return;
        }

        List<EventCallback> eCBList = _eventCallbackMap[eID];
        for (int i = 0; i < eCBList.Count; i++) {
            eCBList[i](eArgs);
        }
    }
    #endregion
}
