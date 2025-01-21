// Generic Item Distribution (2000)
// 90% Single Item Orders (1800):
// Simulate 90% of orders containing one item.
// 5% 2 items (100):
// Simulate 30% of orders with two items (30)
// Include 70% cases where the items are bundled (70).
// 5% Others (100):
// Simulate 5% of orders with more than 2 items

using System.Text.Json;
using Bogus;
using GetOrderRestApi.Features.CreateOrderRequests.Models;

var nonBundledItems = new[]
{
    "Item A", "Item B", "Item C", "Item D", "Item E", "Item F"
};

var bundledItems = new[]
{
    "Bundle X", "Bundle Y", "Bundle Z"
};

var nonBundledItemsSKU = new[]
{
    "Item A", "Item B", "Item C", "Item D", "Item E", "Item F"
};

var bundledItemsSKU = new[]
{
    "Bundle X", "Bundle Y", "Bundle Z"
};


var singleItemFaker = new Faker<ShipmentItem>()
    .RuleFor(i => i.IsTaxIncluded, true)
    .RuleFor(i => i.ItemName, f => f.PickRandom(nonBundledItems)) 
    .RuleFor(i => i.SKUCode, f => f.PickRandom(nonBundledItemsSKU))
    .RuleFor(i => i.Quantity, 1)
    .RuleFor(i => i.TaxRate, 0.1)
    .RuleFor(i => i.Price, f => f.Random.Double(100, 1000)) 
    .RuleFor(i => i.IsSingleItemShipping, false)
    .RuleFor(i => i.SelectedChoice, f => (string)null)
    .RuleFor(i => i.Tax, 0.0);

var bundledItemFaker = new Faker<ShipmentItem>()
    .RuleFor(i => i.IsTaxIncluded, true)
    .RuleFor(i => i.ItemName, f => f.PickRandom(bundledItems))
    .RuleFor(i => i.SKUCode, f => f.PickRandom(bundledItemsSKU))
    .RuleFor(i => i.Quantity, 1) 
    .RuleFor(i => i.TaxRate, 0.1)
    .RuleFor(i => i.Price, f => f.Random.Double(200, 2000)) 
    .RuleFor(i => i.IsSingleItemShipping, false)
    .RuleFor(i => i.SelectedChoice, f => (string)null)
    .RuleFor(i => i.Tax, 0.0);

var shipmentFaker = new Faker<Shipment>()
    .RuleFor(s => s.Consignee, f => new Consignee(
    f.Random.Number(1000000000, 2000000000).ToString(), // Phone
    "北海道",        // Address1
    "TestCity",        // Address2
    "TestStreet",       // Address3
    "000-0000",      // PostalCode
    "JP",            // CountryCode
    "TestConsignee" // Name
    ))
    .RuleFor(s => s.GiftComments, f => f.Random.Words())
    .RuleFor(s => s.GiftFlag, f => f.Random.Bool())
    .RuleFor(s => s.Noshi, f => f.Random.Words())
    .RuleFor(s => s.DeliveryMethod, "宅配便（RSL）asdf") // Why asdf?
    .RuleFor(s => s.NextDayDelivery, f => f.Random.Bool())
    .RuleFor(s => s.DeliveryDesignatedDate, f => DateOnly.FromDateTime(f.Date.Future()))
    .RuleFor(s => s.DeliveryDesignatedTime, f => f.PickRandom<DeliveryDesignatedTimeEnum>())
    .RuleFor(s => s.DeliveryComments, f => f.Random.Words())
    .RuleFor(s => s.WarehouseComments, f => f.Random.Words())
    .RuleFor(s => s.WrappingFee, f => f.Random.Int(0, 1000))
    .RuleFor(s => s.WrappingTaxRate, 10)
    .RuleFor(s => s.DeliveryFee, f => f.Random.Int(0, 1000))
    .RuleFor(s => s.DeliveryTaxRate, 10)
    .RuleFor(s => s.CommisionFee, f => f.Random.Int(0, 1000))
    .RuleFor(s => s.CommisionTaxRate, 10)
    .RuleFor(s => s.PointAmount, f => f.Random.Int(0, 100))
    .RuleFor(s => s.CouponAmount, 0)
    .RuleFor(s => s.Items, f =>
    {
        var random = f.Random.Int(1, 100);
        if (random <= 90)
        {
            // 90% Single Item Orders
            return singleItemFaker.Generate(1);
        }
        else if (random <= 95)
        {
            // 5% Two Items Orders
            if (f.Random.Bool(0.7f)) // 70% bundled
                return bundledItemFaker.Generate(1);
            return singleItemFaker.Generate(2);
        }
        else
        {
            // 5% More than Two Items Orders
            return singleItemFaker.Generate(f.Random.Int(3, 200));
        }
    })
    .RuleFor(s => s.DropOffLocation, f => null);

var orderFaker = new Faker<Order>()
    .RuleFor(o => o.MallOrderDateTime, f => f.Date.Past())
    .RuleFor(o => o.ShopId, 5000305)
    .RuleFor(o => o.MallOrderNumber, f => f.Random.AlphaNumeric(20))
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
    .RuleFor(o => o.OrderComments, "[配送日時指定:]")
    .RuleFor(o => o.CustomerComments, (string)null)
    .RuleFor(o => o.Memo, (string)null)
    .RuleFor(o => o.PaymentMethodName, "銀行振込")
    .RuleFor(o => o.Shipments, f => shipmentFaker.Generate(1).ToList());


var orders = orderFaker.Generate(20);
var json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
var fileName = "generic.json";
File.WriteAllText(fileName, json);
Console.WriteLine($"Order data has been written to the file: {fileName}");
