namespace WebClient.ViewModels
{
    public class Alert
    {
        public enum AlertType
        {
            Info = 0,
            Success = 1,
            Error = 2,
            Warning = 3
        }

        public int Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Timer { get; set; }
    }
}
