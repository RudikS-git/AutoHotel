using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AutoHotel.Models
{
    public class People : IDataErrorInfo
    {
        static protected SqlConnection sqlConnection; // пусть для бд

        static People()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        } // статический конструктор

        public int Id { get; set; }
        public string Name { get; set; }
        public string NumberPhone { get; set; }
        public string Email { get; set; }

        public string this[string propertyName]
        {
            get
            {
                string validationResult = String.Empty;
                switch (propertyName)
                {
                    case "Id":
                        break;
                    case "IdEmession":
                        break;
                    case "Name":
                        validationResult = ValidateName();
                        break;
                    case "NumberPhone":
                        validationResult = ValidatePhone();
                        break;
                    case "Email":
                        validationResult = ValidateEmail();
                        break;
                    case "Remark":
                        break;
                    default:
                        throw new ApplicationException("Неизвестное свойство пытается пройти валидацию!");
                }
                return validationResult;
            }
        } // индексатор для валидации

        private string ValidateName()
        {

            string pattern = @"^[А-ЯЁ][а-яё]+ [А-ЯЁ][а-яё]+ [А-ЯЁ][а-яё]+$";

            if (Name == null)
            {
                return "Значение не должно быть пустым!";
            }
            else if (!Regex.IsMatch(Name, pattern))
            {
                return "Некорректное ФИО \nПример: Иванов Иван Иванович";
            }
            else
            {
                return String.Empty;
            }
        } // проверка имени

        private string ValidatePhone()
        {
            string pattern = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";

            if (NumberPhone == null)
            {
                return "Значение не должно быть пустым!";
            }
            else if (!Regex.IsMatch(NumberPhone, pattern))
            {
                return "Некорректный номер \nПример: +79124738254";
            }
            else
            {
                return String.Empty;
            }
        } // проверка телефона

        private string ValidateEmail()
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$"; // взято у microsoft'a

            if (Email == null)
            {
                return "Значение не должно быть пустым!";
            }
            else if (Email == "")
            {
                return String.Empty;
            }
            else if (!Regex.IsMatch(Email, pattern))
            {
                return "Некорректная почта! \nПример: ivanov@gmail.com";
            }
            else
            {
                return String.Empty;
            }
        } // проверка почты

        public string Error
        {
            get
            {
                return null;
            }
        }
    }
}
