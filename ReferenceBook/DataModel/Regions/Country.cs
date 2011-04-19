using System.Collections.Generic;

namespace ReferenceBook
{
    public class Microdistrict : IEntity
    {
        public Microdistrict(string microdistrictName)
        {
            this.Name = microdistrictName;
        }

        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Borough : IEntity
    {
        public Borough(string boroughName)
        {
            this.Name = boroughName;
        }

        public int ID { get; set; }

        public string Name { get; set; }

        List<Microdistrict> _microdistricts = new List<Microdistrict>();
        public List<Microdistrict> Microdistricts
        {
            get { return _microdistricts; }
        }
    }

    public class Street : IEntity
    {
        public Street(string streetName)
        {
            this.Name = streetName;
        }

        public int ID { get; set; }

        public string Name { get; set; }
    }

    public class City : IEntity
    {
        public City(string cityName)
        {
            this.Name = cityName;
        }

        public int ID { get; set; }

        public string Name { get; set; }

         List<Street> _streets = new List<Street>();
        public List<Street> Streets
        {
            get { return _streets; }
        }

         List<Borough> _boroughs = new List<Borough>();
        public List<Borough> Boroughs
        {
            get { return _boroughs; }
        }
    }

    public class District : IEntity
    {
        public District(string districtName)
        {
            this.Name = districtName;
        }

        public int ID { get; set; }

        public string Name { get; set; }

         List<City> _cities = new List<City>();
        public List<City> Cities
        {
            get { return _cities; }
        }
    }

    public class Region : IEntity
    {
        public Region(string regionName)
        {
            this.Name = regionName;
        }

        public int ID { get; set; }

        public string Name { get; set; }

         List<District> _districts = new List<District>();
        public List<District> Districts
        {
            get { return _districts; }
        }
    }

    public class Country : IEntity
    {
        public Country(string countryName)
        {
            this.Name = countryName;
        }

        public int ID { get; set; }

        public string Name { get; set; }

         List<Region> _regions = new List<Region>();
        public List<Region> Regions
        {
            get { return _regions; }
        }
    }
}