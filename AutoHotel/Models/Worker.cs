using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

using AutoHotel.Enums;

namespace AutoHotel.Models
{
    class Worker : People, INotifyPropertyChanged
    {
        static public void Select(out ObservableCollection<Worker> workers)
        {
            workers = new ObservableCollection<Worker>();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Workers", sqlConnection);

            try
            {
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        workers.Add(new Worker
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            NumberPhone = reader.GetString(2),
                            Email = reader.GetString(3),
                            Login = reader.GetString(4),
                            Pass = reader.GetString(5),
                            Post = (PostOrganization)reader.GetInt32(6)

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

        private string login;
        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        private string pass;
        public string Pass
        {
            get
            {
                return pass;
            }

            set
            {
                pass = value;
                OnPropertyChanged("Pass");
            }
        }

        private PostOrganization post;
        public PostOrganization Post
        {
            get
            {
                return post;
            }

            set
            {
                post = value;
                OnPropertyChanged("Post");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
