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
        public string UserId { get; }
    }
}

// TODO: Create Cloud Functions instead of accessing DB directly

namespace PhrasePlanter.QuickRegistrar
{
    public class PhrasePlanterDataBase
    {
        private readonly Scecrets.IDataBaseConnectionSettings settings;
        public PhrasePlanterDataBase(string userId, string password)
        {
             settings = new Scecrets.PhrasePlanterDataBaseConnectionSettings(userId, password);
        }

        public bool InsertPhrase(string phrase, string phraseMeaning)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(settings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(
                        "INSERT INTO pp.Phrases (UserId, Phrase, PhraseMeaning) VALUES (" +
                        "  (SELECT UserId FROM dbo.Users Where UserAccount = @UserAccount)," +
                        "  @Phrase," +
                        "  @PhraseMeaning" +
                        ");",
                        connection
                    ))
                    {
                        command.Parameters.AddWithValue("@UserAccount", settings.UserId);
                        command.Parameters.AddWithValue("@Phrase", phrase);
                        command.Parameters.AddWithValue("PhraseMeaning", phraseMeaning); // automatically use N'xxx'

                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
        }

        public string[] WritePhrases()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(settings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(
                        "SELECT Phrase, PhraseMeaning FROM pp.Phrases",
                        connection
                    ))
                    {
                        string phrase = "";
                        string phraseMeaning = "";
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                phrase = $"{reader.GetSqlString(0)}";
                                phraseMeaning = $"{reader.GetSqlString(1)}";
                                Debug.WriteLine($"{phrase}: {phraseMeaning}");
                            }
                        }

                        return new string[] { phrase, phraseMeaning };
                    }
                }
            }
            catch (SqlException e)
            {
                Debug.WriteLine(e.ToString());
                return new string[] { e.ToString() };
            }
        }
    }
}
