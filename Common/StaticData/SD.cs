namespace Common.Enums
{
    public static class SD
    {
        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }

        public enum PaymentsStatusEnum
        {
            Done,Incoming
        }

        public enum FiltersEnum
        {
            Distrinct,
            Rating,
            NearMe
        }

        public enum SponsorTypeEnum
        {
            HomePage,
            Profile,
            Search
        }

        public enum CacheKeysEnum
        {
            serviceMainDetails,
            serviceSocialMedia,
            serviceGallery,
            servicePackage,
            PublicCategoriesList,
            ServiceCategoryList,
            CountryDistrictSelect,
            AllowedFilters,
            IconsList,
            IconsListSelect,
        }
    }
}
