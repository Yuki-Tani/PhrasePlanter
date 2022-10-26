using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using System.Diagnostics; // Debug

namespace PhrasePlanter.QuickRegistrar.Scecrets
{ 
    public interface IDataBaseConnectionSettings
    {
        public string ConnectionString { get; }
    }
}

namespace PhrasePlanter.QuickRegistrar
{
    public class PhrasePlanterDataBase
    {
        private readonly Scecrets.IDataBaseConnectionSettings connectionSettings;
        public PhrasePlanterDataBase()
        {
             connectionSettings = new Scecrets.PhrasePlanterDataBaseConnectionSettings();
        }

        public string TestAccess()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionSettings.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");
                    Console.WriteLine("=========================================\n");

                    connection.Open();

                    String sql = "SELECT Name FROM Users WHERE UserAccount = 'yutani'";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            Debug.WriteLine(reader.GetSqlString(0));
                            return reader.GetSqlString(0).ToString();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return e.Message;
            }
        }
    }
}
