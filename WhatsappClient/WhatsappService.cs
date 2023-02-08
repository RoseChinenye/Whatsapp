using Microsoft.Data.SqlClient;
using System.Data;
using Whatsapp.Model;

namespace Whatsapp
{
    public class WhatsappService : IWhatsappService
    {
        private readonly WhatsappDbContext _dbContext;
        private bool _disposed;

        public WhatsappService(WhatsappDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<long> CreateUser(UserViewModel user)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            var UserName = new SqlParameter
            {
                ParameterName= "@name",
                Value = user.UserName,
                SqlDbType = SqlDbType.VarChar,
                Direction= ParameterDirection.Input,
                Size= 50,
            };

            var PhoneNumber = new SqlParameter
            {
                ParameterName = "@PhoneNumber",
                Value = user.PhoneNumber,
                SqlDbType = SqlDbType.VarChar,
                Direction= ParameterDirection.Input,
                Size= 20,
            };

            string insertQuery =
                $"INSERT INTO USERS (UserName, PhoneNumber)" +
                $" VALUES (@name,@PhoneNumber); SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            

            await using SqlCommand command = new SqlCommand(insertQuery, sqlConn);

            command.Parameters.Add(UserName);
            command.Parameters.Add(PhoneNumber);

            long userId = (long) await command.ExecuteScalarAsync();
            
            
            return userId;

        }

        public async Task<bool> UpdateUser(int id, UserViewModel user)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            var UserName = new SqlParameter
            {
                ParameterName= "@name",
                Value = user.UserName,
                SqlDbType = SqlDbType.VarChar,
                Direction= ParameterDirection.Input,
                Size= 50,
            };

            var PhoneNumber = new SqlParameter
            {
                ParameterName = "@PhoneNumber",
                Value = user.PhoneNumber,
                SqlDbType = SqlDbType.VarChar,
                Direction= ParameterDirection.Input,
                Size= 20,
            };

            string updateQuery =
               $" UPDATE Users set Username = @name, PhoneNumber = @PhoneNumber WHERE UserID = {id}";

            await using SqlCommand command = new SqlCommand(updateQuery, sqlConn);

            command.Parameters.Add(UserName);
            command.Parameters.Add(PhoneNumber);

            var updatedresult = await command.ExecuteNonQueryAsync();

            return updatedresult != 0 ? true : false;
            

        }

        public async Task<bool> DeleteUser(int id)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            string deleteQuery = $"DELETE FROM Users WHERE userID = @UserId ";
            await using SqlCommand command = new SqlCommand(deleteQuery, sqlConn);


            command.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter
                {
                    ParameterName = "@UserId",
                    Value = id,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
            });

            var result = command.ExecuteNonQuery();


            return (result != 0);

            

        }

        public async Task<UserViewModel> GetUser(int id)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();
            
            string getUserQuery = $"SELECT Users.UserName,Users.PhoneNumber FROM Users WHERE UserID = @UserId ";

            await using SqlCommand command = new SqlCommand(getUserQuery, sqlConn);
            
            command.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter
                {
                    ParameterName = "@UserId",
                    Value = id,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    Size = 50
                }
            });

            UserViewModel user = new UserViewModel();

            using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
            {
                
                while(dataReader.Read())
                {
                    user.UserName = dataReader["UserName"].ToString();
                    user.PhoneNumber = dataReader["PhoneNumber"].ToString();
                }
            }
            return user;
        }

        public async Task<IEnumerable<UserViewModel>> GetUsers()
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();
            
            string getAllUsersQuery = $"SELECT Users.UserName,Users.PhoneNumber FROM Users";
            
            await using SqlCommand command = new SqlCommand(getAllUsersQuery, sqlConn);
            
            List<UserViewModel> users = new List<UserViewModel>();
            
            using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
            {
                while (dataReader.Read())
                {
                    users.Add(
                        new UserViewModel() 
                        {
                            UserName = dataReader["UserName"].ToString(),
                            PhoneNumber = dataReader["PhoneNumber"].ToString()
                        }
                );
                }
            }

            return users;
        }



        protected virtual void Dispose(bool disposing)
        {

            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



    }
}
