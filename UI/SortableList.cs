using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.UIElements;

using MouseButton = UnityEngine.UIElements.MouseButton;

public class SortableList : VisualElement {

    private class SortableListRow : VisualElement {

        public int index;

        public Dictionary<string, string> rowItemsMap = new();

        public SortableListRow(int index, List<(string, string)> columnNamesValues) {

            this.index = index;

            AddToClassList("sortable-list-row");
            style.flexDirection = FlexDirection.Row;

            for(int i = 0; i < columnNamesValues.Count; i++) {

                string columnName = columnNamesValues[i].Item1;
                string value = columnNamesValues[i].Item2;

                rowItemsMap[columnName] = value;

                var label = new Label(value);
                label.AddToClassList("sortable-list-row-entry");
                Add(label);

            }

        }

        public string GetItem(string columnName) {
            if(rowItemsMap.TryGetValue(columnName, out string value))
                return value;
            return "";
        }

        public void SetItem(string columnName, string value) {
            if(rowItemsMap.ContainsKey(columnName))
                rowItemsMap[columnName] = value;
        }

    }

    private List<SortableListRow> rows = new();

    private List<string> columnNames;

    public VisualElement header { get; private set; }
    public VisualElement itemsContainer { get; private set; }

    public event Action<int> OnRowSingleClick;
    public event Action<int> OnRowDoubleClick;

    public SortableList() {
        this.columnNames = new List<string>();
        ConstructorCommon();
    }

    public SortableList(List<string> columnNames) {
        this.columnNames = columnNames;
        ConstructorCommon();
    }

    private void ConstructorCommon() {

        header = new VisualElement();
        header.AddToClassList("sortable-list-header");
        header.style.flexDirection = FlexDirection.Row;

        itemsContainer = new ScrollView();
        itemsContainer.AddToClassList("sortable-list-content");

        for(int i = 0; i < columnNames.Count; i++) {
            Label headerCol = new Label(columnNames[i]);
            headerCol.AddToClassList("sortable-list-header-col");
            header.Add(headerCol);
        }

        Add(header);
        Add(itemsContainer);

    }

    public void InsertRow(int index, List<string> values) {

        var colNamesValues = new List<(string, string)>();

        for(int i = 0; i < columnNames.Count; i++) {
            string value = i < values.Count ? values[i] : "";
            colNamesValues.Add((columnNames[i], value));
        }

        var row = new SortableListRow(rows.Count, colNamesValues);
        rows.Insert(index, row);

        var singleDoubleManipulator = new SingleDoubleClickManipulator(
            evt => OnRowSingleClick?.Invoke(index),
            evt => OnRowDoubleClick?.Invoke(index)
        );

        row.AddManipulator(singleDoubleManipulator);

        Refresh();

    }

    public void AddRow(List<string> values) {
        InsertRow(rows.Count, values);
    }

    public void DeleteRow(int index) {
        rows.RemoveAt(index);
        Refresh();
    }

    public void ClearRows() {
        rows.Clear();
        Refresh();
    }

    private void Refresh() {
        itemsContainer.Clear();
        foreach(var row in rows)
            itemsContainer.Add(row);
    }

    private void SortByColumn(int columnIndex) {

    }

    public void SetColumns(List<string> colNames) {
        columnNames = colNames;
    }

    public void SetColumn(int index, string colName) {
        columnNames[index] = colName;
        Refresh();
    }

    public void AddColumn(string colName) {
        columnNames.Add(colName);
        Refresh();
    }

    public void InsertColumn(int index, string colName) {
        columnNames.Insert(index, colName);
        Refresh();
    }

    public void SwapColumns(int i1, int i2) {
        (columnNames[i1], columnNames[i2]) = (columnNames[i2], columnNames[i1]);
        Refresh();
    }

    public void RemoveColumn(string colName) {
        int index = columnNames.IndexOf(colName);
        columnNames.RemoveAt(index);
        Refresh();
    }

    public void ClearColumns() {
        columnNames.Clear();
        Refresh();
    }

    public void Reset() {
        rows.Clear();
        Refresh();
    }

}

public class SortableListControl<T> {

    private List<T> items = new();

    private List<Func<T, IComparable>> columnSelectors;
    private List<string> columnNames;

    private int sortedColumn = -1;
    private bool ascending = true;

    public event Action<T> OnSingleClick;
    public event Action<T> OnDoubleClick;

    public SortableList view { get; private set; }

    public SortableListControl(List<string> columnNames, List<Func<T, IComparable>> columnSelectors) {
        this.columnSelectors = columnSelectors;
        this.columnNames = columnNames;
    }

    public void BindView(SortableList view) {

        this.view = view;
        view.Reset();
        view.SetColumns(columnNames);
        AddRowsToView();

        view.OnRowSingleClick += (i) => OnSingleClick?.Invoke(items[i]);
        view.OnRowDoubleClick += (i) => OnDoubleClick?.Invoke(items[i]);

    }

    public void BindView() {
        BindView(new SortableList(columnNames));
    }

    public void SetItems(List<T> items) {
        this.items = items;
        AddRowsToView();
    }

    public void ClearRows() {
        items.Clear();
        AddRowsToView();
    }

    private void AddRowsToView() {

        view?.ClearRows();

        foreach(var item in items) {
            var values = new List<string>();
            foreach(var selector in columnSelectors)
                values.Add(selector != null ? selector(item)?.ToString() : "");
            view?.AddRow(values);
        }

    }

}