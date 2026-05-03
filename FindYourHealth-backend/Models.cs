namespace FindYourHealth_backend
{
    public class InsuranceAffiliationsModel
    {
        public string ServiceLevel { get; set; }
        public string ServiceType { get; set; }
        public string Service { get; set; }
        public string ServiceCategory { get; set; }
        public string SubServiceCategory { get; set; }
        public string AgeGroup { get; set; }
        public string InsuranceCompany { get; set; }
        public string InsurancePlan { get; set; }
        public string Company { get; set; }
        public string Location { get; set; }
        public string County { get; set; }
        public string Street { get; set; }
        public string SuiteBuilding { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Religion { get; set; }

        public InsuranceAffiliationsModel(string ServiceLevel, string ServiceType, string Service, string ServiceCategory, string SubServiceCategory, string AgeGroup, string InsuranceCompany, string InsurancePlan, string Company, string Location, string County, string Street, string SuiteBuilding, string City, string State, string Zip, string Phone, string Website, string Religion)
        {
            this.ServiceLevel = ServiceLevel;
            this.ServiceType = ServiceType;
            this.Service = Service;
            this.ServiceCategory = ServiceCategory;
            this.SubServiceCategory = SubServiceCategory;
            this.AgeGroup = AgeGroup;
            this.City = City;
            this.State = State;
            this.Zip = Zip;
            this.Phone = Phone;
            this.InsuranceCompany = InsuranceCompany;
            this.InsurancePlan = InsurancePlan;
            this.Location = Location;
            this.County = County;
            this.Company = Company;
            this.Street = Street;
            this.SuiteBuilding = SuiteBuilding;
            this.Website = Website;
            this.Religion = Religion;
        }

    }
}
