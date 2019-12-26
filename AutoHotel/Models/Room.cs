using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Configuration;

using AutoHotel.Enums;
using AutoHotel.Interface;

namespace AutoHotel.RoomLodger
{
    public class Room : ISqlControl, INotifyPropertyChanged, IDataErrorInfo
    {
        static private SqlConnection sqlConnection;

        private int id;
        private int number;
        private TypeRoom typeroom;
        private PlaceRoom placeroom;
        private FeatureRoom featureroom;

        static Room()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        static public void Select(out ObservableCollection<Room> rooms)
        {
            rooms = new ObservableCollection<Room>();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Rooms", sqlConnection);

            try
            {
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room
                        {
                            Id = reader.GetInt32(0),
                            Number = reader.GetInt32(1),
                            TypeRoom = (TypeRoom)reader.GetInt32(2),
                            PlaceRoom = (PlaceRoom)reader.GetInt32(3),
                            FeatureRoom = (FeatureRoom)reader.GetInt32(4)
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

            sqlCommand.CommandText = "INSERT INTO Rooms(Number, TypeRoom, PlaceRoom, FeatureRoom) VALUES(@Number, @TypeRoom, @PlaceRoom, @FeatureRoom) SET @Id=SCOPE_IDENTITY()";
            sqlCommand.Parameters.AddWithValue("@Number", Number);
            sqlCommand.Parameters.AddWithValue("@TypeRoom", TypeRoom);
            sqlCommand.Parameters.AddWithValue("@PlaceRoom", PlaceRoom);
            sqlCommand.Parameters.AddWithValue("@FeatureRoom", FeatureRoom);
            sqlCommand.Parameters.Add(sqlParameter);


            SendRequest(sqlCommand);
            Id = Int32.Parse(sqlParameter.Value.ToString());
        }

        public void Delete()
        {
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"DELETE FROM Rooms WHERE Id = {Id}";
            SendRequest(sqlCommand);

        }

        public void Update()
        {
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"UPDATE Rooms SET " +
                $"Number={Number}, " +
                $"TypeRoom={(int)TypeRoom}, " +
                $"PlaceRoom={(int)PlaceRoom}, " +
                $"FeatureRoom={(int)FeatureRoom} " +
                $"WHERE Id={Id}";
            SendRequest(sqlCommand);
        }

        static private void SendRequest(SqlCommand sqlCommand)
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

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public int Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }
        public TypeRoom TypeRoom
        {
            get
            {
                return typeroom;
            }

            set
            {
                typeroom = value;
                OnPropertyChanged("TypeRoom");
            }
        }
        public PlaceRoom PlaceRoom
        {
            get
            {
                return placeroom;
            }

            set
            {
                placeroom = value;
                OnPropertyChanged("PlaceRoom");
            }
        }
        public FeatureRoom FeatureRoom
        {
            get
            {
                return featureroom;
            }

            set
            {
                featureroom = value;
                OnPropertyChanged("FeatureRoom");
            }
        }

        public string this[string propertyName]
        {
            get
            {
                string validationResult = String.Empty;
                switch (propertyName)
                {
                    case "Id":
                        break;
                    case "Number":
                        validationResult = ValidateNumber();
                        break;
                    case "TypeRoom":
                        break;
                    case "PlaceRoom":
                        break;
                    case "FeatureRoom":
                        break;
                    default:
                        throw new ApplicationException("Неизвестное свойство пытается пройти валидацию!");
                }
                return validationResult;
            }
        }

        private string ValidateNumber()
        {
            if (Number < 0)
            {
                return "Номер комнаты не может быть отрицательным!";
            }
            else
            {
                return String.Empty;
            }
        }

        public string Error
        {
            get
            {
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
