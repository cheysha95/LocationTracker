namespace ConsoleApp2; 

public class NOAALink {

    // https://www.ncdc.noaa.gov/cdo-web/webservices/v2
    
    // gettin gthis to work is a pain, come back later

    public Dictionary<string, string> Endpoints = new Dictionary<string, string>() {
        {"datasets", "datasets"},
        {"datacategories","datacategories"},
        {"datatypes","datatypes"},
        {"locations","locations"},
        {"locationcategories","locationcategories"},
        {"stations","stations"},
        {"data","data"}
    };
    

    private string BaseUrl = "https://www.ncei.noaa.gov/cdo-web/api/v2/";

    public string Link() {
        return BaseUrl;
    }
    
    public NOAALink() {
        
    }

    

}