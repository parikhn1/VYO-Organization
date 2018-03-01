using System.Data;
using System.Collections.Generic;
using VYODAL.Infrastructure;
using MySql.Data.MySqlClient;
using VYODomain.Entities;
using System;

namespace VYODAL.DB.Repository
{
    public class VYOUserRepository : BaseRepository
    {
        public List<User> ValidateUser(string UserName, string Userpwd)
        {
            List<User> UserList = new List<User>();
            SetConnection();
            using (MySqlCommand cmd = new MySqlCommand("ValidateUser", Connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@email", UserName);
                cmd.Parameters.AddWithValue("@userpassword", Userpwd);
                MySqlDataReader reader = cmd.ExecuteReader();
                int i = reader.FieldCount;

                while (reader.Read())
                {
                    User UserObj = new User();
                    UserObj.User_FName = reader["User_FName"].ToString();
                    UserObj.User_LName = reader["User_LName"].ToString();
                    UserObj.UserEmail = reader["UserEmail"].ToString();
                    UserObj.UserName = reader["UserName"].ToString();
                    UserList.Add(UserObj);
                }

                reader.Close();
                CloseConnection();
                return UserList;
            }
        }

        public List<User> ValidateUser()
        {
            throw new NotImplementedException();
        }
    }
}