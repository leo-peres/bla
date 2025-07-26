using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class SortableList<T> : VisualElement {

    private List<T> items;
    private Func<T, IComparable>[] columnSelectors;
    private string[] columnNames;

    public VisualElement header { get; private set; }
    public VisualElement itemsContainer { get; private set; }

    private int sortedColumn = -1;
    private bool ascending = true;

    public SortableList(Func<T, IComparable>[] columnSelectors, string[] columnNames) {

        this.columnSelectors = columnSelectors;
        this.columnNames = columnNames;

        header = new VisualElement();
        header.style.flexDirection = FlexDirection.Row;

        itemsContainer = new VisualElement();

        for(int i = 0; i < columnNames.Length; i++) {
            Debug.Log(columnNames[i]);
            Label headerCol = new Label(columnNames[i]);
            header.Add(headerCol);
        }

        Add(header);
        Add(itemsContainer);

    }

    public void SetItems(List<T> items) {
        this.items = items;
        Refresh();
    }

    private void Refresh() {

        itemsContainer.Clear();

        foreach(var item in items) {

            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;

            for(int i = 0; i < columnSelectors.Length; i++) {
                var value = columnSelectors[i](item)?.ToString() ?? "";
                var label = new Label(value);
                row.Add(label);
            }

            itemsContainer.Add(row);

        }

    }

    private void SortByColumn(int columnIndex) {

    }

}