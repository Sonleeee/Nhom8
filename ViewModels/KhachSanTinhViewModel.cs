

namespace Nhom8.ViewModels
{

    public class KhachSanTinhViewModel
    {
        public IQueryable<KhachSanS> KhachSans { get; set; }
        public List<TinhS> Tinhs { get; set; }
    }

    public class KhachSanS
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public double Star { get; set; }
        public string Tinh { get; set; }
        // Other properties...
    }

    public class TinhS
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Other properties...
    }
}
