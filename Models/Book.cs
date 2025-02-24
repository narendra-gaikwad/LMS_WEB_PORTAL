namespace LMS_WEB_PORTAL.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int TotalCopies { get; set; }    
        public int IssuedCopies { get; set; }

    
        public bool IsIssued { get; set; }

     
        public int AvailableCopies => Math.Max(TotalCopies - IssuedCopies, 0);

    }
}
