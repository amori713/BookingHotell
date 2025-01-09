using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotell.Models
{
    public enum RoomType
    {
        Single = 1,  // Enkelrum
        Double = 2   // Dubbelrum
    }

    public class Room
    {
        public int RoomId { get; set; }

        // Här använder vi enumen istället för en sträng
        public RoomType RoomType { get; set; }  // Rumstyp: Single eller Double

        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public bool HasExtraBedOption { get; set; }
        public int ExtraBedsAvailable { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
