using System;

namespace server.Exceptions
{
    public class DatabaseException: Exception
    {
        public DatabaseException()
        {
        }
    }

    public class DoublicateException : DatabaseException
    {
        public DoublicateException() : base()
        {
            
        }
    }
}