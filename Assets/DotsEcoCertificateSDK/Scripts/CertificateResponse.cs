using System;
using System.Collections.Generic;

namespace DotsEcoCertificateSDK
{
    [Serializable]
    public class CertificateResponse
    {
        public string certificate_id;
        public string certificate_url;
        public string certificate_image_url;
        public int app_id;
        public string app_name;
        public string remote_user_id; // Developers need to provide their own remote user id (For example unique user account id)
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

    [Serializable]
    public class Rendering
    {
        public App app;
        public Project project;
        public Allocation allocation;
        public string certificate_header;
        public string greeting;
        public Theme theme;

        public Rendering()
        {
            app = new App();
            certificate_header = "";
            greeting = "";
            theme = new Theme();
        }

        public Rendering(App App, Project Project, Allocation Allocation, string CertificateHeader, string Greeting, Theme Theme)
        {
            app = App;
            project = Project;
            allocation = Allocation;
            certificate_header = CertificateHeader;
            greeting = Greeting;
            theme = Theme;
        }

        public Rendering(Theme Theme)
        {
            theme = Theme;
        }
    }
    
    [Serializable]
    public class Geolocation
    {
        public double lat;
        public double lng;
        
        public Geolocation()
        {
            lat = 0;
            lng = 0;
        }

        public Geolocation(double Lat, double Lng)
        {
            lat = Lat;
            lng = Lng;
        }
    }

    [Serializable]
    public class App
    {
        public string logo_url;

        public App()
        {
            logo_url = "";
        }

        public App(string LogoURL)
        {
            logo_url = LogoURL;
        }
    }

    [Serializable]
    public class Project
    {
        public string image_url;
        public string location_image_url;

        public Project(string ImageURL, string LocationImageURL)
        {
            image_url = ImageURL;
            location_image_url = LocationImageURL;
        }
    }

    [Serializable]
    public class Allocation
    {
        public string image_url;

        public Allocation(string ImageURL)
        {
            image_url = ImageURL;
        }
    }

    [Serializable]
    public class Theme
    {
        public ImpactTypeCategory impact_type_category;
        public CategoryTheme category_theme;
        
        public Theme()
        {
            impact_type_category = new ImpactTypeCategory();
            category_theme = new CategoryTheme();
        }

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
        
        public ImpactTypeCategory()
        {
            icon_url = "";
        }

        public ImpactTypeCategory(string IconURL)
        {
            icon_url = IconURL;
        }
    }

    [Serializable]
    public class CategoryTheme
    {
        public string name;
        public string primary;
        public string secondary;
        public string tertiary;
        public string background;
        public string scratch_image_url;
        
        public CategoryTheme()
        {
            name = "";
            primary = "";
            secondary = "";
            tertiary = "";
            background = "";
            scratch_image_url = "";
        }
        
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

    [Serializable]
    public class ErrorResponse
    {
        public string message;
    }
}