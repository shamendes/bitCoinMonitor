using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace bitCoinMonitor.api.objetos_tapi
{

    public class clsApiOrders_data
    {
        public List<clsApiOrders_orders_data> orders;
        public clsApiOrders_orders_data order;
    }


    public class clsApiOrders_orders_data
    {
        public int order_id { get; set; }
        public string coin_pair { get; set; }
        public int order_type { get; set; }
        public int status { get; set; }
        public bool has_fills { get; set; }
        public string quantity { get; set; }
        public string limit_price { get; set; }
        public string executed_quantity { get; set; }
        public string executed_price_avg { get; set; }
        public string fee { get; set; }
        public string created_timestamp { get; set; }
        public string updated_timestamp { get; set; }
        public List<clsApiOrders_operations_data> operations { get; set; }
    }

    public class clsApiOrders_operations_data
    {
        public int operation_id { get; set; }
        public string quantity { get; set; }
        public string price { get; set; }
        public string fee_rate { get; set; }
        public string executed_timestamp { get; set; }
    }

    class clsApiOrders:clsApiBaseRetornoTAPI
    {
        public clsApiOrders_data response_data { get; set; }

        public clsApiOrders()
        {

        }


        public clsApiOrders(string aStrJson)
        {
            try
            {
                clsApiOrders vObjOrders = JsonConvert.DeserializeObject<clsApiOrders>(aStrJson);

                this.response_data = vObjOrders.response_data;
                this.status_code = vObjOrders.status_code;
                this.error_message = vObjOrders.error_message;
            }
            catch
            {
                throw;
            }
        }
    }





}
