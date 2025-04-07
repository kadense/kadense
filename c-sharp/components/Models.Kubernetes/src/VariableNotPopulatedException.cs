using System;

namespace Kadense.Models.Kubernetes
{
    public class VariableNotPopulatedException : Exception
    {

        public string? VariableName { get; set; }
        public VariableNotPopulatedException(string message) : base(message)
        {

        }

        public VariableNotPopulatedException(string message, string variableName) : base(message)
        {
            this.VariableName = variableName;  
        }
    }
}