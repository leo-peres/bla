using UnityEngine;
using UnityEngine.UIElements;

public class DraggableManipulator : Manipulator {

    private VisualElement _handler;
    private VisualElement _target;

    private Vector2 _startMousePosition;
    private Vector2 _startElementPosition;

    private bool _isDragging;

    public DraggableManipulator(VisualElement handler = null) {
        _handler = handler;
    }

    protected override void RegisterCallbacksOnTarget() {

        _target = target;

        var handler = _handler ?? _target;

        handler.RegisterCallback<MouseDownEvent>(OnMouseDown);
        handler.RegisterCallback<MouseMoveEvent>(OnMouseMove);
        handler.RegisterCallback<MouseUpEvent>(OnMouseOut);

    }

    protected override void UnregisterCallbacksFromTarget() {
        var handler = _handler ?? _target;
        handler.UnregisterCallback<MouseDownEvent>(OnMouseDown);
        handler.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
        handler.UnregisterCallback<MouseUpEvent>(OnMouseOut);
    }

    private void OnMouseDown(MouseDownEvent evt) {

        if(evt.button == 0) {

            _startMousePosition = evt.mousePosition;
            _startElementPosition = new Vector2(_target.resolvedStyle.left, _target.resolvedStyle.top);
            _isDragging = true;

            var handler = _handler ?? _target;
            handler.CaptureMouse();
            evt.StopPropagation();

        }

    }

    private void OnMouseMove(MouseMoveEvent evt) {

        if(_isDragging) {

            Vector2 delta = evt.mousePosition - _startMousePosition;

            _target.style.left = _startElementPosition.x + delta.x;
            _target.style.top = _startElementPosition.y + delta.y;

            evt.StopPropagation();
        }

    }

    private void OnMouseOut(MouseUpEvent evt) {

        if(_isDragging && evt.button == 0) {

            _isDragging = false;

            var handler = _handler ?? _target;
            handler.ReleaseMouse();
            evt.StopPropagation();

        }

    }
    
}