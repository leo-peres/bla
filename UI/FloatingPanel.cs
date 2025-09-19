using UnityEngine.UIElements;

public class FloatingPanel : VisualElement {

    public VisualElement topBar;
    public VisualElement content;

    public FloatingPanel() {

        topBar = new VisualElement();
        topBar.AddToClassList("floating-panel-top-bar");

        content = new VisualElement();
        content.AddToClassList("floating-panel-content");

        style.position = Position.Absolute;

        this.AddManipulator(new DraggableManipulator(topBar));

        Add(topBar);
        Add(content);

    }

}
