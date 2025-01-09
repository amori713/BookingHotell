using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotell.Models
{
    public enum RoomType
    {
        Single = 1,  
        Double = 2   
    }

    public class Room
    {
        public int RoomId { get; set; }

       
        public RoomType RoomType { get; set; }  

        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public bool HasExtraBedOption { get; set; }
        public int ExtraBedsAvailable { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
