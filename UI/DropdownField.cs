using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevelPhysics;
using UnityEngine.UI;
using UnityEngine.UIElements;

using Button = UnityEngine.UIElements.Button;

namespace bla.UI {

    [UxmlElement]
    public partial class DropdownField : VisualElement, INotifyValueChanged<string> {

        [UxmlAttribute("choices")]
        public string ChoicesRaw { get; set; }

        [UxmlAttribute("default-choice")]
        public int DefaultChoice { get; set; } = 0;

        public Action<string> OnValueChanged;

        private string _value;
        public string value {
            get => _value;
            set {

                var previous = _value;
                _value = value;

                using(var evt = ChangeEvent<string>.GetPooled(previous, _value)) {
                    evt.target = this;
                    SendEvent(evt);
                }

            }
        }

        public Button dropdownBtn { get; private set; }
        private DropdownMenuWithOverlay dropdownOverlay;

        public VisualElement rootContainer { get; private set; }

        private List<string> choices;
        private int selected;
        private bool dropdownVisible = false;

        private List<string> dropdownMenuClasses;

        public DropdownField() {
            Initialize(false);
        }

        public DropdownField(List<string> choices, int defaultChoice = 0) {
            SetChoices(choices, defaultChoice);
            Initialize(true);
        }

        private void Initialize(bool choicesListSet) {

            if(!choicesListSet) {
                List<string> choicesList = !string.IsNullOrEmpty(ChoicesRaw)
                                           ? new List<string>(ChoicesRaw.Split(","))
                                           : new List<string>();

                SetChoices(choicesList, DefaultChoice);
            }

            dropdownBtn = new Button();
            if(selected >= 0)
                dropdownBtn.text = this.choices[selected];
            dropdownBtn.AddToClassList("dropdown-field-btn");

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

        public void SetChoices(List<string> newChoices, int defaultChoice = 0) {

            selected = -1;
            _value = "";
            choices = newChoices ?? new List<string>();

            if(choices.Count > 0 && defaultChoice < choices.Count && defaultChoice >= 0) {
                selected = defaultChoice;
                _value = choices[defaultChoice];
            }
            else if(choices.Count > 0) {
                selected = 0;
                _value = choices[0];
            }

            if(selected >= 0 && dropdownBtn != null)
                dropdownBtn.text = choices[selected];

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

                for(int i = 0; i < styleSheets.count; i++)
                    dropdownOverlay.styleSheets.Add(styleSheets[i]);

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

        public void ChangeSelected(string newSelected) {

            int index = choices.FindIndex(x => x == newSelected);

            if(index != selected) {
                selected = index;
                dropdownBtn.text = newSelected;
                value = newSelected;
                OnValueChanged?.Invoke(newSelected);
            }

            ToggleDropdown();

        }

        public void SetValueWithoutNotify(string newValue) {
            _value = newValue;
        }
    }

}