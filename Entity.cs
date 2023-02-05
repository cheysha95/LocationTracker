
using System.Text.Json.Nodes;

namespace ConsoleApp2; 

public class Entity {
    private string City { get; set; }
    private string County { get; set; }
    string CountryCode = "USA";
    private int zipCode { get; set; }
    private string State { get; set; }
    private string StateCode { get; set; }
    private int Population { get; set; }
    private int LandArea { get; set; }
    
    private int AverageIncome { get; set; }
    private float UnemploymentRate { get; set; }
    private float CreimeRate { get; set; }
    
    private float AveragePercipitation { get; set; }
    private string climate { get; set; }
    private AverageTemp[] AverageTemps { get; set; }
    private string CurrentWeatherDescription { get; set; }
    private int CurrentTemp { get; set; }
    private int CurrentFeelsLike { get; set; }
    private int CurrentHumidity { get; set; }
    private int TempMin { get; set; }
    private int TempMax { get; set; }


    public Entity(string city, string stateCode) {
        //stores the state code passed in but gets the city name from the api
        this.StateCode = stateCode;
        this.City = city;
        
        GetCityInfo();
        GetWeatherInfoOpenWeatherMap();
        
        Console.WriteLine("City: " + City);
    }

    // OpenStreetMap API fills in city, county, state, zip code
    private void GetCityInfo() {
        // need to us $"" to use variables in url
        string url = $"https://nominatim.openstreetmap.org/search?q={City},{StateCode}&format=json";

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "C# App"); // required to use api, thanks chatgpt
        
        var response = client.GetAsync(url).Result;
        var content = response.Content.ReadAsStringAsync().Result;
        var json = JsonArray.Parse(content);
    
        //parse json into class variables
        var tempString = json[0]["display_name"].ToString(); 

        County = tempString.Split(",")[1];
        State = tempString.Split(",")[2];
        zipCode = int.Parse(tempString.Split(",")[3]);
        
        client.Dispose();
    }
    
    
    // OpenWeather API fills in weather info, WEATHER APIS SUCK average percipitation, climate, average temps
    public void GetWeatherInfoNOAA() {
        // this is using the NOAA api, which is a bit more complicated than i need, but probaly has everything i could want, but may want to use a different one
        // https://www.ncei.noaa.gov/support/access-data-service-api-user-documentation
        
        //string baseUrl = "https://www.ncei.noaa.gov/cdo-web/api/v2/data?";
        string dataset = "GSOM"; // Global Summary of the Month
        string location = City + ", " + State; // will probaly need its own function to get correct values. will fill FIPS and station
        string startDate = "2022-01-01"; // test duration of one year
        string endDate = "2022-12-01";
        string limit = "100";
        string token = "vNeuShRzmzZIlwUshfxceryeFaBYTntX";
        
        
        // FIPS is state, CITY is city, 

        //string url =
           // "http://www.ncdc.noaa.gov/cdo-web/api/v2/locations?locationcategoryid=CITY&statecode=CA&limit=1000";
        
        //url for gettin glist of cities in alaska using noaa api
        string url =
            "https://www.ncei.noaa.gov/cdo-web/api/v2/locations?locationcategoryid=ST:AK&limit=52";

     
           
        //string url = $"{baseUrl}datasetid={dataset}&datatypeid=TAVG&locationid=FIPS:02&units=standard&startdate={startDate}&enddate={endDate}&limit={limit}";
        
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "C# App"); // required to use api, thanks chatgpt
        client.DefaultRequestHeaders.Add("token", token); 
        
        var response = client.GetAsync(url).Result;
        var content = response.Content.ReadAsStringAsync().Result;
        var json = JsonArray.Parse(content);
    }
    public void GetWeatherInfoOpenWeatherMap() {
        
        string ApiKey = "b18448debbdc3bf7dcb63ff197207e28";
        string GetCityString() {
            var CityString = City + "," + StateCode + "," + CountryCode;
            return CityString;
        }

        // example format{Seward,AK,USA}, TEMP WILL BE IN KELVIN, gets the curent weather
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={GetCityString()}&appid={ApiKey}";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "C# App"); // required to use api, thanks chatgpt
        
        var response = client.GetAsync(url).Result;
        var content = response.Content.ReadAsStringAsync().Result;
        var json = JsonArray.Parse(content);
        
        CurrentWeatherDescription = json["weather"][0]["description"].ToString(); // must be in this format
        CurrentTemp = int.Parse(json["main"]["temp"].ToString());
        CurrentFeelsLike = int.Parse(json["main"]["feels_like"].ToString());
        CurrentHumidity = int.Parse(json["main"]["humidity"].ToString());
        TempMin = int.Parse(json["main"]["temp_min"].ToString());
        TempMax = int.Parse(json["main"]["temp_max"].ToString());

        client.Dispose();
    }
    
    //  using (var client = new HttpClient()) {} // this is a disposable object, so it should be disposed of when done cool
    

}