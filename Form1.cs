using HtmlAgilityPack;

namespace HepsiBuradaScraping
{
    public partial class Form1 : Form
    {
        public static readonly string TurboAz = "turbo.az";
        MyDbContext context = new MyDbContext();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var url = "https://turbo.az/autos?q%5Bsort%5D=&q%5Bmake%5D%5B%5D=3&q%5Bmodel%5D%5B%5D=&q%5Bmodel%5D%5B%5D=35&q%5Bused%5D=&q%5Bregion%5D%5B%5D=&q%5Bprice_from%5D=&q%5Bprice_to%5D=&q%5Bcurrency%5D=azn&q%5Bloan%5D=0&q%5Bbarter%5D=0&q%5Bcategory%5D%5B%5D=&q%5Byear_from%5D=&q%5Byear_to%5D=&q%5Bcolor%5D%5B%5D=&q%5Bfuel_type%5D%5B%5D=&q%5Bgear%5D%5B%5D=&q%5Btransmission%5D%5B%5D=&q%5Bengine_volume_from%5D=&q%5Bengine_volume_to%5D=&q%5Bpower_from%5D=&q%5Bpower_to%5D=&q%5Bmileage_from%5D=&q%5Bmileage_to%5D=&q%5Bonly_shops%5D=&q%5Bprior_owners_count%5D%5B%5D=&q%5Bseats_count%5D%5B%5D=&q%5Bmarket%5D%5B%5D=&q%5Bcrashed%5D=1&q%5Bpainted%5D=1&q%5Bfor_spare_parts%5D=0";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var containers = doc.DocumentNode.Descendants("div")
                                             .Where(x => x.HasClass("products-container"))
                                             .ToList()[0]
                                             .Descendants("div")
                                             .Where(x=>x.HasClass("tz-container"))
                                             .ToList();
            var latestContainer = containers[2];
            var productsContainer = latestContainer.Descendants("div").Where(x => x.HasClass("products")).ToList()[0];
            var products = productsContainer.Descendants("div").Where(x => x.HasClass("products-i")).ToList();


            foreach (var product in products)
            {
                var productLink = TurboAz + product.Descendants("a").ToList()[0].GetAttributes().ToList()[2].Value;
                var productBottomSection = product.Descendants("div").Where(x => x.HasClass("products-i__bottom")).ToList()[0];
                var productPrice = productBottomSection.Descendants("div")
                                                       .Where(x => x.HasClass("products-i__price"))
                                                       .ToList()[0]
                                                       .Descendants("div")
                                                       .Where(x => x.HasClass("product-price"))
                                                       .ToList()[0]
                                                       .InnerText
                                                       .Trim()
                                                       ;

                var productBrand = productBottomSection.Descendants("div")
                                                       .Where(x => x.HasClass("products-i__name"))
                                                       .ToList()[0]
                                                       .InnerText
                                                       .Split(' ')[0];

                var productModel = productBottomSection.Descendants("div")
                                                      .Where(x => x.HasClass("products-i__name"))
                                                      .ToList()[0]
                                                      .InnerText
                                                      .Split(' ')[1];

                var productYear = productBottomSection.Descendants("div")
                                                      .Where(x => x.HasClass("products-i__attributes"))
                                                      .ToList()[0]
                                                      .InnerText
                                                      .Split(", ")[0];

                var productEngine = productBottomSection.Descendants("div")
                                                      .Where(x => x.HasClass("products-i__attributes"))
                                                      .ToList()[0]
                                                      .InnerText
                                                      .Split(", ")[1];

                var productKilometer = productBottomSection.Descendants("div")
                                                      .Where(x => x.HasClass("products-i__attributes"))
                                                      .ToList()[0]
                                                      .InnerText
                                                      .Split(", ")[2];

                var carModel = new Car()
                {
                    Brand = productBrand,
                    CarLink = productLink,
                    EngineCapacity = productEngine,
                    Kilometer = productKilometer,
                    Model = productModel,
                    Price = productPrice,
                    ProductionYear = productYear
                };

                context.Cars.Add(carModel);


            }

            context.SaveChanges();

            MessageBox.Show($"{products.Count} cars are added to database");

        }
    }
}