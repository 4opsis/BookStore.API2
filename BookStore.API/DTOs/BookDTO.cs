using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.DTOs
{
    public class BookDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string ISBN { get; set; }
        public string Summarty { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public int? AuthorID { get; set; }
        public virtual AuthorDTO Author { get; set; }
    }
    public class BookCreateDTO
    {
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }
        [Required]
        public string ISBN { get; set; }
        [StringLength(500)]
        public string Summarty { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        [Required]
        public int AuthorID { get; set; }
    }
    public class BookUpdateDTO
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }
        [StringLength(500)]
        public string Summarty { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public int? AuthorID { get; set; }
    }
}
