using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;
using static WeatherApp.WeatherInfo;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        private Size originalSize;
        private string APIKey = "29e8e546e5ac4f413e3c9ee688325a59";

        public Form1()
        {
            InitializeComponent();
            originalSize = this.Size; // Save the original size of the form
            HideControls();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the current form
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
          
            getWeather();
            if (string.IsNullOrEmpty(tbCity.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thành phố!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string city = tbCity.Text.Trim(); // Remove any extra whitespace
            if (string.IsNullOrEmpty(city))
            {
                MessageBox.Show("Vui lòng nhập tên thành phố để tìm công ty du lịch!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Clear old items in ListBox before adding new items
            listBox1.Items.Clear();

            // Display information based on city
            if (city.Equals("hanoi", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.Items.Add("Chào mừng đến với Hà Nội!");
                listBox1.Items.Add("A Chau Travel");
                listBox1.Items.Add("Địa chỉ: Số 5, Phố Tràng Tiền, Hà Nội");
                listBox1.Items.Add("Lịch trình: Tour 2: Hà Nội - Ninh Bình - Hạ Long (4 ngày 3 đêm)");
                listBox1.Items.Add("Đánh giá: 4.8");
                listBox1.Items.Add("Website: https://achautravel.com.vn/");
            }
            else if (city.Equals("saigon", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.Items.Add("Chào mừng đến với Thành phố Hồ Chí Minh!");
                listBox1.Items.Add("Saigon Travel");
                listBox1.Items.Add("Địa chỉ: 123 Nguyễn Huệ, Quận 1, TP.HCM");
                listBox1.Items.Add("Lịch trình: Tour 1: TP.HCM - Vũng Tàu - Cần Giờ (3 ngày 2 đêm)");
                listBox1.Items.Add("Đánh giá: 4.9");
                listBox1.Items.Add("Website: https://saigontravel.com.vn/");
            }
            else if (city.Equals("quangngai", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.Items.Add("Chào mừng đến với Quảng Ngãi!");
                listBox1.Items.Add("Quang Ngai Travel");
                listBox1.Items.Add("Địa chỉ: Số 10, Phố Trần Phú, Quảng Ngãi");
                listBox1.Items.Add("Lịch trình: Tour 1: Quảng Ngãi - Bình Sơn - Lý Sơn (3 ngày 2 đêm)");
                listBox1.Items.Add("Đánh giá: 4.7");
                listBox1.Items.Add("Website: https://quangngaitravel.com.vn/");
            }
            else if (city.Equals("danang", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.Items.Add("Chào mừng đến với Đà Nẵng!");
                listBox1.Items.Add("Danang Travel");
                listBox1.Items.Add("Địa chỉ: Số 20, Phố Bạch Đằng, Đà Nẵng");
                listBox1.Items.Add("Lịch trình: Tour 3: Đà Nẵng - Hội An - Huế (4 ngày 3 đêm)");
                listBox1.Items.Add("Đánh giá: 4.9");
                listBox1.Items.Add("Website: https://danangtravel.com.vn/");
            }
            else if (city.Equals("london", StringComparison.OrdinalIgnoreCase))
            {
                listBox1.Items.Add("Chào mừng đến với London!");
                listBox1.Items.Add("London Travel");
                listBox1.Items.Add("Địa chỉ: 123 Oxford Street, London, UK");
                listBox1.Items.Add("Lịch trình: Tour 4: London - Oxford - Cambridge (6 ngày 5 đêm)");
                listBox1.Items.Add("Đánh giá: 4.6");
                listBox1.Items.Add("Website: https://londontravel.com/");
            }
            else
            {
                // Display message for other cities
                listBox1.Items.Add($"Không có công ty du lịch tại {city}");
                listBox1.Items.Add("Chưa có sẵn lịch trình!");
            }
            // Show ListBox only if there are items to display
            listBox1.Visible = listBox1.Items.Count > 0;
        }

        private void lb03_Click(object sender, EventArgs e)
        {
            // Xử lý khi label "Tốc độ gió" được click
            // Bạn có thể để trống nếu không cần xử lý gì
        }

        private void getWeather()
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    string cityName = Uri.EscapeDataString(tbCity.Text);
                    string url = string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&appid={1}&units=metric", cityName, APIKey);

                    var json = web.DownloadString(url);

                    WeatherInfo.Root info = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);

                    if (info != null && info.List != null && info.List.Count > 0)
                    {
                        // Assuming we want to display the first forecast entry
                        var forecast = info.List[0];

                        string iconUrl = "https://openweathermap.org/img/w" +
                            "" +
                            "" +
                            "" +
                            "" +
                            "" +
                            "" +
                            "/" + forecast.Weather[0].Icon + ".png";


           
                        



                        // Translate and display weather information
                        lab_tinhtrang.Text = WeatherTranslator.TranslateMain(forecast.Weather[0].Main);
                        lab_chitiet.Text = WeatherTranslator.TranslateDescription(forecast.Weather[0].Description);

                        ShowControls();

                        // Display temperature in Celsius
                        double tempCelsius = forecast.Main.Temp;
                        lab_nhietdo.Text = $"{tempCelsius.ToString("0.0")} °C";
                        lab_nhietdo.ForeColor = Color.Black;
                      

                        // Display humidity
                       // Display humidity
lab_doam.Text = $"{forecast.Main.Humidity} %";
lab_doam.ForeColor = Color.Black;  // Set text color to blue
// Set background color to white


// Display pressure
lab_apsuat.Text = $"{forecast.Main.Pressure} hPa";
lab_apsuat.ForeColor = Color.Black;



// Display wind gust
lab_giogiat.Text = $"{forecast.Wind.Gust?.ToString("0.00") ?? "N/A"} m/s";
lab_giogiat.ForeColor = Color.Black;



// Display wind speed
lab_tdgio.Text = $"{forecast.Wind.Speed:0.00} m/s";
lab_tdgio.ForeColor = Color.Black;



// Display rainfall
lab_luongmua.Text = $"{forecast.Rain?.Rain1h?.ToString("0.0") ?? "0.0"} mm";
lab_luongmua.ForeColor = Color.Black;


                        GiveHealthAdvice(tempCelsius, forecast.Main.Humidity, forecast.Wind.Speed);

                        // Adjust form size
                        this.Size = originalSize;
                    }
                    else
                    {
                        MessageBox.Show("Weather data is not available or incomplete.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving weather data: " + ex.Message);
            }
        }

        private void HideControls()
        {
            lab_nhietdo.Visible = false;
            lab_luongmua.Visible = false;
            lab_tdgio.Visible = false;
            lab_doam.Visible = false;
            lab_apsuat.Visible = false;
            lab_giogiat.Visible = false;
            label1.Visible = false;
            lb02.Visible = false;
            lab_chitiet.Visible = false;
            lb03.Visible = false;
            lb04.Visible = false;
            lb05.Visible = false;
            lb06.Visible = false;
            lb07.Visible = false;

            lab_tinhtrang.Visible = false;
            lab_giogiat.Visible = false;
            lab_luongmua.Visible = false;
            lab_apsuat.Visible = false;
            lab_doam.Visible = false;
            lab_tdgio.Visible = false;
            lab_nhietdo.Visible = false;

            btn_chitiet01.Visible = false;

         
        }

        private void ShowControls()
        {
            lab_nhietdo.Visible = true;
            lab_luongmua.Visible = true;
            lab_tdgio.Visible = true;
            lab_doam.Visible = true;
            lab_apsuat.Visible = true;
            lab_giogiat.Visible = true;

            lb02.Visible = true;
            lab_chitiet.Visible = false;
            lb03.Visible = true;
            lb04.Visible = true;
            lb05.Visible = true;
            lb06.Visible = true;
            lb07.Visible = true;

            lab_tinhtrang.Visible = true;
            lab_giogiat.Visible = true;
            lab_luongmua.Visible = true;
            lab_apsuat.Visible = true;
            lab_doam.Visible = true;
            lab_tdgio.Visible = true;
            lab_nhietdo.Visible = true;

            btn_chitiet01.Visible = true;


        }

        private void LoadAndResizeImage(string url)
        {
            try
            {
                using (WebClient webClient = new WebClient()) // Tạo một instance mới của WebClient
                {
                    byte[] imageBytes = webClient.DownloadData(url); // Tải dữ liệu hình ảnh từ URL đã chỉ định
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message); // Hiển thị thông báo lỗi nếu có sự cố
            }
        }

        void GiveHealthAdvice(double temperature, int humidity, double windSpeed)
        {
            string advice = "";

            // Khuyến cáo sức khỏe dựa trên nhiệt độ
            if (temperature > 25) // Nhiệt độ trên 30°C
            {
                advice += "Hãy uống đủ nước và tránh ra ngoài quá lâu, đặc biệt vào giữa ngày khi nhiệt độ cao.\n";
            }
            else if (temperature < 10) // Nhiệt độ dưới 10°C
            {
                advice += "Hãy mặc ấm và tránh tiếp xúc lâu ngoài trời khi trời lạnh.\n";
            }
            else
            {
                advice += "Thời tiết này rất phù hợp với việc chơi thể thao và các hoạt động ngoài trời.\n";
            }

            // Khuyến cáo sức khỏe dựa trên độ ẩm
            if (humidity > 80) // Độ ẩm trên 80%
            {
                advice += "Độ ẩm cao có thể khiến bạn cảm thấy khó chịu, hãy tránh các hoạt động thể chất ngoài trời.\n";
            }
            else if (humidity < 30) // Độ ẩm dưới 30%
            {
                advice += "Không khí khô có thể gây khô da và vấn đề hô hấp, hãy uống nước thường xuyên.\n";
            }

            // Khuyến cáo sức khỏe dựa trên tốc độ gió
            if (windSpeed > 10) // Gió mạnh trên 10m/s
            {
                advice += "Gió mạnh có thể gây nguy hiểm, tránh ra ngoài khi gió quá lớn.\n";
            }

            // Hiển thị khuyến cáo sức khỏe
            label1.Text = advice;
        }  // Đảm bảo đóng ngoặc phương thức này
        private Image ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }
        public static class WeatherTranslator
        {
            private static readonly Dictionary<string, string> WeatherMainDescriptions = new Dictionary<string, string>
            {
                { "Thunderstorm", "Dông bão" },
                { "Drizzle", "Mưa phùn" },
                { "Rain", "Mưa" },
                { "Snow", "Tuyết" },
                { "Mist", "Sương mù" },
                { "Smoke", "Khói" },
                { "Haze", "Sương mù" },
                { "Dust", "Bụi" },
                { "Fog", "Sương mù" },
                { "Sand", "Cát" },
                { "Ash", "Tro" },
                { "Squall", "Gió mạnh" },
                { "Tornado", "Lốc xoáy" },
                { "Clear", "Trời quang" },
                { "Clouds", "Mây" }
            };

            private static readonly Dictionary<string, string> WeatherDetailDescriptions = new Dictionary<string, string>
            {
                { "light rain", "Mưa nhẹ" },
                { "moderate rain", "Mưa vừa" },
                { "heavy intensity rain", "Mưa lớn" },
                { "very heavy rain", "Mưa rất lớn" },
                { "extreme rain", "Mưa cực lớn" },
                { "freezing rain", "Mưa đá" },
                { "light intensity shower rain", "Mưa rào nhẹ" },
                { "shower rain", "Mưa rào" },
                { "heavy intensity shower rain", "Mưa rào lớn" },
                { "ragged shower rain", "Mưa rào không đều" },
                { "light snow", "Tuyết nhẹ" },
                { "snow", "Tuyết" },
                { "heavy snow", "Tuyết lớn" },
                { "sleet", "Mưa tuyết" },
                { "light shower sleet", "Mưa tuyết nhẹ" },
                { "shower sleet", "Mưa tuyết" },
                { "light rain and snow", "Mưa và tuyết nhẹ" },
                { "rain and snow", "Mưa và tuyết" },
                { "light shower snow", "Tuyết rơi nhẹ" },
                { "shower snow", "Tuyết rơi" },
                { "heavy shower snow", "Tuyết rơi lớn" },
                { "mist", "Sương mù" },
                { "smoke", "Khói" },
                { "haze", "Sương mù" },
                { "sand/dust whirls", "Bụi cát" },
                { "fog", "Sương mù" },
                { "sand", "Cát" },
                { "dust", "Bụi" },
                { "volcanic ash", "Tro núi lửa" },
                { "squalls", "Gió mạnh" },
                { "tornado", "Lốc xoáy" },
                { "clear sky", "Bầu trời quang đãng" },
                { "few clouds", "Ít mây" },
                { "scattered clouds", "Mây rải rác" },
                { "broken clouds", "Mây đứt đoạn" },
                { "overcast clouds", "Mây bao phủ" }
            };

            public static string TranslateMain(string main)
            {
                if (WeatherMainDescriptions.TryGetValue(main, out string description))
                {
                    return description;
                }
                return "Không xác định";
            }

            public static string TranslateDescription(string description)
            {
                if (WeatherDetailDescriptions.TryGetValue(description, out string translatedDescription))
                {
                    return translatedDescription;
                }
                return "Không xác định";
            }

        }
      

        private void Form1_Load(object sender, EventArgs e)
        {
            HideControls();
            
            this.Size = new Size(781, 500); // Set initial small size
            lab_nhietdo.Location = new Point(370, 130);
            lab_tdgio.Location = new Point(200,190);     // Vị trí cho tốc độ gió
            lab_doam.Location = new Point(200, 230);      // Vị trí cho độ ẩm
            
            lab_apsuat.Location = new Point(270, 270);    // Vị trí cho áp suất
            lab_giogiat.Location = new Point(200, 360);   // Vị trí cho gió giật
            lab_luongmua.Location = new Point(200, 310);  // Vị trí cho lượng mưa

            lab_tinhtrang.Location = new Point(590, 140);   // Vị trí tình trạng

            label1.Visible = true;



        }

        private void tbCity_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add logic to handle text changes in the city name textbox
        }

        private void btn_chitiet01_Click(object sender, EventArgs e)
        {
            string city = tbCity.Text;
            Form2 form2 = new Form2(city);
            form2.Show();

        }

       

        private void lb02_Click(object sender, EventArgs e)
        {

        }

        private void lab_tieude_Click(object sender, EventArgs e)
        {

        }
        private void lab_HealthAdvice1_Click(object sender, EventArgs e)
        {

        }
        private void AdjustUI()
        {
            // Đưa Label lên trên cùng
            label1.BringToFront();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}