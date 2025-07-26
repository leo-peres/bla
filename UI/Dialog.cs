using UnityEngine;
using UnityEngine.UIElements;

public class ModalDialog : VisualElement {

    public string title;

    public VisualElement parentElement;

    public VisualElement overlay;
    public VisualElement content;

    public ModalDialog(VisualElement parent) {
        ConstructorCommon(parent, "");
    }

    public ModalDialog(VisualElement parent, string title) {
        ConstructorCommon(parent, title);
    }

    private void ConstructorCommon(VisualElement parent, string title) {

        this.title = title;

        parentElement = parent;

        overlay = new VisualElement();
        content = new VisualElement();

        overlay.AddToClassList("dialog-overlay");
        content.AddToClassList("dialog-content");

        UIUtility.ApplyFullStretchStyle(this);
        UIUtility.SetAsCloseableOverlay(overlay);

        Add(overlay);
        Add(content);

    }

    public void Open() {
        UIUtility.CenterElement(content, new Vector2(content.resolvedStyle.width, content.resolvedStyle.height));
        parentElement.Add(this);
    }

    public void Close() {
        //parentElement.Remove(content);
        //parentElement.Remove(overlay);
        RemoveFromHierarchy();
    }


}
