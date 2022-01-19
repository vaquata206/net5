using System.ComponentModel.DataAnnotations;

namespace WebClient.Core.ViewModels
{
    public class PagingRequest<TFilter> where TFilter : class
    {
        /// <summary>
        /// Draw counter
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Paging first record indicator. Default: 0
        /// </summary>
        [Required]
        public int Start { get; set; }

        /// <summary>
        /// Number of records that the table can display in the current draw.
        /// </summary>
        [Required]
        public int Length { get; set; }

        /// <summary>
        /// filter
        /// </summary>
        [Required]
        public TFilter Filter { get; set; }

        public string OrderBy { get; set; }
    }
}
