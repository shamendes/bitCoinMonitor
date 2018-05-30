using System;
using Newtonsoft.Json;
using bitCoinMonitor.tools;

namespace bitCoinMonitor.api.objetos_tapi
{

    public class clsApiTickerBitstamp_data
    {
        public decimal high { get; set; }
        public decimal last { get; set; }
        public decimal timestamp { get; set; }
        public decimal bid { get; set; }
        public decimal vwap { get; set; }
        public decimal volume { get; set; }
        public decimal low { get; set; }
        public decimal ask { get; set; }
        public double open { get; set; }


        public DateTime verDataHora()
        {
            return clsTooUtil.converterUnixTimeStamp(Convert.ToDecimal(this.timestamp));
        }

    }


    class clsApiTickerBitstamp
    {
        public clsApiTickerBitstamp_data ticker;



        public clsApiTickerBitstamp()
        {
            this.ticker = new clsApiTickerBitstamp_data();
            this.ticker.bid = 0;
            this.ticker.ask = 0;

        }

        public clsApiTickerBitstamp(string aStrJson)
        {
            try
            {
                clsApiTickerBitstamp_data vObjTicker = JsonConvert.DeserializeObject<clsApiTickerBitstamp_data>(aStrJson);

                this.ticker = vObjTicker;
            }
            catch
            {
                throw;
            }

        }

    }
}
