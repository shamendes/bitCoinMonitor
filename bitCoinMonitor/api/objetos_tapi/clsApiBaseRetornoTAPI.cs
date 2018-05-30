using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitCoinMonitor.api.objetos_tapi
{
    class clsApiBaseRetornoTAPI
    {
        public enum enumParMoedas { BRLBTC, BRLLTC, BRLBCH }

        public int status_code { get; set; }
        public string error_message { get; set; }
        public decimal server_unix_timestamp { get; set; }
    }
}
