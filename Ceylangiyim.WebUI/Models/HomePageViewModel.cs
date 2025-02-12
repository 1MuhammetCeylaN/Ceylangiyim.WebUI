using Ceylangiyim.Core.Entities;

namespace Ceylangiyim.WebUI.Models
{
    public class HomePageViewModel
    {
        public List<Slider>? Sliders { get; set; }
        public List<Product>? Products { get; set; }
        public List<News>? News { get; set; }
        public List<Category>? Categories { get; set; }

    }
}
