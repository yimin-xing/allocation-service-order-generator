using System.Text.Json;
using System.Text.Json.Serialization;

namespace GetOrderRestApi.Features.CreateOrderRequests.Models;

public class Order
{
    public string MallOrderNumber { get; set; }
    public DateTimeOffset? MallOrderDateTime { get; set; }
    public int ShopId  { get; set; }
    public Buyer Buyer { get; set; }
    public string? OrderComments { get; set; }
    public string? CustomerComments { get; set; }
    public string? Memo { get; set; }
    public string PaymentMethodName { get; set; }
    public List<Shipment> Shipments { get; set; }
}

public class Shipment
{
    public Consignee Consignee { get; set; }
    public string? GiftComments { get; set; }
    public bool? GiftFlag { get; set; }
    public string? Noshi { get; set; }
    public string DeliveryMethod { get; set; }
    public bool? NextDayDelivery { get; set; }
    public DateOnly? DeliveryDesignatedDate { get; set; }
    public DeliveryDesignatedTimeEnum? DeliveryDesignatedTime { get; set; }
    public string? DeliveryComments { get; set; }
    public string? WarehouseComments { get; set; }
    public int? WrappingFee { get; set; }
    public int? WrappingTaxRate { get; set; }
    public int? DeliveryFee { get; set; }
    public int? DeliveryTaxRate { get; set; }
    public int? CommisionFee { get; set; }
    public int? CommisionTaxRate { get; set; }
    public int? PointAmount { get; set; }
    public int? CouponAmount { get; set; }
    public List<ShipmentItem> Items { get; set; }
    public string? DropOffLocation { get; set; }
}

public class ShipmentItem
{
    public double TaxRate { get; set; }
    public string ItemName { get; set; }
    public bool IsTaxIncluded { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public bool IsSingleItemShipping { get; set; }
    public double Tax { get; set; }
    public string SKUCode { get; set; }
    public string? SelectedChoice { get; set; }
};

public record Buyer(
    string Phone,
    string Address1,
    string Address2,
    string Address3,
    string PostalCode,
    string CountryCode,
    string Name,
    string Email);

public record Consignee(
    string Phone,
    string Address1,
    string Address2,
    string Address3,
    string PostalCode,
    string CountryCode,
    string Name);


[JsonConverter(typeof(DeliveryDesignatedTimeEnumConverter))]
public enum DeliveryDesignatedTimeEnum
{
    _08001200 = 1,
    _12001400 = 2,
    _14001600 = 3,
    _16001800 = 4,
    _18002000 = 5,
    _19002100 = 6,
    _20002100 = 7
}

public class DeliveryDesignatedTimeEnumConverter : JsonConverter<DeliveryDesignatedTimeEnum>
{
    public override DeliveryDesignatedTimeEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var data = reader.GetString();
        return data switch
        {
            "0800-1200" => DeliveryDesignatedTimeEnum._08001200,
            "1200-1400" => DeliveryDesignatedTimeEnum._12001400,
            "1400-1600" => DeliveryDesignatedTimeEnum._14001600,
            "1600-1800" => DeliveryDesignatedTimeEnum._16001800,
            "1800-2000" => DeliveryDesignatedTimeEnum._18002000,
            "1900-2100" => DeliveryDesignatedTimeEnum._19002100,
            "2000-2100" => DeliveryDesignatedTimeEnum._20002100,
            _ => throw new JsonException("""Invalid time range specified; Must be one of the following: "0800-1200", "1200-1400", "1400-1600", "1600-1800", "1800-2000", "1900-2100", "2000-2100" """)
        };
    }

    public override void Write(Utf8JsonWriter writer, DeliveryDesignatedTimeEnum value, JsonSerializerOptions options)
    {
        var data = value switch
        {
            DeliveryDesignatedTimeEnum._08001200 => "0800-1200",
            DeliveryDesignatedTimeEnum._12001400 => "1200-1400",
            DeliveryDesignatedTimeEnum._14001600 => "1400-1600",
            DeliveryDesignatedTimeEnum._16001800 => "1600-1800",
            DeliveryDesignatedTimeEnum._18002000 => "1800-2000",
            DeliveryDesignatedTimeEnum._19002100 => "1900-2100",
            DeliveryDesignatedTimeEnum._20002100 => "2000-2100",
            _ => throw new JsonException("""Invalid time range specified; Must be one of the following: "0800-1200", "1200-1400", "1400-1600", "1600-1800", "1800-2000", "1900-2100", "2000-2100" """)
        };

        writer.WriteStringValue(data);
    }

    public static string ToRequestMessageString(DeliveryDesignatedTimeEnum value)
    {
        return value switch
        {
            DeliveryDesignatedTimeEnum._08001200 => "0812",
            DeliveryDesignatedTimeEnum._12001400 => "1214",
            DeliveryDesignatedTimeEnum._14001600 => "1416",
            DeliveryDesignatedTimeEnum._16001800 => "1618",
            DeliveryDesignatedTimeEnum._18002000 => "1820",
            DeliveryDesignatedTimeEnum._19002100 => "1921",
            DeliveryDesignatedTimeEnum._20002100 => "2021",
            _ => throw new JsonException("""Invalid time range specified; Must be one of the following: "0800-1200", "1200-1400", "1400-1600", "1600-1800", "1800-2000", "1900-2100", "2000-2100" """)
        };
    }
}
