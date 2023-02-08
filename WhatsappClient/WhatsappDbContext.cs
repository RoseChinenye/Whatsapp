using Microsoft.Data.SqlClient;

namespace Whatsapp
{
    public class WhatsappDbContext : IDisposable
    {
        private readonly string _connString;

        private SqlConnection _dbConnection = null;
        public WhatsappDbContext():this(@"Data Source=LAPTOP-AI62M7MS\SQLEXPRESS; Initial Catalog = WhatsappDB; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
        {
            
        }

        public WhatsappDbContext(string connString)
        {
            _connString = connString;
        }

        public async Task<SqlConnection> OpenConnection()
        {
            _dbConnection = new SqlConnection(_connString);
            await _dbConnection.OpenAsync();
            return _dbConnection;
        }

        public async Task CloseConnection()
        {
            if(_dbConnection?.State != System.Data.ConnectionState.Closed)
            {
                await _dbConnection?.CloseAsync();
            }
        }

        bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _dbConnection.Dispose();
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
