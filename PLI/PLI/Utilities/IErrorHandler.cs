using System;

namespace PLI.Utilities
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
