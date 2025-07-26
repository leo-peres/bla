using UnityEngine;
using UnityEngine.UIElements;

public class Draggable : MonoBehaviour {

    bool beingDragged = false;

    Vector2 initialPosition, initialMousePosition;

    VisualElement targetElement, handle;

    void Update() {

        if(beingDragged) {

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            Vector2 mousePosition = root.WorldToLocal(Input.mousePosition);
            Vector2 targetPosition = new Vector2(targetElement.resolvedStyle.left, targetElement.resolvedStyle.top);

            float height = targetElement.resolvedStyle.height;

            targetElement.style.left = mousePosition.x - initialMousePosition.x;
            targetElement.style.bottom = mousePosition.y - (height - initialMousePosition.y);

        }

    }

    public void SetUpDraggable(VisualElement target) {

        SetUpDraggable(target, target);

        targetElement = target;
        handle = target;

        targetElement.RegisterCallback<PointerDownEvent>(OnPointerDown);
        targetElement.RegisterCallback<PointerUpEvent>(OnPointerUp);

    }

    public void SetUpDraggable(VisualElement target, VisualElement handle) {

        targetElement = target;
        this.handle = handle;

        handle.RegisterCallback<PointerDownEvent>(OnPointerDown);
        handle.RegisterCallback<PointerUpEvent>(OnPointerUp);

    }

    void OnPointerDown(PointerDownEvent evt) {
        beingDragged = true;
        initialPosition = new Vector2(targetElement.resolvedStyle.left, targetElement.resolvedStyle.top);
        initialMousePosition = new Vector2(evt.position.x, evt.position.y);
        initialMousePosition = targetElement.WorldToLocal(initialMousePosition);
    }

    void OnPointerUp(PointerUpEvent evt) {
        beingDragged = false;
    }

}
