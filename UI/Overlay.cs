using UnityEngine;
using UnityEngine.UIElements;

public class Overlay : VisualElement {

    private Clickable clickable;

    public Overlay(bool closeable) {

        if (closeable)
            clickable = new Clickable(() => RemoveFromHierarchy());
        else
            clickable = new Clickable(() => {});

        this.AddManipulator(clickable);

        ApplyStyles();
    }

    public Overlay(Clickable clickable) {
        this.clickable = clickable;
        this.AddManipulator(clickable);

        ApplyStyles();
    }

    private void ApplyStyles() {
        style.position = Position.Absolute;
        style.left = 0;
        style.top = 0;
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);
        style.backgroundColor = new Color(0, 0, 0, 0.2f);
    }

    public void SetClickable(Clickable newClickable) {
        this.RemoveManipulator(clickable);
        clickable = newClickable;
        this.AddManipulator(clickable);
    }
    
}
