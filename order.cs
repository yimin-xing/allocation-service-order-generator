using System.Text.Json;
using System.Text.Json.Serialization;

namespace GetOrderRestApi.Features.CreateOrderRequests.Models;

public class Order{
    public string MallOrderNumber { get; set; }
    public DateTimeOffset? MallOrderDateTime { get; set; }
    public int ShopId  { get; set; }
    public Buyer Buyer { get; set; }
    public string? OrderComments { get; set; }
    public string? CustomerComments { get; set; }
    public string? Memo { get; set; }
    public string PaymentMethodName { get; set; }
    public List<Shipment> Shipments { get; set; }
};


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

public record Shipment
(
    Consignee Consignee,
    string? GiftComments,
    bool? GiftFlag,
    string? Noshi,
    string DeliveryMethod,
    bool? NextDayDelivery,
    DateOnly? DeliveryDesignatedDate,
    DeliveryDesignatedTimeEnum? DeliveryDesignatedTime,
    string? DeliveryComments,
    string? WarehouseComments,
    int? WrappingFee,
    int? WrappingTaxRate,
    int? DeliveryFee,
    int? DeliveryTaxRate,
    int? CommisionFee,
    int? CommisionTaxRate,
    int? PointAmount,
    int? CouponAmount,
    List<ShipmentItem> Items,
    string? DropOffLocation
);

public record ShipmentItem
(
    int? TaxRate,
    string ItemName,
    bool? IsTaxIncluded,
    int? Price,
    int? Quantity,
    bool? IsSingleItemShipping,
    int? Tax,
    string SKUCode,
    string? selectedChoice
);

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