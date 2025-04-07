using System;

namespace Kadense.Models.Kubernetes
{
    public class AwaitingDependencyException : Exception
    {

        public string? VariableName { get; set; }
        public AwaitingDependencyException(string message) : base(message)
        {

        }

        public AwaitingDependencyException(string message, string variableName) : base(message)
        {
            this.VariableName = variableName;  
        }
    }
}