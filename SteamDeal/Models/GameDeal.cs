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

        // Calculated property
        public string SavingsPercent =>
            int.TryParse(Savings?.Split('.')[0], out var val) ? val.ToString() : "0";
    }
}
