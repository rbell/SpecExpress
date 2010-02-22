using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpress.Web
{
    public class ValidationNotificationEventArgs
    {
        private ValidationNotification _ve;
        public ValidationNotificationEventArgs(ValidationNotification vn)
        {
            _ve = vn;
        }

        public ValidationNotification ValidationNotification
        {
            get { return _ve; }
        }
    }
}
