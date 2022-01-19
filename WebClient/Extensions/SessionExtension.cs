using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebClient.Extensions
{
    /// <summary>
    /// Session extension class
    /// </summary>
    public static class SessionExtension
    {
        /// <summary>
        /// Session key menu
        /// </summary>
        public static readonly string SessionKeyMenu = "KEY_SESSION_MENU";

        /// <summary>
        /// Store a object to session
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value that we want to store</param>
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Get value by key 
        /// </summary>
        /// <typeparam name="T">Type of object that we want to response</typeparam>
        /// <param name="session">The session</param>
        /// <param name="key">The key</param>
        /// <returns>Value map to key</returns>
        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
