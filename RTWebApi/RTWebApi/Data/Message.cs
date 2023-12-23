using System.ComponentModel.DataAnnotations.Schema;

namespace RTWebApi.Data
{
    public class Message
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
