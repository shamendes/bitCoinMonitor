using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace bitCoinMonitor.api.objetos_tapi
{

    public class clsApiAccountInfo_data
    {
        public clsApiAccountInfo_balance_data balance { get; set; }
        public  clsApiAccountInfo_withdrawal_data withdrawal_limits { get; set; }
    }  
    public class clsApiAccountInfo_balance_data
    {
        public clsApiAccountInfo_balance_brl_data brl { get; set; }
        public clsApiAccountInfo_balance_btc_data btc { get; set; }
        public clsApiAccountInfo_balance_ltc_data ltc { get; set; }
        public clsApiAccountInfo_balance_bch_data bch { get; set; }
    }
    public class clsApiAccountInfo_withdrawal_data
    {
        public clsApiAccountInfo_withdrawal_brl_data brl { get; set; }
        public clsApiAccountInfo_withdrawal_btc_data btc { get; set; }
        public clsApiAccountInfo_withdrawal_ltc_data ltc { get; set; }
        public clsApiAccountInfo_withdrawal_bch_data bch { get; set; }
    }
    public class clsApiAccountInfo_balance_brl_data
    {
        public string available { get; set; }
        public string total { get; set; }
    }
    public class clsApiAccountInfo_balance_btc_data
    {
        public string available { get; set; }
        public string total { get; set; }
        public int amount_open_orders { get; set; }
    }
    public class clsApiAccountInfo_balance_ltc_data
    {
        public string available { get; set; }
        public string total { get; set; }
        public int amount_open_orders { get; set; }
    }
    public class clsApiAccountInfo_balance_bch_data
    {
        public string available { get; set; }
        public string total { get; set; }
        public int amount_open_orders { get; set; }
    }
    public class clsApiAccountInfo_withdrawal_brl_data
    {
        public string available { get; set; }
        public string total { get; set; }
    }
    public class clsApiAccountInfo_withdrawal_btc_data
    {
        public string available { get; set; }
        public string total { get; set; }
    }
    public class clsApiAccountInfo_withdrawal_ltc_data
    {
        public string available { get; set; }
        public string total { get; set; }
    }
    public class clsApiAccountInfo_withdrawal_bch_data
    {
        public string available { get; set; }
        public string total { get; set; }
    }



    class clsApiAccountInfo : clsApiBaseRetornoTAPI
    {

        public clsApiAccountInfo_data response_data { get; set; }

        public clsApiAccountInfo()
        {

        }

        public clsApiAccountInfo(string aStrJson)
        {
            try
            {
                clsApiAccountInfo vObjAccountInfo = JsonConvert.DeserializeObject<clsApiAccountInfo>(aStrJson);

                this.response_data = vObjAccountInfo.response_data;
                this.status_code = vObjAccountInfo.status_code;
                this.error_message = vObjAccountInfo.error_message;
            }
            catch
            {
                throw;
            }
        }

    }
}
