namespace PustokApp.Areas.PustokArea.ViewModels
{
    public class UpdateSlideVM
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile? Photo { get; set; }
    }
}
