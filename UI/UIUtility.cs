using UnityEngine;
using UnityEngine.UIElements;

public static class UIUtility {

    public static void CenterElement(VisualElement element, Vector2 size) {
        //element.style.width = size.x;
        //element.style.height = size.y;

        element.style.position = Position.Absolute;
        element.style.left = Length.Percent(50);
        element.style.top = Length.Percent(50);
        element.style.translate = new StyleTranslate(new Translate(new Length(-50, LengthUnit.Percent), new Length(-50, LengthUnit.Percent)));
    }

    public static void ApplyFullStretchStyle(VisualElement element) {
        element.style.position = Position.Absolute;
        element.style.left = 0;
        element.style.top = 0;
        element.style.width = Length.Percent(100);
        element.style.height = Length.Percent(100);
    }

    public static void SetAsCloseableOverlay(VisualElement overlayElement) {
        ApplyFullStretchStyle(overlayElement);
        overlayElement.AddManipulator(new Clickable(() => overlayElement.parent.RemoveFromHierarchy()));
    }

    public static void ApplyEqualPadding(VisualElement element, float padding) {
        element.style.paddingBottom = padding;
        element.style.paddingLeft = padding;
        element.style.paddingRight = padding;
        element.style.paddingTop = padding;
    }

    public static void ApplyEqualBorderWidth(VisualElement element, float borderWidth) {
        element.style.borderBottomWidth = borderWidth;
        element.style.borderLeftWidth = borderWidth;
        element.style.borderRightWidth = borderWidth;
        element.style.borderTopWidth = borderWidth;
    }

    public static void ApplyEqualMargin(VisualElement element, float margin) {
        element.style.marginBottom = margin;
        element.style.marginLeft = margin;
        element.style.marginRight = margin;
        element.style.marginTop = margin;
    }

}
