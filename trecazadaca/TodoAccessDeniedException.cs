using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trecazadaca
{
    class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException(string message) : base(message)
        { }

    }
}
