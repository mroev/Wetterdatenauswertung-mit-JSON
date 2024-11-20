using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wetterdatenauswertung
{
    public class Stadt
    {
        public required string Name { get; set; }
        public required int Temperatur { get; set; }
        public required int Luftfeuchtigkeit { get; set; }
        public required int Windgeschwindigkeit { get; set; }
    }
}
