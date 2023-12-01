namespace WebApplication1.Models
{
    public class UploadModel
    {
        public IFormFile Image { get; set; }
        public int Id { get; set; }
        public String Name { get; set; }
        public string Description { get; set; }
    }
}
