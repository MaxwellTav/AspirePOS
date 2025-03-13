using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Aspire_POS.Models
{
    public class OrderRequestModel
    {
        [JsonPropertyName("estado")]
        public string Estado { get; set; }
        public List<OrderItemModel> Productos { get; set; }
    }

    public class OrderItemModel
    {
        [JsonPropertyName("product_Id")]
        public int ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

    }

}
