using Kadense.Malleable.Reflection;
using Kadense.Malleable.Workflow.Processing;
using Kadense.Malleable.Workflow.Connections;
using Kadense.Malleable.Workflow.Validation;

namespace Kadense.Malleable.Workflow
{
    public class MalleableWorkflowCoordinatorActor : UntypedActor
    {
        public MalleableWorkflowCoordinatorActor(MalleableWorkflowContext workflowContext, MalleableWorkflowAction actionMapper, MalleableWorkflowValidator validator)
        {
            WorkflowContext = workflowContext;
            
            if(!validator.Validate(WorkflowContext))
                throw new InvalidOperationException($"Workflow {WorkflowContext.Workflow.Metadata.Name} is not valid.");

            foreach(var step in WorkflowContext.Workflow.Spec!.Steps!)
            {
                Props props = actionMapper.CreateProps(WorkflowContext, step.Key)!;

                if (props == null)
                    throw new InvalidOperationException($"Action function for step {step.Key} returned null.");
                
                var actor = Context.ActorOf(props, step.Key);
                this.WorkflowContext.Destinations.Add(step.Key, new ActorConnection(actor));
            }
        }

        public MalleableWorkflowContext WorkflowContext { get; } 
        protected override void OnReceive(object message)
        {
            throw new NotImplementedException();
        }
    }
}