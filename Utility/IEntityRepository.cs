using System;
using System.Collections.Generic;

public interface IEntityRepository {
    GameEntity FindById(string tableName, int id);
    IEnumerable<GameEntity> GetAll(string tableName);
    IEnumerable<GameEntity> Search(string tableName, Func<GameEntity, bool> filter);
}

