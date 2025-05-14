using UnityEngine;
using UnityEngine.UIElements;

public class DraggableManipulator : PointerManipulator {

    private Vector2 startMousePosition;
    private Vector2 startElementPosition;
    private bool dragging;

    public DraggableManipulator() {
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
    }

    protected override void RegisterCallbacksOnTarget() {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected override void UnregisterCallbacksFromTarget() {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    private void OnPointerDown(PointerDownEvent evt) {

        if(!CanStartManipulation(evt))
            return;

        dragging = true;
        startMousePosition = evt.position;
        startElementPosition = new Vector2(target.resolvedStyle.left, target.resolvedStyle.top);

        target.style.height = target.resolvedStyle.height;
        target.style.width = target.resolvedStyle.width;

        /*
        VisualElement root = target;
        while(!root.ClassListContains("unity-ui-document__root"))
            root = root.parent;

        root.Add(target);
        */

        target.CapturePointer(evt.pointerId);
        evt.StopPropagation();

    }

    private void OnPointerMove(PointerMoveEvent evt) {

        if(!dragging || !target.HasPointerCapture(evt.pointerId))
            return;

        Vector2 delta = new Vector2(evt.position.x, evt.position.y) - startMousePosition;

        target.style.position = Position.Absolute;
        target.style.left = startElementPosition.x + delta.x;
        target.style.top = startElementPosition.y + delta.y;

        target.BringToFront();

        evt.StopPropagation();

    }

    private void OnPointerUp(PointerUpEvent evt) {

        if(!CanStopManipulation(evt))
            return;

        dragging = false;
        target.ReleasePointer(evt.pointerId);
        evt.StopPropagation();
   
    }
}
