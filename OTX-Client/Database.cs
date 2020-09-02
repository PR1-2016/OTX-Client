using System;
using System.Data.SQLite;
using System.IO;

namespace OTX_Client
{
    public class Database
    {
        public SQLiteConnection connection;
        public Database()
        {
            connection = new SQLiteConnection("Data Source=database.sqlite3");

            if (!File.Exists("./database.sqlite3"))
            {
                SQLiteConnection.CreateFile("database.sqlite3");
            }
        }

        private void OpenConnection()
        {
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
            {
                connection.Close();
            }
        }
        public void AddIndicator(Indicator i)
        {
            string query = "INSERT INTO Indicators ('IndicatorId', 'PulseId', 'Indicator', 'Type', 'Description', 'Created')" +
                "VALUES (@indicatorId, @pulseId, @indicator, @type, @description, @created)";

            OpenConnection();
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@indicatorId", i.Id);
            command.Parameters.AddWithValue("@pulseId", i.PulseId);
            command.Parameters.AddWithValue("@indicator", i.IndicatorProperty);
            command.Parameters.AddWithValue("@type", i.Type);
            command.Parameters.AddWithValue("@description", i.Description);
            command.Parameters.AddWithValue("@created", i.Created.ToString());
            var result = command.ExecuteNonQuery();
            CloseConnection();
        }

        public void AddPulse(Pulse p)
        {
            string query = "INSERT INTO Pulses ('PulseId', 'Name', 'Description', 'TLP', 'Created', 'Modified')" +
                "VALUES (@pulseId, @name, @description, @tlp, @created, @modified)";

            OpenConnection();
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@pulseId", p.Id);
            command.Parameters.AddWithValue("@name", p.Name);
            command.Parameters.AddWithValue("@description", p.Description);
            command.Parameters.AddWithValue("@tlp", p.Tlp);
            command.Parameters.AddWithValue("@created", p.Created.ToString());
            command.Parameters.AddWithValue("@modified", p.Modified.ToString());
            var result = command.ExecuteNonQuery();

            if (p.Indicators != null)
            {
                foreach (var i in p.Indicators)
                {
                    AddIndicator(i);
                }
            }
            
            CloseConnection();
        }

        public void GetAllIndicators()
        {
            string query = "SELECT * FROM Indicators";
            OpenConnection();
            SQLiteCommand command = new SQLiteCommand(query, connection);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("Indicator id: {0}, indicator: {1}, type: {2}", 
                        reader["IndicatorId"], reader["Indicator"], reader["Type"]);
                }
            }
            CloseConnection();
        }

        public void GetAllPulses()
        {
            string query = "SELECT * FROM Pulses";
            OpenConnection();
            SQLiteCommand command = new SQLiteCommand(query, connection);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("Pulse id: {0}, name: {1}, description: {2}", 
                        reader["PulseId"], reader["Name"], reader["Description"]);
                }
            }
            CloseConnection();
        }

    }
}
