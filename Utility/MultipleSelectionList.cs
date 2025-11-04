using System;
using System.Collections.Generic;
using UnityEngine;

public class MultipleSelectionList {

    private HashSet<int> selectedSet = new HashSet<int>();
    private int? lastTouchedIndex = null;

    public Func<int, int, List<int>> FindChain = DefaultFindChain;

    public MultipleSelectionList() {}

    public MultipleSelectionList(Func<int, int, List<int>> FindChain) {
        this.FindChain = FindChain;
    }

    public (List<int>, List<int>) AddUnique(int index) {

        List<int> selectedItems;
        List<int> deselectedItems;

        if(selectedSet.Contains(index)) {
            selectedItems = new List<int>();
            deselectedItems = new List<int>();
            foreach(int i in selectedSet) {
                if(i != index)
                    deselectedItems.Add(i);
            }
        }
        else {
            selectedItems = new List<int> { index };
            deselectedItems = new List<int>(selectedSet);
        }

        selectedSet.Clear();
        selectedSet.Add(index);
        lastTouchedIndex = index;

        return (selectedItems, deselectedItems);

    }

    public (List<int>, List<int>) AddSingle(int index) {

        List<int> selectedItems;
        List<int> deselectedItems;

        if(selectedSet.Contains(index)) {
            selectedSet.Remove(index);
            selectedItems = new List<int>();
            deselectedItems = new List<int> { index };
        }
        else {
            selectedSet.Add(index);
            selectedItems = new List<int> { index };
            deselectedItems = new List<int>();
        }

        lastTouchedIndex = index;

        return (selectedItems, deselectedItems);

    }

    public (List<int>, List<int>) AddChain(int index) {

        if(lastTouchedIndex == null)
            return AddSingle(index);

        List<int> chain = FindChain(index, lastTouchedIndex.Value);
        List<int> selectedItems = new List<int>();
        List<int> deselectedItems = new List<int>();

        if(selectedSet.Contains(index)) {
            foreach(int i in chain) {
                if(selectedSet.Contains(i)) {
                    selectedSet.Remove(i);
                    deselectedItems.Add(i);
                }
            }
        } 
        else {
            foreach(int i in chain) {
                if(selectedSet.Add(i)) {
                    selectedItems.Add(i);
                }
            }
        }

        lastTouchedIndex = index;

        return (selectedItems, deselectedItems);
    }

    public void Remove(int index) {
        if(selectedSet.Contains(index)) {
            selectedSet.Remove(index);
            lastTouchedIndex = index;
        }
    }

    public List<int> Clear() {
        List<int> deselectedItems = new List<int>(selectedSet);
        selectedSet.Clear();
        lastTouchedIndex = null;
        return deselectedItems;
    }

    public bool Contains(int index) {
        return selectedSet.Contains(index);
    }

    public List<int> GetSelectedList() {
        return new List<int>(selectedSet);
    }

    public static List<int> DefaultFindChain(int a, int b) {
        List<int> chain = new List<int>();
        int start = Mathf.Min(a, b);
        int end = Mathf.Max(a, b);
        for (int i = start; i <= end; i++) {
            chain.Add(i);
        }
        return chain;
    }

}
