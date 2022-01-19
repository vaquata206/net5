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
        /// User code
        /// </summary>
        public const string UserCode = ClaimTypes.Surname;

        /// <summary>
        /// Employee id
        /// </summary>
        public const string EmployeeId = "EmployeeId";

        /// <summary>
        /// Employee Name
        /// </summary>
        public const string EmployeeName = ClaimTypes.Name;

        /// <summary>
        /// Department id
        /// </summary>
        public const string DepartmentId = "DepartmentId";

        /// <summary>
        /// Department name
        /// </summary>
        public const string DepartmentName = "DepartmentName";

        /// <summary>
        /// Position id
        /// </summary>
        public const string PositionId = "PositionId";
    }
}
