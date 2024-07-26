using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using Dapper;
using Device.ControlUint.Data.Entitys;


namespace Device.ControlUnit.Data
{
    public class DeviceContextDb
    {
        private readonly string connectionString;
        public DeviceContextDb()
        {
            string path = ConfigurationManager.AppSettings["DBPath"].ToString();
            if (Environment.Is64BitProcess)
            {
                connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};";
            }
            else
            {
                connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={path};";
            }
        }

        // 查询
        public IEnumerable<T> Query<T>(string sql, object parameters = null)
        {
            using (IDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                return connection.Query<T>(sql, parameters);
            }
        }

        // 执行sql语句
        public int Execute(string sql, object parameters = null)
        {
            using (IDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                return connection.Execute(sql, parameters);
            }
        }

        public long InsertNamePlate(NameplateParameterTableEntity entity)
        {
            using (IDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                return connection.Execute($"INSERT INTO NameplateParameterTable (Content,ProjectNo,ProductName,Standard,Model,TestName,TestScale) VALUES ('{entity.Content}','{entity.ProjectNo}','{entity.ProductName}','{entity.Standard}','{entity.Model}','{entity.TestName}','{entity.TestScale}');");
            }
        }

        public long DeleteNamePlate()
        {
            using (IDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                return connection.Execute($"Delete FROM NameplateParameterTable;");
            }
        }

        public long InsertReport(ReportTableEntity entity)
        {
            using (IDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                return connection.Execute($"INSERT INTO ReportTable ([ItemName],[Value],TypeName) VALUES ('{entity.ItemName}','{entity.Value}','{entity.TypeName}');");
            }
        }




    }
}
