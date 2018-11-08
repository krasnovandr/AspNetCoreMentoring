using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMentoring.API.Contracts.Dto.Category
{
    public class CategoryWriteItemDto
    {
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }
}