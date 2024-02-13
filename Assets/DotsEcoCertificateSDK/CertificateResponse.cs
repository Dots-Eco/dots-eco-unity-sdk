using System.Collections.Generic;

namespace DotsEcoCertificateSDK
{
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
        
        public List<Geolocation> geolocation;
        public Rendering rendering;
    }

    [System.Serializable]
    public class Rendering
    {
        public App app;
        public string impact_title;
        public Theme theme;

        public Rendering(App App, string ImpactTitle, Theme Theme)
        {
            app = App;
            impact_title = ImpactTitle;
            theme = Theme;
        }

        public Rendering(Theme Theme)
        {
            theme = Theme;
        }
    }
    
    [System.Serializable]
    public class Geolocation
    {
        public double lat;
        public double lng;

        public Geolocation(double Lat, double Lng)
        {
            lat = Lat;
            lng = Lng;
        }
    }

    [System.Serializable]
    public class App
    {
        public string logo_url;

        public App(string LogoURL)
        {
            logo_url = LogoURL;
        }
    }
    
    [System.Serializable]
    public class Theme
    {
        public ImpactTypeCategory impact_type_category;
        public CategoryTheme category_theme;

        public Theme(ImpactTypeCategory ImpactTypeCategory, CategoryTheme CategoryTheme)
        {
            impact_type_category = ImpactTypeCategory;
            category_theme = CategoryTheme;
        }

        public Theme(CategoryTheme CategoryTheme)
        {
            category_theme = CategoryTheme;
        }
    }
    
    [System.Serializable]
    public class ImpactTypeCategory
    {
        public string icon_url;

        public ImpactTypeCategory(string IconURL)
        {
            icon_url = IconURL;
        }
    }

    [System.Serializable]
    public class CategoryTheme
    {
        public string name;
        public string primary;
        public string secondary;
        public string tertiary;
        public string background;
        public string scratch_image_url;
        
        public CategoryTheme(string Name, string Primary, string Secondary, string Tertiary, string Background, string ScratchImageURL)
        {
            name = Name;
            primary = Primary;
            secondary = Secondary;
            tertiary = Tertiary;
            background = Background;
            scratch_image_url = ScratchImageURL;
        }
    }

    [System.Serializable]
    public class ErrorResponse
    {
        public string message;
    }
}