using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Windows;
using System.Reflection;
using _7DaysToDieBackup.Models;

namespace _7DaysToDieBackup.DAL
{
    class Database
    {
        private Properties.Settings set = new Properties.Settings();
        public string ConString
        {
            get
            {
                return set.source + Path.Combine(set.location,set.name) + set.version;
            }
            set
            {

            }
        }

        public Database()
        {
            VerifyLocation();
            CreateDatabase();
        }
        public void AddLocation(string strGameLocation, string strSaveLocation)
        {
            string sql = "INSERT INTO InstallDirectory (GameLocation, BackupLocation) values('" + strGameLocation + "','" + strSaveLocation + "')";
            ExecuteNonQuery(sql);
        }

        public List<Games> GetAllLocations()
        {
            List<Games> Locations = new List<Games>();
            string sql = "SELECT * FROM InstallDirectory";
            SQLiteDataReader reader = ExecuteReader(sql);
            if(reader.FieldCount == 0)
            {

            }
            while (reader.Read())
            {
                Games game = new Games();
                game.strGameLocation = reader["GameLocation"].ToString();
                game.strBackupLocation = reader["BackupLocation"].ToString();
                Locations.Add(game);
            }

            return Locations;
        }

        public void Delete(Games game)
        {
            string sql = "DELETE FROM InstallDirectory where GameLocation ='" + game.strGameLocation + "' and BackupLocation ='" + game.strBackupLocation +"'";
            ExecuteNonQuery(sql);
        }
        private void CreateDatabase()
        {
            if(!Directory.Exists(set.location))
            {
                Directory.CreateDirectory(set.location);
            }

            if(!File.Exists(Path.Combine(set.location,set.name)))
            {
                SQLiteConnection.CreateFile(Path.Combine(set.location, set.name));
                CreateTables();
            }
        }

        private void CreateTables()
        {
            string sql = "CREATE TABLE InstallDirectory (GameLocation varchar(1000), BackupLocation varchar(1000))";
            ExecuteNonQuery(sql);
        }

        private void ExecuteNonQuery(string sql)
        {
            SQLiteConnection con = new SQLiteConnection(ConString);
            con.Open();
            SQLiteCommand com = new SQLiteCommand(sql, con);
            com.ExecuteNonQuery();
            con.Close();
        }

        private SQLiteDataReader ExecuteReader(string sql)
        {
            SQLiteConnection con = new SQLiteConnection(ConString);
            con.Open();
            SQLiteCommand com = new SQLiteCommand(sql, con);
            SQLiteDataReader read = com.ExecuteReader();
            return read;
        }
        private void VerifyLocation()
        {
            if(String.IsNullOrEmpty(set.location))
            {
                set.location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetEntryAssembly().GetName().Name);
                set.Save();
            }
        }
    }
}
