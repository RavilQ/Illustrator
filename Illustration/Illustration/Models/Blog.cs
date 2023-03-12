using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Illustration.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [MaxLength(170)]
        public string? FirstImage { get; set; }
        [MaxLength(25)]
        public string? FirstTitle { get; set; }
        [MaxLength(600)]
        public string? FirstText { get; set; }
        [MaxLength(170)]
        public string? SecondImage { get; set; }
        [MaxLength(25)]
        public string? SecondTitle { get; set; }
        [MaxLength(600)]
        public string? SecondText { get; set; }
        [MaxLength(150)]
        public string? FirstGreyText { get; set; }
        [MaxLength(30)]
        public string? FirstGreyAuthor { get; set; }
        [MaxLength(170)]
        public string? ThirdImage { get; set; }
        [MaxLength(25)]
        public string? ThirdTitle { get; set; }
        [MaxLength(600)]
        public string? ThirdText { get; set; }
        [MaxLength(150)]
        public string? SecondGreyText { get; set; }
        [MaxLength(30)]
        public string? SecondGreyAuthor { get; set; }
        [NotMapped]
        public IFormFile? FirstImageFile { get; set; }
        [NotMapped]
        public IFormFile? SecondImageFile { get; set; }
        [NotMapped]
        public IFormFile? ThirdImageFile { get; set; }

    }
}
