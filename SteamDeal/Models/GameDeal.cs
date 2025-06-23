using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamDeal.Models
{
    public class GameDeal
    {
        public string Title { get; set; }
        public string Thumb { get; set; }
        public string SalePrice { get; set; }
        public string DealRating { get; set; }
        public string NormalPrice { get; set; }
        public string Savings { get; set; }
        public string SteamAppID { get; set; }
        public string DealID { get; set; }

    }
}
