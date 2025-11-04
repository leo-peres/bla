using UnityEngine;
using System;
using System.Collections.Generic;

using Random = UnityEngine.Random;
using System.Linq;

public class IDAssigner : IIDAssigner {

    public SortedSet<int> takenIds { get; set; }

    private readonly SortedSet<int> freeIds = new SortedSet<int>();
    private int minId;
    private int maxId;

    public IDAssigner(int minId = 1) {
        this.minId = minId;
        maxId = this.minId - 1;
        takenIds = new();
    }

    public IDAssigner(List<int> ids, int minId = 1) {
        this.minId = 1;
        maxId = this.minId - 1;
        takenIds = new();
        SetIDs(ids);
    }

    public int GetID() {
        if(freeIds.Count > 0) {
            int id = Min(freeIds);
            freeIds.Remove(id);
            takenIds.Add(id);
            return id;
        }
        takenIds.Add(++maxId);
        return maxId;
    }

    public void ReleaseID(int id) {

        if(!takenIds.Contains(id))
            return;

        if(id < maxId)
            freeIds.Add(id);
        else
            maxId--;

        takenIds.Remove(id);

    }

    public void SetIDs(List<int> ids) {

        Clear();

        if(ids.Count == 0)
            return;

        ids.Sort();

        maxId = ids[^1];

        int current = 1;
        foreach(int id in ids) {

            while(current < id) {
                freeIds.Add(current);
                current++;
            }

            takenIds.Add(id);
            current = id + 1;

        }

    }

    public void Clear() {
        takenIds.Clear();
        freeIds.Clear();
        maxId = 0;
    }

    private int Min(SortedSet<int> set) {
        foreach(int val in set)
            return val;
        return -1;
    }

}

public class IDAssignerInc : IIDAssigner {

    public SortedSet<int> takenIds { get; set; }

    private int maxId;

    public IDAssignerInc() {
        takenIds = new();
    }

    public IDAssignerInc(List<int> ids) {
        takenIds = new();
        SetIDs(ids);
    }

    public int GetID() {
        return ++maxId;
    }

    public int GetRandomID() {
        return Random.Range(1, maxId);
    }

    public void ReleaseID(int id) {

        if(!takenIds.Contains(id))
            return;

        takenIds.Remove(id);

        if(id == maxId)
            maxId = takenIds.Max();

    }

    public void SetIDs(List<int> ids) {

        Clear();

        if(ids.Count == 0)
            return;

        ids.Sort();

        maxId = ids[^1];

        foreach(int id in ids)
            takenIds.Add(id);

    }

    public void Clear() {
        maxId = 0;
    }

}

public interface IIDAssigner {

    public SortedSet<int> takenIds { get; set; }

    int GetID();
    void ReleaseID(int id);
    void SetIDs(List<int> ids);
    void Clear();

}