using System;
using Newtonsoft.Json;
using bitCoinMonitor.tools;

namespace bitCoinMonitor.api.objetos_tapi
{

    public class clsApiTicker_data
    {
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal vol { get; set; }
        public decimal vol_brl { get; set; }
        public decimal last { get; set; }
        public decimal buy { get; set; }
        public decimal sell { get; set; }
        public decimal date { get; set; }
        public string pair { get; set; }


        public DateTime verDataHora()
        {
            return clsTooUtil.converterUnixTimeStamp(this.date);
        }

    }

    
    class clsApiTicker
    {
        public clsApiTicker_data ticker;



        public clsApiTicker()
        {
            this.ticker = new clsApiTicker_data();
            this.ticker.buy = 0;
            this.ticker.sell = 0;

        }

        public clsApiTicker(string aStrJson)
        {
            try
            {
                clsApiTicker vObjTicker = JsonConvert.DeserializeObject<clsApiTicker>(aStrJson);

                this.ticker = vObjTicker.ticker;
            }
            catch
            {
                throw;
            }

        }

    }
}
