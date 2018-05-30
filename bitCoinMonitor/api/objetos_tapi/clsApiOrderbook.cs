using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace bitCoinMonitor.api.objetos_tapi
{

    #region Mercado Bitcoin

    public class clsApiOrderbook_data
    {
        public clsApiOrderbook_orderbook_data orderbook { get; set; }
    }


    public class clsApiOrderbook_orderbook_data
    {
        public List<clsApiOrderbook_bids_asks_data> bids { get; set; }
        public List<clsApiOrderbook_bids_asks_data> asks { get; set; }
        public int latest_order_id { get; set; }

    }

    public class clsApiOrderbook_bids_asks_data
    {
        public int order_id { get; set; }
        public string quantity { get; set; }
        public string limit_price { get; set; }
        public bool is_owner { get; set; }
    }
    #endregion

    #region FoxBit
    class clsApiTickerFoxBit
    {
        public string pair { get; set; }
        public clsApiBidsAsks[] bids { get; set; }
        public clsApiBidsAsks[] asks { get; set; }
    }
    class clsApiBidsAsks
    {
        public decimal[] unit_price;
        public decimal[] amount;
        public int[] userID;
    }
    #endregion




    class clsApiOrderbook : clsApiBaseRetornoTAPI
    {
        #region Mercado Bitcoin
        public clsApiOrderbook_data response_data { get; set; }
        #endregion
        #region FoxBit
        public clsApiTickerFoxBit ticker { get; set; }
        #endregion

        public clsApiOrderbook()
        {

        }


        public clsApiOrderbook(string aStrJson)
        {
            try
            {
                clsApiOrderbook vObjOrderBook = JsonConvert.DeserializeObject<clsApiOrderbook>(aStrJson);

                this.response_data = vObjOrderBook.response_data;
                this.status_code = vObjOrderBook.status_code;
                this.error_message = vObjOrderBook.error_message;
                this.ticker = vObjOrderBook.ticker;
            }
            catch
            {
                throw;
            }
        }
    }
}
