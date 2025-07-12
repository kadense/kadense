namespace Kadense.Storage;

public static class TaskExtension
{
    public static async Task<TResult[]> WhenSuccessfulAsync<TResult>(this List<Task<TResult>> tasks, Action<Exception>? onException = null, CancellationToken cancellationToken = default)
    {
        List<TResult> successfulTasks = new List<TResult>(); 
        foreach (var task in tasks)
        {
            try
            {
                await task.WaitAsync(cancellationToken);
                if (task.IsCompletedSuccessfully)
                    successfulTasks.Add(task.Result);
            }
            catch (Exception ex)
            {
                if (onException != null)
                    onException?.Invoke(ex);
            }
        }
        return successfulTasks.ToArray();
    }
}