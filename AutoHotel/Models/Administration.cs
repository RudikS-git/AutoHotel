using System;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

using AutoHotel.Interface;

namespace AutoHotel.RoomLodger
{
    public class Administration : Lodger, ISqlControl
    {
        static public void Select(out ObservableCollection<Administration> admins)
        {
            admins = new ObservableCollection<Administration>();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Administrations", sqlConnection);

            try
            {
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        admins.Add(new Administration
                        {
                            Id = reader.GetInt32(0),
                            IdEmession = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            NumberPhone = reader.GetString(3),
                            Email = reader.GetString(4),
                            Remark = reader.GetString(5),
                            DateCheck = reader.GetDateTime(6),
                            DateEviction = reader.GetDateTime(7),
                            NumAdults = reader.GetInt32(8),
                            NumChilds = reader.GetInt32(9)

                        });
                    }
                }

                reader.Close();
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public void Insert()
        {
            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "Id";
            sqlParameter.Size = 4; // 4 byte
            sqlParameter.Direction = System.Data.ParameterDirection.Output; // выходной параметр

            sqlCommand.CommandText = "INSERT INTO Administrations(IdEmession, Name, NumberPhone, Email, Remark, DateCheck, DateEviction, NumAdults, NumChilds) " +
                "VALUES(@IdEmession, @Name, @NumberPhone, @Email, @Remark, @DateCheck, @DateEviction, @NumAdults, @NumChilds) SET @Id=SCOPE_IDENTITY()";
            sqlCommand.Parameters.AddWithValue("@IdEmession", IdEmession);
            sqlCommand.Parameters.AddWithValue("@Name", Name);
            sqlCommand.Parameters.AddWithValue("@NumberPhone", NumberPhone);
            sqlCommand.Parameters.AddWithValue("@Email", Email);
            sqlCommand.Parameters.AddWithValue("@Remark", Remark);
            sqlCommand.Parameters.AddWithValue("@DateCheck", DateCheck);
            sqlCommand.Parameters.AddWithValue("@DateEviction", DateEviction);
            sqlCommand.Parameters.AddWithValue("@NumAdults", NumAdults);
            sqlCommand.Parameters.AddWithValue("@NumChilds", NumChilds);
            sqlCommand.Parameters.Add(sqlParameter);


            SendRequest(sqlCommand);
            Id = Int32.Parse(sqlParameter.Value.ToString());
        }

        public void Delete()
        {
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"DELETE FROM Administrations WHERE Id = {Id}";
            SendRequest(sqlCommand);

        }

        public void Update()
        {
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"UPDATE Administrations SET " +
                $"IdEmession='{IdEmession}', " +
                $"Name='{Name}', " +
                $"NumberPhone='{NumberPhone}', " +
                $"Email='{Email}', " +
                $"Remark='{Remark}', " +
                $"DateCheck='{DateCheck.Year}-{DateCheck.Month}-{DateCheck.Day}', " +
                $"DateEviction='{DateEviction.Year}-{DateEviction.Month}-{DateEviction.Day}', " +
                $"NumAdults='{NumAdults}', " +
                $"NumChilds='{NumChilds}' " +
                $"WHERE Id='{Id}'";
            SendRequest(sqlCommand);
        }

        private void SendRequest(SqlCommand sqlCommand)
        {
            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public DateTime DateCheck { get; set; }
        public DateTime DateEviction { get; set; }
        public int NumChilds { get; set; }
        public int NumAdults { get; set; }
    }
}
