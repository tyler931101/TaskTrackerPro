using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagementSystem.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "To Do";  // To Do | Progress | Review | Done | Closed

        [Required]
        public DateTime DueDate { get; set; }

        // 👇 who is responsible for this ticket
        public int? AssignedUserId { get; set; }

        [ForeignKey(nameof(AssignedUserId))]
        public User? AssignedUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}