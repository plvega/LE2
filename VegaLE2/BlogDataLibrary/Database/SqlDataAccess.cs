﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Dapper;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public List<T> LoadData<T, U>(string sqlStatement,
                                  U parameters,
                                  string connectionStringName,
                                  bool isStoredProcedure)
    {
        CommandType commandType = CommandType.Text;
        string connectionString = _config.GetConnectionString(connectionStringName);

        if (isStoredProcedure)
        {
            commandType = CommandType.StoredProcedure;
        }

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            List<T> rows = connection.Query<T>(sqlStatement, parameters,
                                               commandType: commandType).ToList();
            return rows;
        }
    }
    public void SaveData<T>(string sqlStatement,
                        T parameters,
                        string connectionStringName,
                        bool isStoredProcedure)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);
        CommandType commandType = CommandType.Text;

        if (isStoredProcedure)
        {
            commandType = CommandType.StoredProcedure;
        }

        using (IDbConnection connection = new SqlConnection(connectionString))
        {
            connection.Execute(sqlStatement, parameters, commandType: commandType);
        }
    }

}
