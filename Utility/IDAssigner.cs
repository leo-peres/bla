using System.Collections.Generic;

public class IDAssigner {

    private readonly SortedSet<int> freeIds = new SortedSet<int>();
    private int maxId = -1;

    public int GetID() {
        if(freeIds.Count > 0) {
            int id = Min(freeIds);
            freeIds.Remove(id);
            return id;
        }
        return ++maxId;
    }

    public void ReleaseID(int id) {
        if(id < maxId)
            freeIds.Add(id);
        else
            maxId--;
    }

    private int Min(SortedSet<int> set) {
        foreach(int val in set)
            return val;
        return -1;
    }

}
