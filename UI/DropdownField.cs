using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DropdownField : VisualElement {
    
    public Action<string> OnValueChanged;
    
    public string value;
    
    public Button dropdownBtn {get; private set;}
    public DropdownMenuWithOverlay dropdownOverlay {get; private set;}
    
    public VisualElement rootContainer {get; private set;}
    
    private List<string> choices;
    private int selected;
    private bool dropdownVisible = false;

    private List<string> dropdownMenuClasses;

    public DropdownField(List<string> choices, int defaultChoice = 0) {
        
        selected = -1;
        value = "";
        if(choices.Count > 0 && defaultChoice < choices.Count && defaultChoice >= 0) {
            selected = defaultChoice;
            value = choices[defaultChoice];
        }
        else if(choices.Count > 0) {
            selected = 0;
            value = choices[0];
        }
        
        this.choices = choices ?? new List<string>();
        
        dropdownBtn = new Button();
        if(selected >= 0)
            dropdownBtn.text = this.choices[selected];
        dropdownBtn.AddToClassList("dropdown-btn");
        
        dropdownBtn.RegisterCallback<ClickEvent>(evt => ToggleDropdown());
        
        AddToClassList("custom-dropdown");
        
        Add(dropdownBtn);
        
        dropdownMenuClasses = new List<string>();

        RegisterCallback<AttachToPanelEvent>(evt => {
            rootContainer = this;
            while(rootContainer.parent != null && rootContainer.parent.parent != null)
                rootContainer = rootContainer.parent;
        });
        
    }
    
    public void AddClassToBtn(string className) {
        dropdownBtn.AddToClassList(className);
        if(dropdownOverlay != null)
            foreach(Button btn in dropdownOverlay.menu.Children())
                btn.AddToClassList(className);
    }

    public void AddClassToDropdownMenu(string className) {
        dropdownMenuClasses.Add(className);
    }
    
    void ToggleDropdown() {
        
        dropdownVisible = !dropdownVisible;
        
        if(dropdownVisible) {
            List<Button> choiceBtns = new List<Button>();
            foreach(string choiceText in choices) {
                Button btn = new Button();
                btn.text = choiceText;
                btn.clicked += () => ChangeSelected(choiceText);
                btn.AddToClassList("dropdown-btn");
                choiceBtns.Add(btn);
            }
            
            Vector2 dropdownBtnPosition = dropdownBtn.LocalToWorld(new Vector2(dropdownBtn.layout.xMin, dropdownBtn.layout.yMin));
            
            dropdownOverlay = new DropdownMenuWithOverlay(choiceBtns);
            dropdownOverlay.menu.style.position = Position.Absolute;
            //dropdownOverlay.menu.style.width = dropdownBtn.resolvedStyle.width;
            dropdownOverlay.menu.style.left = dropdownBtnPosition.x;
            dropdownOverlay.menu.style.top = dropdownBtnPosition.y + dropdownBtn.resolvedStyle.height;
            
            foreach(string className in dropdownMenuClasses)
                dropdownOverlay.AddToClassList(className);

            dropdownOverlay.SetClickable(new Clickable(() => {
                CloseDropdown();
            }));
            
            //rootContainer.Add(dropdownOverlay.overlay);
            //rootContainer.Add(dropdownOverlay.menu);

            rootContainer.Add(dropdownOverlay);

        }
        else
            CloseDropdown();
        
    }
    
    void CloseDropdown() {
        if(dropdownOverlay != null) {
            //dropdownOverlay.Disable();
            dropdownOverlay.RemoveFromHierarchy();
            dropdownOverlay = null;
        }
        dropdownVisible = false;
    }

    void ChangeSelected(string newSelected) {
        
        int index = choices.FindIndex(x => x == newSelected);
        
        if(index != selected) {
            selected = index;
            dropdownBtn.text = newSelected;
            value = newSelected;
            OnValueChanged?.Invoke(newSelected);
        }
        
        ToggleDropdown();
        
    }

}