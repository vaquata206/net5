using Microsoft.Extensions.Configuration;

namespace WebClient.Core
{
    public class AppSetting
    {
        public static readonly string Key_BaseUrl = "BaseUrl";
        public static readonly string Key_ConnectionString = "ConnectionString";
        public static readonly string Key_ExpiredTicket = "ExpiredTicket";
        public static readonly string Key_ResourceVersion = "ResourceVersion";
        public static readonly string Key_Enviroment = "Enviroment";
        public static readonly string Key_Version = "Version";
        public static readonly string Key_SSOServiceName = "SSOService:ServiceName";
        public static readonly string Key_SSOServer = "SSOService:Server";
        public static readonly string Key_SyncUrl = "SyncUrl";

        private readonly IConfiguration Configuration;
        private string connectionString;
        private int? expiredTicket;
        private string resourceVersion;
        private string enviroment;
        private string version;
        private string baseUrl;
        private string sSOServiceName;
        private string sSOServer;
        private string syncUrl;

        public AppSetting(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public string ConnectionString => this.connectionString ??= this.Configuration[Key_ConnectionString];

        /// <summary>
        ///  the time at which the authentication ticket expires
        ///  unit is minutes
        /// </summary>
        public int ExpiredTicket => this.expiredTicket ??= int.Parse(this.Configuration[Key_ExpiredTicket]);

        public string ResourceVersion => this.resourceVersion ??= this.Configuration[Key_ResourceVersion];
        public string Enviroment => this.enviroment ??= this.Configuration[Key_Enviroment];
        
        /// <summary>
        /// System version
        /// </summary>
        public string Version => this.version ??= this.Configuration[Key_Version];

        /// <summary>
        /// Url api system
        /// </summary>
        public string BaseUrl => this.baseUrl ??= this.Configuration[Key_BaseUrl];

        /// <summary>
        /// SSO service name
        /// </summary>
        public string SSOServiceName => this.sSOServiceName ??= this.Configuration[Key_SSOServiceName].Trim('/');

        /// <summary>
        /// SSO server
        /// </summary>
        public string SSOServer => this.sSOServer ??= this.Configuration[Key_SSOServer];

        public string SyncUrl => this.syncUrl ??= this.Configuration[Key_SyncUrl];

    }
}
