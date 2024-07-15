using System;

namespace DataBase
{
    internal class IgnoreAuditAttribute : Attribute
    {
        protected bool ignoreToAuditLogs;
        public bool IgnoreToAuditLogs
        {
            get
            {
                return this.ignoreToAuditLogs;

            }
        }

        public IgnoreAuditAttribute(bool value)
        {
            ignoreToAuditLogs = value;
        }
    }
}
