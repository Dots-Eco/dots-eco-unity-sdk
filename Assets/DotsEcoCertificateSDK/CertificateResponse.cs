[System.Serializable]
public class CertificateResponse
{
    public string certificate_id;
    public string certificate_url;
    public string certificate_image_url;
    public int app_id;
    public string app_name;
    public string remote_user_id;
    public string name_on_certificate;
    public string certificate_design;
    public string certificate_info;
    public int impact_qty;
    public int impact_type_id;
    public string impact_type_name;
    public string impact_status;
    public string created_timestamp;
    public int allocation_id;
    public string country;
    public Geolocation[] geolocation;
}

[System.Serializable]
public class Geolocation
{
    public double lat;
    public double lng;
}

[System.Serializable]
public class ErrorResponse
{
    public string message;
}
