using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SingleDoubleClickManipulator : PointerManipulator{
    
    private readonly Action<ClickEvent> onSingleClick;
    private readonly Action<ClickEvent> onDoubleClick;
    private readonly float doubleClickDelay;

    private bool singleClickPending;

    public SingleDoubleClickManipulator(Action<ClickEvent> onSingleClick, Action<ClickEvent> onDoubleClick, float doubleClickDelay = 0.25f) {
        this.onSingleClick = onSingleClick;
        this.onDoubleClick = onDoubleClick;
        this.doubleClickDelay = doubleClickDelay;
    }

    protected override void RegisterCallbacksOnTarget() {
        target.RegisterCallback<ClickEvent>(OnClick);
    }

    protected override void UnregisterCallbacksFromTarget() {
        target.UnregisterCallback<ClickEvent>(OnClick);
    }

    private void OnClick(ClickEvent evt) {

        if(evt.clickCount == 1) {

            singleClickPending = true;

            target.schedule.Execute(() => {
                if(singleClickPending)
                    onSingleClick?.Invoke(evt);
            }).StartingIn((long) (doubleClickDelay * 1000f));

        }
        else if(evt.clickCount == 2) {
            singleClickPending = false;
            onDoubleClick?.Invoke(evt);
        }
        
    }

}
