using System.Security.Claims;

namespace WebClient.Extensions
{
    /// <summary>
    /// Claim request type
    /// </summary>
    public static class ClaimAccountType
    {
        /// <summary>
        /// User name
        /// </summary>
        public const string UserName = ClaimTypes.NameIdentifier;

        /// <summary>
        /// User id
        /// </summary>
        public const string UserId = "UserId";

        /// <summary>
        /// Employee Name
        /// </summary>
        public const string FullName = ClaimTypes.Name;

        /// <summary>
        /// Department id
        /// </summary>
        public const string DepartmentId = "DepartmentId";

        /// <summary>
        /// Department name
        /// </summary>
        public const string DepartmentName = "DepartmentName";


        /// <summary>
        /// AccountType
        /// </summary>
        public const string AccountType = "AccountType";

        /// <summary>
        /// Investor id
        /// </summary>
        public const string CustomerId = "CustomerId";

        /// <summary>
        /// Employee id
        /// </summary>
        public const string EmployeeId = "EmployeeId";

        /// <summary>
        /// Status 
        /// </summary>
        public const string Status = "Status";

        /// <summary>
        /// LoginType 
        /// </summary>
        public const string RoleType = "RoleType";
    }
}
