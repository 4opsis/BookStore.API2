﻿using System.ComponentModel.DataAnnotations;

namespace BookStore_UI.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }
        [Required]
        public string ISBN { get; set; }
        [StringLength(150)]
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        [Required]
        public int AuthorID { get; set; }
        public virtual Author Author { get; set; }
    }
}