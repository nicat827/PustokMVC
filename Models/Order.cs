using PustokApp.Utilities.Enums;

namespace PustokApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        public decimal GrandTotal { get; set; }

        public OrderStatus Status { get; set; }

        public List<BasketItem> BasketItems { get; set; }

        public DateTime CreateDate { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }


    }
}
