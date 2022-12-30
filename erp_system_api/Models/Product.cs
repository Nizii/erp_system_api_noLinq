﻿
namespace erp_system_api
{
    public class Product
    {
        public Int32 product_nr { get; set; }

        public String product_name { get; set; }

        public String product_size { get; set; }

        public String description { get; set; }

        public Int32 units_available { get; set; }

        public String unit { get; set; }

        public decimal purchasing_price_per_unit { get; set; }

        public decimal selling_price_per_unit { get; set; }
    }
}
