using System;
using System.Collections.Generic;
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
        public double? Price { get; set; }
        public int? AuthorID { get; set; }
        public virtual AuthorDTO Author { get; set; }
    }
}
