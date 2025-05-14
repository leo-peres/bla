using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuBar : VisualElement {

    private class MenuItem {

        public string label;
        public Button btn;
        public List<Button> dropdownContent;

        public MenuItem(string label) {
            this.label = label;
            btn = new Button() {text = label};
            btn.AddToClassList("top-bar-btn");
            dropdownContent = new List<Button>();
        }

        public void AddDropdownContent(Button newBtn) {
            dropdownContent.Add(newBtn);
        }

        public void AddDropdownContent(int position, Button newBtn) {
            dropdownContent.Insert(position, newBtn);
        }

    }

    private List<MenuItem> menuItems;
    private DropdownMenuWithOverlay activeDropdown;
    private VisualElement rootContainer;
    private bool active;

    public MenuBar(List<string> labels) {

        menuItems = new List<MenuItem>();
        active = false;

        foreach(string label in labels) {

            MenuItem item = new MenuItem(label);

            item.btn.clicked += () => OpenDropdown(item);

            item.btn.RegisterCallback<MouseOverEvent>(evt => {
                if(active && activeDropdown != null && activeDropdown.menu.label != item.label)
                    OpenDropdown(item);
            });

            menuItems.Add(item);
            Add(item.btn);

        }

        RegisterCallback<AttachToPanelEvent>(evt => {
            rootContainer = this;
            while(rootContainer.parent != null && rootContainer.parent.parent != null)
                rootContainer = rootContainer.parent;
        });

    }

    private void OpenDropdown(MenuItem item) {

        CloseDropdown();

        activeDropdown = new DropdownMenuWithOverlay(item.label, item.dropdownContent);

        activeDropdown.menu.style.position = Position.Absolute;
        activeDropdown.menu.style.left = item.btn.worldBound.xMin;
        activeDropdown.menu.style.top = item.btn.worldBound.yMax + 2;

        foreach(Button btn in activeDropdown.menu.items)
            btn.clicked += CloseDropdown;

        activeDropdown.menu.AddToClassList("dark-bg");

        Clickable clickable = new Clickable(CloseDropdown);
        activeDropdown.SetClickable(clickable);

        activeDropdown.overlay.RegisterCallback<MouseMoveEvent>(evt => {

            foreach(MenuItem mItem in menuItems) {
                if(mItem.btn.worldBound.Contains(evt.mousePosition) && mItem.label != item.label) {
                    using(MouseOverEvent newEvt = MouseOverEvent.GetPooled(evt)) {
                        newEvt.target = mItem.btn;
                        mItem.btn.SendEvent(newEvt);
                    }
                    break;
                }
            }

            evt.StopPropagation();

        });

        rootContainer.Add(activeDropdown.overlay);
        rootContainer.Add(activeDropdown.menu);

        active = true;

    }

    private void CloseDropdown() {

        
        if(activeDropdown != null) {
            activeDropdown.overlay.RemoveFromHierarchy();
            activeDropdown.menu.RemoveFromHierarchy();
            activeDropdown = null;
        }

        active = false;

    }

    public void AddItem(string field, Button newItem) {

        int index = menuItems.FindIndex(x => x.label == field);
        if(index >= 0)
            menuItems[index].AddDropdownContent(newItem);

    }

    public void AddItem(string field, int position, Button newItem) {

        int index = menuItems.FindIndex(x => x.label == field);
        if(index >= 0)
            menuItems[index].AddDropdownContent(position, newItem);

    }

    public void ClearMenuItem(string label) {
        
        int index = menuItems.FindIndex(x => x.label == label);
        if(index >= 0)
            menuItems[index].dropdownContent.Clear();

    }

}
