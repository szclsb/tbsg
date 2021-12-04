using System;

namespace api.Exceptions
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