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
        string connectionString = ((string)parameters["source"]);
        string command = ((string)parameters["query"]);
        
        if (command.StartsWith("select ", StringComparison.InvariantCultureIgnoreCase))
        {
            return await this.ExecuteQuery(connectionString, command);
        }
        else if (command.StartsWith("insert ", StringComparison.InvariantCultureIgnoreCase)
            || command.StartsWith("update ", StringComparison.InvariantCultureIgnoreCase)
            || command.StartsWith("delete ", StringComparison.InvariantCultureIgnoreCase))
        {
            return await this.ExecuteNonQuery(connectionString, command);
        }
        else
        {
            throw new InvalidOperationException("Unsupported type of SQL command. Only select, insert, update, delete are supported.");
        }   
    }

    async Task<object> ExecuteQuery(string strConn, string strSelect)
    {
        OleDbConnection myConn = null;
        try {
            object[] meta = new object[50];
            bool read;

            myConn = new OleDbConnection(strConn);
            
            OleDbCommand command = new OleDbCommand(strSelect,myConn);

            await myConn.OpenAsync();

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
        catch(Exception e)
        {
            throw new Exception("ExecuteQuery Error", e);
        }
        finally
        {
            myConn.Close();
        }
    }

    async Task<object> ExecuteNonQuery(string connectionString, string commandString)
    {
        using (var connection = new OleDbConnection(connectionString))
        {
            await connection.OpenAsync();
            
            using (var command = new OleDbCommand(commandString, connection))
            {
                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
