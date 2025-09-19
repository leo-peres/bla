using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class EntityCache {

    private readonly int capacity;
    private readonly Dictionary<int, GameEntity> dict = new();
    private readonly Queue<int> order = new();

    public EntityCache(int capacity) {
        this.capacity = capacity;
    }

    public GameEntity Get(int id) {
        dict.TryGetValue(id, out var entity);
        return entity;
    }

    public void Add(GameEntity entity) {

        int id = entity.Id;

        if(dict.ContainsKey(id))
            return;

        if(dict.Count >= capacity) {
            int oldest = order.Dequeue();
            dict.Remove(oldest);
        }

        dict[id] = entity;
        order.Enqueue(id);

    }

}
