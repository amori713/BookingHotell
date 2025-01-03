using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotell.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomType { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public bool HasExtraBedOption { get; set; }  
        public int ExtraBedsAvailable { get; set; }
    }
}
