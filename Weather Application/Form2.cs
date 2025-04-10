using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WeatherApp
{
    public partial class Form2 : Form
    {
        private WeatherInfo.Root data;
        private string cityName;
        private const string APIKey = "4359ef1cd11b4c97b0da50cce76d01e7";

        public Form2(string City)
        {
            InitializeComponent();
            this.cityName = City;
        }
//
        private async void Form2_Load(object sender, EventArgs e)
        {
            await prepareForecastToDisplay(cityName);
            displayWeather();
        }

        public async Task prepareForecastToDisplay(string City)
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    string url = string.Format("https://api.openweathermap.org/data/2.5/forecast?q={0}&units=metric&appid={1}", Uri.EscapeDataString(City), APIKey);
                    var json = await web.DownloadStringTaskAsync(url);
                    data = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy dữ liệu thời tiết: {ex.Message}");
            }
        }

        public void displayWeather()
        {
            temperatureLabel1.Location = new Point(50, 266);
            temperatureLabel2.Location = new Point(240, 266);
            temperatureLabel31.Location = new Point(440, 266);



            label5.Location = new Point(609, 269);
            label6.Location = new Point(780, 269);





            temperatureLabel31.Font = new Font(temperatureLabel31.Font.FontFamily, 16, FontStyle.Bold);
            temperatureLabel31.BackColor = Color.Transparent;
            temperatureLabel31.ForeColor = Color.Black;

            if (data != null && data.List != null && data.List.Count > 0)
            {
                var forecasts = data.List
                    .GroupBy(x => DateTime.Parse(x.DtTxt).Date)
                    .Select(g => g.First())
                    .GroupBy(x => DateTime.Parse(x.DtTxt).Date)
                    .Select(g => g.First())
                    .Take(5) // Lấy 5 ngày đầu tiên

                    .ToList();

                DisplayForecast(forecasts[0], dateLabel1, temperatureLabel1, weatherIconBox1, 200, 150);
                if (forecasts.Count > 1) DisplayForecast(forecasts[1], dateLabel2, temperatureLabel2, weatherIconBox2, 200, 250);
                if (forecasts.Count > 2) DisplayForecast(forecasts[2], dateLabel3, temperatureLabel31, weatherIconBox3, 200, 350);
                if (forecasts.Count > 3) DisplayForecast(forecasts[3], dateLabel4, label5, weatherIconBox4, 200, 450);
                if (forecasts.Count > 4) DisplayForecast(forecasts[4], dateLabel5, label6, weatherIconBox5, 200, 550);

                // ✅ Lấy danh sách nhiệt độ để đưa vào lời khuyên
                List<double> temps = forecasts.Select(f => f.Main.Temp).ToList();
                string advice = GenerateAdvice(temps);

                // ✅ Gán lời khuyên vào labelAdvice
                labelAdvice.Text = advice;
                labelAdvice.Visible = true;
            }
            else
            {
                MessageBox.Show("Dữ liệu thời tiết không khả dụng.");
            }
        }

        private void DisplayForecast(WeatherInfo.Forecast forecast, Label dateLabel, Label tempLabel, PictureBox iconBox, int x, int y)
        {
            dateLabel.Text = DateTime.Parse(forecast.DtTxt).ToString("dd/MM/yyyy");
            tempLabel.Text = forecast.Main.Temp.ToString("F1") + " °C";
            tempLabel.ForeColor = Color.Black;
            tempLabel.Font = new Font(tempLabel.Font, FontStyle.Bold);

            string img = "http://openweathermap.org/img/w/" + forecast.Weather[0].Icon + ".png";
            iconBox.Size = new Size(150, 150);
            iconBox.SizeMode = PictureBoxSizeMode.StretchImage;
            iconBox.Load(img);

            dateLabel.Visible = tempLabel.Visible = iconBox.Visible = true;
        }

        // ✅ Hàm sinh lời khuyên
        private string GenerateAdvice(List<double> temps)
        {
            double avgTemp = temps.Average();

            if (avgTemp < 15)
                return "🌬️ Trời lạnh, bạn nên mặc áo ấm và giữ ấm cơ thể.";
            else if (avgTemp >= 15 && avgTemp < 25)
                return "🌤️ Thời tiết khá dễ chịu, bạn có thể ra ngoài thoải mái.";
            else
                return "🔥 Trời nóng, hãy uống nhiều nước và tránh ở ngoài quá lâu.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void temperatureL_Click(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateLabel2_Click(object sender, EventArgs e)
        {

        }
    }
}
