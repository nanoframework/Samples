using System;

namespace Reflection.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class IgnoreAttribute : Attribute
    {
        public IgnoreAttribute() : this(null)
        {
        }

        public IgnoreAttribute(string ignoreMessage)
        {
            IgnoreMessage = ignoreMessage;
        }

        private string _ignoreMessage;

        public string IgnoreMessage
        {
            get
            {
                return _ignoreMessage;
            }

            set
            {
                _ignoreMessage = value;
            }
        }
    }
}
