using System.Text.Json;
using Bogus;
using GetOrderRestApi.Features.CreateOrderRequests.Models;

var shipmentItemFaker = new Faker<ShipmentItem>()
    .RuleFor(i => i.TaxRate, 10);
    // TODO: Add more rules
    // Items: new List<ShipmentItem>
            // {
            //     new ShipmentItem
            //     (
            //         TaxRate: 10,
            //         ItemName: f.Commerce.ProductName(),
            //         IsTaxIncluded: f.Random.Bool(),
            //         Price: f.Random.Int(100, 1000),
            //         Quantity: f.Random.Int(1, 100),
            //         IsSingleItemShipping: f.Random.Bool(),
            //         Tax: null,
            //         SKUCode: f.Random.ArrayElement([ "SKU1", "SKU2", "SKU3" ]),
            //         selectedChoice: f.Random.Words()
            //     )
            // },

var faker = new Faker<Order>()
    .RuleFor(o => o.PaymentMethodName, f => "クレジットカード")
    .RuleFor(o => o.MallOrderNumber, f => f.Random.AlphaNumeric(20))
    .RuleFor(o => o.MallOrderDateTime, f => f.Date.Past())
    .RuleFor(o => o.ShopId, 5000305)
    .RuleFor(o => o.Buyer, f => new Buyer
    (
        Name: "TestBuyer",
        Email: "demo@boss.invalid",
        Phone: f.Random.Number(1000000000, 2000000000).ToString(),
        Address1: "北海道",
        Address2: "TestCity",
        Address3: "TestStreet",
        PostalCode: "000-0000",
        CountryCode: "JP"
    ))
    .RuleFor(o => o.Shipments, f => new List<Shipment>
    {
        new Shipment
        (
            Consignee: new Consignee
            (
                Name: "TestConsignee",
                Phone: f.Random.Number(1000000000, 2000000000).ToString(),
                Address1: "北海道",
                Address2: "TestCity",
                Address3: "TestStreet",
                PostalCode: "000-0000",
                CountryCode: "JP"
            ),
            GiftComments: f.Random.Words(),
            GiftFlag: f.Random.Bool(),
            Noshi: f.Random.Words(),
            DeliveryMethod: "宅配便（RSL）asdf",
            NextDayDelivery: f.Random.Bool(),
            DeliveryDesignatedDate: DateOnly.FromDateTime(f.Date.Future()),
            DeliveryDesignatedTime: null,
            DeliveryComments: f.Random.Words(),
            WarehouseComments: f.Random.Words(),
            WrappingFee: f.Random.Int(0, 1000),
            WrappingTaxRate: 10,
            DeliveryFee: f.Random.Int(0, 1000),
            DeliveryTaxRate: 10,
            CommisionFee: f.Random.Int(0, 1000),
            CommisionTaxRate: 10,
            PointAmount: f.Random.Int(0, 100),
            CouponAmount: f.Random.Int(0, 100),
            Items: shipmentItemFaker.Generate(100),
            DropOffLocation: null
        )
    });
    ;

var orders = faker.Generate(1000);
Console.Write(JsonSerializer.Serialize(orders));