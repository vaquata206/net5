namespace WebClient.Contexts
{
    /// <summary>
    /// Interface context factory
    /// </summary>
    public interface IContextFactory
    {
        /// <summary>
        /// Get request context
        /// </summary>
        /// <returns>Request context</returns>
        ApplicationContext GetInstance();
    }
}
