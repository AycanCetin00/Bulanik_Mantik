using BulanikCamasirMakinesi;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CamasirMakinesiBulanikMantik

{


    public partial class Form1 : Form
    {
        private FuzzyInferenceSystem fuzzySystem;


        public Form1()
        {
            InitializeComponent();
            fuzzySystem = new FuzzyInferenceSystem();

            // Varsayýlan deðerleri ayarla
            TxtHassaslýk.Text = "6,90";
            txtMiktar.Text = "3,30";
            txtKirlilik.Text = "6,90";

            // Grafikleri hazýrla
            InitializeCharts();


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // DataGridView genel ayarlarý
            dgvRules.GridColor = Color.Black;
            dgvRules.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRules.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRules.RowHeadersVisible = false;

            // En baþta tabloyu doldur (isteðe baðlý)
            var emptyRules = new List<Tuple<FuzzyRule, double>>();
            PopulateRulesTable(emptyRules);
            InitializeAteþlemeChart();

        }

        private void InitializeCharts()
        {
            // Giriþ grafikleri için ayarlar
            ConfigureChart(chart1, "Hassaslýk Üyelik Fonksiyonlarý", 0, 10);
            ConfigureChart(chart2, "Miktar Üyelik Fonksiyonlarý", 0, 10);
            ConfigureChart(chart3, "Kirlilik Üyelik Fonksiyonlarý", 0, 10);

            // Çýkýþ grafikleri için ayarlar
            ConfigureChart(chart4, "Dönüþ Hýzý Üyelik Fonksiyonlarý", 0, 10);
            ConfigureChart(chart5, "Süre Üyelik Fonksiyonlarý", 0, 100);
            ConfigureChart(chart6, "Deterjan Miktarý Üyelik Fonksiyonlarý", 0, 300);
        }

        private void InitializeAteþlemeChart()
        {
            ConfigureChart(chart7, "Ateþleme Deðerleri", 1, 20); // 1-20 arasý kurallar için ayarladým
            chart7.ChartAreas[0].AxisX.Interval = 1; // Her kural numarasý 1 adým
        }

        private void ConfigureChart(Chart chart, string title, double minX, double maxX)
        {
            chart.Titles.Add(title);
            chart.ChartAreas[0].AxisX.Minimum = minX;
            chart.ChartAreas[0].AxisX.Maximum = maxX;
            chart.ChartAreas[0].AxisX.Title = "Deðer";
            chart.ChartAreas[0].AxisY.Title = "Üyelik Derecesi";
            chart.ChartAreas[0].AxisY.Maximum = 1.1;
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.Series.Clear();
        }

      


        private void textDeterjanMiktarý_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            try
            {
                // Giriþ deðerlerini al
                double hassaslik = double.Parse(TxtHassaslýk.Text);
                double miktar = double.Parse(txtMiktar.Text);
                double kirlilik = double.Parse(txtKirlilik.Text);

                // Ateþlenen kurallarý hesapla
                var firedRules = fuzzySystem.EvaluateRules(hassaslik, miktar, kirlilik);
                PopulateRulesTable(firedRules);

               
                // Çýktýlarý hesapla
                double donusHizi = fuzzySystem.CalculateDonusHizi(hassaslik, miktar, kirlilik);
                double sure = fuzzySystem.CalculateSure(hassaslik, miktar, kirlilik);
                double deterjanMiktari = fuzzySystem.CalculateDeterjanMiktari(hassaslik, miktar, kirlilik);

                // Çýktýlarý ekrana yazdýr
                textDönüþHýzý.Text = donusHizi.ToString("F2");
                txtSüre.Text = sure.ToString("F2");
                textDeterjanMiktarý.Text = deterjanMiktari.ToString("F2");

                // Aðýrlýklý ortalama yöntemi ile hesapla
                double donusHiziWA = CalculateWeightedAverage(firedRules, "DonusHizi");
                double sureWA = CalculateWeightedAverage(firedRules, "Sure");
                double deterjanMiktariWA = CalculateWeightedAverage(firedRules, "DeterjanMiktari");

                // Label'lara hesaplanan deðerleri ata
                lblDönüsHizi.Text = $"Aðýrlýklý Ort: {donusHiziWA:F2}";
                lblSure.Text = $"Aðýrlýklý Ort: {sureWA:F2}";
                lblDeterjanMiktari.Text = $"Aðýrlýklý Ort: {deterjanMiktariWA:F2}";

                // Ateþleme grafiðini güncelle
                UpdateAteþlemeChart(firedRules);


                // Giriþ grafiklerini güncelle
                UpdateInputChart(chart1, fuzzySystem.Hassaslik, hassaslik);
                UpdateInputChart(chart2, fuzzySystem.Miktar, miktar);
                UpdateInputChart(chart3, fuzzySystem.Kirlilik, kirlilik);

                // Çýkýþ grafikleri güncelle
                UpdateOutputChart(chart4, fuzzySystem.DonusHizi, donusHizi);
                UpdateOutputChart(chart5, fuzzySystem.Sure, sure);
                UpdateOutputChart(chart6, fuzzySystem.DeterjanMiktari, deterjanMiktari);
            }
            catch (FormatException)
            {
                MessageBox.Show("Lütfen tüm giriþleri doðru formatta giriniz (örneðin 5.0).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PopulateRulesTable(List<Tuple<FuzzyRule, double>> firedRules)
        {
            dgvRules.Rows.Clear();
            dgvRules.Columns.Clear();

            dgvRules.Columns.Add("No", "No");
            dgvRules.Columns.Add("Hassaslik", "Hassaslýk");
            dgvRules.Columns.Add("Miktar", "Miktar");
            dgvRules.Columns.Add("Kirlilik", "Kirlilik");
            dgvRules.Columns.Add("DonusHizi", "Dönüþ Hýzý");
            dgvRules.Columns.Add("Sure", "Süre");
            dgvRules.Columns.Add("Deterjan", "Deterjan");
            dgvRules.Columns.Add("AteþlemeGücü", "Ateþleme Gücü"); // Son sütun olarak ateþleme gücü

            foreach (var rule in fuzzySystem.Rules)
            {
                var matchingRule = firedRules.Find(r => r.Item1.RuleNumber == rule.RuleNumber);
                bool isRuleFired = matchingRule != null && matchingRule.Item2 > 0;
                double firingStrength = isRuleFired ? matchingRule.Item2 : 0.0;

                int rowIndex = dgvRules.Rows.Add(
                    rule.RuleNumber,
                    rule.HassaslikCondition,
                    rule.MiktarCondition,
                    rule.KirlilikCondition,
                    rule.DonusHiziOutput,
                    rule.SureOutput,
                    rule.DeterjanOutput,
                    firingStrength.ToString("F2") // Formatlanmýþ ateþleme gücü deðeri

                    );

                // Ateþlenen kurallarý mavi olarak iþaretle
                if (isRuleFired)
                {
                    dgvRules.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                }
            }

            dgvRules.EnableHeadersVisualStyles = false;
            dgvRules.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvRules.DefaultCellStyle.SelectionBackColor = dgvRules.DefaultCellStyle.BackColor;
            dgvRules.DefaultCellStyle.SelectionForeColor = dgvRules.DefaultCellStyle.ForeColor;
            dgvRules.AllowUserToAddRows = false;
            dgvRules.ReadOnly = true;
            dgvRules.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            // Kolon geniþliklerini ayarla
            dgvRules.Columns[0].Width = 40; // No kolonu dar olsun
            dgvRules.Columns["AteþlemeGücü"].Width = 80; // Ateþleme gücü kolonunun geniþliði
            dgvRules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
           // UpdateFiredRulesChart(firedRules);

        }

        private void UpdateAteþlemeChart(List<Tuple<FuzzyRule, double>> firedRules)
        {
            // Önce eski serileri temizle
            chart7.Series.Clear();

            // Yeni seri oluþtur
            var series = new Series
            {
                Name = "Ateþleme Deðeri",
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.MediumVioletRed, // Rengi deðiþtirebilirsin
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                MarkerColor = Color.Red
            };

            // X ekseni: Kural numarasý
            // Y ekseni: Ateþleme gücü
            for (int i = 0; i < firedRules.Count; i++)
            {
                double ateslemeDegeri = firedRules[i].Item2;
                series.Points.AddXY(i + 1, ateslemeDegeri); // (Kural No, Ateþleme Deðeri)
            }

            // Seriyi ekle
            chart7.Series.Add(series);
        }


        private void UpdateInputChart(Chart chart, FuzzySet fuzzySet, double inputValue)
        {
            chart.Series.Clear();
            chart.ChartAreas[0].AxisX.Minimum = fuzzySet.Min;
            chart.ChartAreas[0].AxisX.Maximum = fuzzySet.Max;
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.ChartAreas[0].AxisY.Maximum = 1;

            // Üyelik fonksiyonlarýný çiz
            foreach (var mf in fuzzySet.MembershipFunctions)
            {
                Series series = new Series(mf.Name);
                series.ChartType = SeriesChartType.Line;

                var points = mf.GetGraphPoints(fuzzySet.Min, fuzzySet.Max);
                foreach (var point in points)
                {
                    series.Points.AddXY(point.Item1, point.Item2);
                }

                chart.Series.Add(series);
            }

            // Girdi deðeri iþaretle
            Series crispSeries = new Series("Input Value");
            crispSeries.ChartType = SeriesChartType.Point;
            crispSeries.Color = Color.Red;

            // Girdi deðeri için üyelik derecelerini iþaretle
            foreach (var mf in fuzzySet.MembershipFunctions)
            {
                double membership = mf.CalculateMembership(inputValue);
                crispSeries.Points.AddXY(inputValue, membership);
            }

            chart.Series.Add(crispSeries);
        }
        private void UpdateOutputChart(Chart chart, FuzzySet fuzzySet, double crispValue)
        {
            chart.Series.Clear();
            chart.ChartAreas[0].AxisX.Minimum = fuzzySet.Min;
            chart.ChartAreas[0].AxisX.Maximum = fuzzySet.Max;
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.ChartAreas[0].AxisY.Maximum = 1;

            foreach (var mf in fuzzySet.MembershipFunctions)
            {
                Series series = new Series(mf.Name);
                series.ChartType = SeriesChartType.Line;

                var points = mf.GetGraphPoints(fuzzySet.Min, fuzzySet.Max);
                foreach (var point in points)
                {
                    series.Points.AddXY(point.Item1, point.Item2);
                }

                chart.Series.Add(series);
            }

            // Çýkýþ deðeri iþaretle
            Series crispSeries = new Series("Crisp Value");
            crispSeries.ChartType = SeriesChartType.Point;
            crispSeries.Color = Color.Red;
            crispSeries.Points.AddXY(crispValue, 0);
            crispSeries.Points.AddXY(crispValue, 1);
            chart.Series.Add(crispSeries);
        }



       // Aðýrlýklý ortalama yöntemi
private double CalculateWeightedAverage(List<Tuple<FuzzyRule, double>> firedRules, string outputType)
        {
            double numerator = 0;
            double denominator = 0;

            foreach (var ruleTuple in firedRules)
            {
                FuzzyRule rule = ruleTuple.Item1;
                double firingStrength = ruleTuple.Item2;

                if (firingStrength <= 0)
                    continue;

                double centerValue = 0;

                // Çýkýþ tipine göre merkez deðerini al
                if (outputType == "DonusHizi")
                {
                    centerValue = GetCenterValueForOutput(rule.DonusHiziOutput, fuzzySystem.DonusHizi);
                }
                else if (outputType == "Sure")
                {
                    centerValue = GetCenterValueForOutput(rule.SureOutput, fuzzySystem.Sure);
                }
                else if (outputType == "DeterjanMiktari")
                {
                    centerValue = GetCenterValueForOutput(rule.DeterjanOutput, fuzzySystem.DeterjanMiktari);
                }

                // Aðýrlýklý toplamý hesapla
                numerator += centerValue * firingStrength;
                denominator += firingStrength;
            }

            // Eðer hiçbir kural ateþlenmediyse
            if (denominator == 0)
                return 0;

            // Aðýrlýklý ortalamayý döndür
            return numerator / denominator;
        }

        // Üyelik fonksiyonunun merkez deðerini döndürür
        private double GetCenterValueForOutput(string outputName, FuzzySet fuzzySet)
        {
            // Burada her çýkýþ üyelik fonksiyonu için merkez deðeri tanýmlamalýsýnýz
            // Bu deðerler belgenizde verilen üyelik fonksiyonlarýnýn merkez deðerleridir

            // Dönüþ hýzý için
            if (fuzzySet == fuzzySystem.DonusHizi)
            {
                switch (outputName.ToLower())
                {
                    case "hassas": return 0.5;
                    case "normal_hassas": return 2.75;
                    case "orta": return 5.0;
                    case "normal_güçlü": return 7.25;
                    case "güçlü": return 9.5;
                    default: return 5.0; // varsayýlan orta deðer
                }
            }
            // Süre için
            else if (fuzzySet == fuzzySystem.Sure)
            {
                switch (outputName.ToLower())
                {
                    case "kýsa": return 22.3;
                    case "normal_kýsa": return 39.9;
                    case "orta": return 57.5;
                    case "normal_uzun": return 75.1;
                    case "uzun": return 92.7;
                    default: return 57.5; // varsayýlan orta deðer
                }
            }
            // Deterjan miktarý için
            else if (fuzzySet == fuzzySystem.DeterjanMiktari)
            {
                switch (outputName.ToLower())
                {
                    case "çok_az": return 20.0;
                    case "az": return 85.0;
                    case "orta": return 150.0;
                    case "fazla": return 215.0;
                    case "çok_fazla": return 280.0;
                    default: return 150.0; // varsayýlan orta deðer
                }
            }

            return 0;
        }


    }
}
