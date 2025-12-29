using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;
using bankAccount;

//------------------------------------------------------------
//------------------BNR Currency Converter--------------------
//------------------------------------------------------------

public class BnrCurrencyConverter : ICurrencyConverter
{
    private const string BnrUrl = "https://www.bnr.ro/nbrfxrates.xml"; // BNR XML feed URL

    // Method to get the Euro to RON conversion rate 
    public decimal GetEurToRonRate()
    {
        return GetEurToRonRateAsync().GetAwaiter().GetResult();
    }

    // Asynchronous method to fetch and parse the BNR XML for Euro to RON rate
    public async Task<decimal> GetEurToRonRateAsync()
    {
        using var http = new HttpClient();
        http.Timeout = TimeSpan.FromSeconds(30);
        var xmlContent = await http.GetStringAsync(BnrUrl);
        var doc = XDocument.Parse(xmlContent);

        XNamespace ns = "http://www.bnr.ro/xsd";
        var eurRateElement = doc.Descendants(ns + "Rate")
            .FirstOrDefault(x => (string?)x.Attribute("currency") == "EUR");

        if (eurRateElement == null)
            throw new Exception("EUR rate not found in BNR XML.");

        string valueStr = eurRateElement.Value.Replace(",", ".");
        decimal value = decimal.Parse(valueStr, CultureInfo.InvariantCulture);

        // Check if there's a multiplier attribute
        var multiplierAttr = eurRateElement.Attribute("multiplier");
        if (multiplierAttr != null && int.TryParse(multiplierAttr.Value, out int multiplier))
        {
            value /= multiplier;
        }

        return value;
    }
}
