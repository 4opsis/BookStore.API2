namespace BookStore_UI.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string ISBN { get; set; }
        public string Summarty { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public int? AuthorID { get; set; }
        public virtual Author Author { get; set; }
    }
}