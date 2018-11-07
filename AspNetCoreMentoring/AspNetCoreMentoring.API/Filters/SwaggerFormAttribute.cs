using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.API.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SwaggerFormAttribute : Attribute
    {
        public SwaggerFormAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
