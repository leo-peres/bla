using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class DropdownMenu : VisualElement {

    public string label {get; private set;}

    public List<Button> items;

    public DropdownMenu(string label, List<Button> items) {
        InitializeDropdown(label, items);
    }

    public DropdownMenu(List<Button> items) {
        InitializeDropdown("", items);
    }

    private void InitializeDropdown(string label, List<Button> items) {

        this.label = label;

        this.items = items;

        foreach(Button item in items) {
            Add(item);
        }

        ApplyStyleClasses();

        RegisterCallback<MouseDownEvent>(evt => evt.StopPropagation());      

    }

    private void ApplyStyleClasses() {
        AddToClassList("dropdown-main");
        //AddToClassList("dark-bg");
    }

    public void AddClass(string newClass) {
        AddToClassList(newClass);
    }

}

public class DropdownMenuWithOverlay : VisualElement {

    public DropdownMenu menu {get; private set;}
    public VisualElement overlay {get; private set;}

    private Clickable clickable;
    private RightClickable rightClickable;

    public DropdownMenuWithOverlay() {
        var items = new List<Button>();
        menu = new DropdownMenu("", items);
        InitializeDropdownWithOverlay(items);
    }

    public DropdownMenuWithOverlay(string label, List<Button> items) {
        menu = new DropdownMenu(label, items);
        InitializeDropdownWithOverlay(items);
    }

    public DropdownMenuWithOverlay(List<Button> items) {
        menu = new DropdownMenu(items);
        InitializeDropdownWithOverlay(items);
    }

    private void InitializeDropdownWithOverlay(List<Button> items) {

        overlay = CreateOverlay();

        clickable = new Clickable(() => {
            RemoveFromHierarchy();
        });

        rightClickable = new RightClickable(() => {
            RemoveFromHierarchy();
        });

        overlay.AddManipulator(clickable);
        overlay.AddManipulator(rightClickable);

        ApplyStyles();

        Add(overlay);
        Add(menu);
        
    }

    private void ApplyStyles() {

        style.position = Position.Absolute;
        style.left = 0;
        style.top = 0;
        style.width = Length.Percent(100);
        style.height = Length.Percent(100);

        overlay.style.position = Position.Absolute;
        overlay.style.left = 0;
        overlay.style.top = 0;
        overlay.style.width = Length.Percent(100);
        overlay.style.height = Length.Percent(100);
        overlay.style.backgroundColor = new Color(0, 0, 0, 0);
   
    }

    private VisualElement CreateOverlay() {
        return new VisualElement();
    }

    public void SetClickable(Clickable newClickable) {
        overlay.RemoveManipulator(clickable);
        clickable = newClickable;
        overlay.AddManipulator(clickable);
    }

}

public static class DropdownMenuUtils {

    public static void ShowDropdownMenuWithOverlay(VisualElement parent, string label, List<Button> items, Vector2 position, System.Action onClose = null) {

        DropdownMenuWithOverlay dropdownOverlay = new DropdownMenuWithOverlay(label, items);

        dropdownOverlay.menu.style.position = Position.Absolute;
        dropdownOverlay.menu.style.left = position.x;
        dropdownOverlay.menu.style.top = position.y;

        var clickable = new Clickable(() => {
            dropdownOverlay.RemoveFromHierarchy();
            onClose?.Invoke();
        });

        dropdownOverlay.overlay.AddManipulator(clickable);

        parent.Add(dropdownOverlay.overlay);
        parent.Add(dropdownOverlay.menu);

    }

}