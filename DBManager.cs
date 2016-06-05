using System;
using System.Data;
using System.Data.Common;

namespace GenericDA
{
    public class DBManager
    {        
        private string providerName = string.Empty;
        private string connectionString = string.Empty;
        public DBManager(string ProviderName,string ConnectionString)
        {            
            providerName = ProviderName;
            connectionString = ConnectionString;
        }

        private DbConnection GetConnection()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            return connection;
        }

        private IDbDataAdapter GetAdapter()
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            var adapter = factory.CreateDataAdapter();
            return adapter;
        }

        public DataTable GetTableSchema(string tableName)
        {
            DataTable table = new DataTable();
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.ConnectionString = connectionString;
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = string.Format("select * from {0} limit 1", tableName);
                        Logger.Debug("Query: " + command.CommandText);
                        DataSet dataSet = new DataSet();
                        IDbDataAdapter adapter = GetAdapter();
                        {
                            adapter.SelectCommand = command;
                            adapter.Fill(dataSet);
                            if (dataSet.Tables.Count > 0)
                            {
                                table = dataSet.Tables[0];
                            }
                            else
                            {
                                Logger.Info("Empty table.");
                                return null;
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                Logger.Debug(e.StackTrace);
                return null;
            }

            return table;
        }
        public DataTable ExecuteQuery(string query)
        {
            DataTable table = new DataTable();
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.ConnectionString = connectionString;
                    if(connection.State!= ConnectionState.Open)
                        connection.Open();
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        Logger.Debug("Query: "+query);
                        DataSet dataSet = new DataSet();
                        IDbDataAdapter adapter = GetAdapter();
                        {
                            adapter.SelectCommand = command;
                            adapter.Fill(dataSet);
                            if (dataSet.Tables.Count > 0)
                            {
                                table= dataSet.Tables[0];
                            }
                            else
                            {
                                Logger.Info("Empty table.");
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                Logger.Debug(e.StackTrace);
                return null;
            }

            return table;

        }

        public int ExecuteNonQuery(string query)
        {
            int count = 0;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.ConnectionString = connectionString;
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        Logger.Debug("Query: " + query);
                        count = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                Logger.Debug(e.StackTrace);
                return 0;
            }

            return count;

        }

        public string BuildQuery(string baseQuery, string WhereClause = "", string OrderByColumnName = "", string OrderByDirection = "asc")
        {
            string query = string.Empty;

            try
            {
                query = baseQuery;

                query = !string.IsNullOrEmpty(WhereClause) ? query + " " +
                    string.Format(" Where {0} ", WhereClause) : query;

                query = !string.IsNullOrEmpty(OrderByColumnName) ? (query + " " +
                    string.Format(" ORDER BY {0} {1} ", string.IsNullOrEmpty(OrderByColumnName) ? "id" : OrderByColumnName,
                    string.IsNullOrEmpty(OrderByDirection) ? "asc" : OrderByDirection)) : query;

            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                Logger.Debug(e.StackTrace);
            }
            return query;
        }

        public long GetCount(string query)
        {
            long count = 0;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.ConnectionString = connectionString;
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = string.Format("select count(*) from ({0}) as T", query);
                        Logger.Debug("Query: " + command.CommandText);
                        count = (long)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                Logger.Debug(e.StackTrace);
                return 0;
            }
            return count;
        }
       
    }
}
