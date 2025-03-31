namespace Kadense.Template.Operator
{
    public class Operator
    {
        public static void Main(string[] args)
        {
            var watcherService = new WatcherService();
            watcherService.Start();
        }
    }
}