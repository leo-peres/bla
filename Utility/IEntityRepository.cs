using System;
using System.Collections.Generic;

public interface IEntityRepository {
    GameEntity FindById(string tableName, int id);
    IEnumerable<GameEntity> GetAll(string tableName, bool pagination, int page, int pageSize);
    IEnumerable<GameEntity> Search(string tableName, Func<GameEntity, bool> filter, bool pagination, int page, int pageSize);
    IEnumerable<GameEntity> Search(string tableName, string queryString, bool pagination, int page, int pageSize);
    void AddToDB(string tableName, GameEntity newObj);
    void WriteToDB(string tableName, GameEntity newObj);
    int GetCount(string tableName, string innerJoins, string whereClause);
}

