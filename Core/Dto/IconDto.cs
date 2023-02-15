namespace Core.Dto
{
    public class IconDto
    {
    }

    public class IconsListDto
    {
        public long IconId { get; set; }
        public string IconName { get; set; }
        public string Svg { get; set; }
    }

    public class AddIconDto
    {
        public long IconId { get; set; }
        public string IconName { get; set; }
        public string Svg { get; set; }
    }

    public class UpdateIconDto
    {
        public long IconId { get; set; }
        public string IconName { get; set; }
        public string Svg { get; set; }
    }
}
