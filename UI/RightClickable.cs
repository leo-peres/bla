using UnityEngine.UIElements;
using UnityEngine;

public class RightClickable : MouseManipulator {

    private System.Action onRightClick;

    public RightClickable(System.Action handler) {
        onRightClick = handler;
        activators.Add(new ManipulatorActivationFilter {button = MouseButton.RightMouse});
    }

    protected override void RegisterCallbacksOnTarget() {
        target.RegisterCallback<MouseDownEvent>(OnMouseDown);
    }

    protected override void UnregisterCallbacksFromTarget() {
        target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
    }

    private void OnMouseDown(MouseDownEvent evt) {
        if(evt.button == (int) MouseButton.RightMouse) {
            onRightClick?.Invoke();
            evt.StopPropagation();
        }
    }

}
