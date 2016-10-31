using System;

namespace Sitko.ModelSelector.Exceptions
{
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException(string message) : base(message)
        {
        }
    }
}