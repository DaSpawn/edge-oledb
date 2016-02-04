/*!
 * edge-oledb
 * Copyright(c) 2015 Brian Taber
 * MIT Licensed
 */

//#r "System.dll"
//#r "System.Data.dll"
//#r "System.Web.Extensions.dll"

using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;


public class Startup
{
    public async Task<object> Invoke(IDictionary<string, object> parameters)
    {
        string connectionString = ((string)parameters["dsn"]);
        string commandString = ((string)parameters["query"]);
        string command = commandString.Substring(0, 6).Trim().ToLower();
        switch (command)
        {
            case "select":
                return await this.ExecuteQuery(connectionString, commandString);
                break;
            case "insert":
            case "update":
            case "delete":
                return await this.ExecuteNonQuery(connectionString, commandString);
                break;
            default:
                throw new InvalidOperationException("Unsupported type of SQL command. Only select, insert, update, delete are supported.");
        }
    }

    async Task<object> ExecuteQuery(string connectionString, string commandString)
    {
        OleDbConnection connection = null;
        try {
            using (connection = new OleDbConnection(connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new OleDbCommand(commandString, connection))
                {

                    List<object> rows = new List<object>();

                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        IDataRecord record = (IDataRecord)reader;
                        while (await reader.ReadAsync())
                        {
                            var dataObject = new ExpandoObject() as IDictionary<string, Object>;
                            var resultRecord = new object[record.FieldCount];
                            record.GetValues(resultRecord);

                            for (int i = 0; i < record.FieldCount; i++)
                            {      
                                Type type = record.GetFieldType(i);
                                if (resultRecord[i] is System.DBNull)
                                {
                                    resultRecord[i] = null;
                                }
                                else if (type == typeof(byte[]) || type == typeof(char[]))
                                {
                                    resultRecord[i] = Convert.ToBase64String((byte[])resultRecord[i]);
                                }
                                else if (type == typeof(Guid) || type == typeof(DateTime))
                                {
                                    resultRecord[i] = resultRecord[i].ToString();
                                }
                                else if (type == typeof(IDataReader))
                                {
                                    resultRecord[i] = "<IDataReader>";
                                }

                                dataObject.Add(record.GetName(i), resultRecord[i]);
                            }

                            rows.Add(dataObject);
                        }

                        return rows;
                    } 
                }
            }
        }
        catch(Exception e)
        {
            throw new Exception("ExecuteQuery Error", e);
        }
        finally
        {
            connection.Close();
        }
    }

    async Task<object> ExecuteNonQuery(string connectionString, string commandString)
    {
        OleDbConnection connection = null;
        try
        {
            using (connection = new OleDbConnection(connectionString))
            {
                await connection.OpenAsync();
                
                using (var command = new OleDbCommand(commandString, connection))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
        catch(Exception e)
        {
            throw new Exception("ExecuteNonQuery Error", e);
        }
        finally
        {
            connection.Close();
        }

    }
}
