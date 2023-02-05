using Microsoft.Data.Sqlite;
using System.Data;

namespace BlazorListBack
{
    public class Obsługa
    {
        public Projekt projekt { get; set; }
        public Zadanie zadanie { get; set; }
        public List<Projekt> projektlist { get; set; }
        public List<Zadanie> zadanielist { get; set; }

        public List<Projekt> listaprojektow()
        {
            using (var connection = new SqliteConnection("Data Source=projekty.db"))
            {
                var i = 0.0;
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"SELECT count(1) FROM PROJEKTY";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var j = reader.GetInt64(0);
                        i = j;
                    }
                }
                i = i / 1;
                for (var j = 0; j < i + 1; j++)
                {
                    command = connection.CreateCommand();
                    command.CommandText = @"SELECT * FROM PROJEKTY  WHERE NRP = $id";
                    command.Parameters.AddWithValue("$id", j);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projekt.ip = reader.GetInt32(0);
                            projekt.nazwa = reader.GetString(1);
                            projekt.status = reader.GetString(2);
                            projektlist.Add(projekt);
                        }
                    }
                }
            }

            return projektlist;
        }

        public List<Zadanie> listazadan(int a)
        {
            int ip = a;
            using (var connection = new SqliteConnection("Data Source=projekty.db"))
            {
                var i = 0.0;
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"SELECT count(1) FROM ZADANIA WHERE NRP = $ip";
                command.Parameters.AddWithValue("$ip", ip);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var j = reader.GetInt64(0);
                        i = j;
                    }
                }
                int z = ((int)i);
                double[] A = new double[z];


                command = connection.CreateCommand();
                command.CommandText = @"SELECT IZ FROM ZADANIA  WHERE NRP = $id";
                command.Parameters.AddWithValue("$id", ip);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (var d = 0; d < z + 1; d++)
                        {
                            A[d] = reader.GetDouble(d);
                        }
                    }
                }

                for (int s = 0; s < A.Length; s++)
                {
                    double x = A[s];
                    command = connection.CreateCommand();
                    command.CommandText = @"SELECT NAZWA, OPIS, STATUS FROM ZADANIA WHERE NRZ = $id";
                    command.Parameters.AddWithValue("$id", x);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            zadanie.iz = s + 1;
                            zadanie.nazwa = reader.GetString(0);
                            zadanie.opis = reader.GetString(1);
                            zadanie.status = reader.GetString(2);
                            zadanielist.Add(zadanie);
                        }
                    }
                }
            }
            return zadanielist;
        }

        public void dodajzadanie(String nazwa, String opis, int ip)
        {
            zadanielist.Clear();

            using (var connection = new SqliteConnection("Data Source=projekty.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO ZADANIA (NRP, NAZWA, OPIS, STATUS) VALUES ($ip, $nazwa, $opis, NZ)";
                command.Parameters.AddWithValue("$ip", ip);
                command.Parameters.AddWithValue("$nazwa", nazwa);
                command.Parameters.AddWithValue("$opis", opis);
                command.ExecuteReader();
            }
            listazadan(ip);
        }

        public void zmienstatus(String nazwa, String opis, int ip)
        {
            zadanielist.Clear();

            using (var connection = new SqliteConnection("Data Source=projekty.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"UPDATE ZADANIA SET STATUS = Z WHERE NRZ=$ip AND NAZWA = $nazwa AND OPIS=$opis";
                command.Parameters.AddWithValue("$ip", ip);
                command.Parameters.AddWithValue("$nazwa", nazwa);
                command.Parameters.AddWithValue("$opis", opis);
                command.ExecuteReader();
            }
            listazadan(ip);
        }

        public void sprawdzzadanie(int ip)
        {
            bool status = false;
            projektlist.Clear();
            using (var connection = new SqliteConnection("Data Source=projekty.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"SELECT NRZ FROM ZADANIA WHERE NRP=$ip AND STATUS=Z";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var x = reader.GetInt64(0);
                        if (x > 0) { status = true; }   
                    }
                }
            }

            if (status==true) {
                using (var connection = new SqliteConnection("Data Source=projeky.db"))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = @"UPDATE PROJEKTY SET STATUS = Z WHERE NP=$ip";
                    command.Parameters.AddWithValue("$ip", ip);
                    command.ExecuteReader();
                }
            }
            listaprojektow();
        } 
    }
}
