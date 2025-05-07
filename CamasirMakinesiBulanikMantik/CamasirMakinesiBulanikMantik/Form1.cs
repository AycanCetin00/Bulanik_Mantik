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

            // Varsay�lan de�erleri ayarla
            TxtHassasl�k.Text = "6,90";
            txtMiktar.Text = "3,30";
            txtKirlilik.Text = "6,90";

            // Grafikleri haz�rla
            InitializeCharts();


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // DataGridView genel ayarlar�
            dgvRules.GridColor = Color.Black;
            dgvRules.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRules.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRules.RowHeadersVisible = false;

            // En ba�ta tabloyu doldur (iste�e ba�l�)
            var emptyRules = new List<Tuple<FuzzyRule, double>>();
            PopulateRulesTable(emptyRules);
            InitializeAte�lemeChart();

        }

        private void InitializeCharts()
        {
            // Giri� grafikleri i�in ayarlar
            ConfigureChart(chart1, "Hassasl�k �yelik Fonksiyonlar�", 0, 10);
            ConfigureChart(chart2, "Miktar �yelik Fonksiyonlar�", 0, 10);
            ConfigureChart(chart3, "Kirlilik �yelik Fonksiyonlar�", 0, 10);

            // ��k�� grafikleri i�in ayarlar
            ConfigureChart(chart4, "D�n�� H�z� �yelik Fonksiyonlar�", 0, 10);
            ConfigureChart(chart5, "S�re �yelik Fonksiyonlar�", 0, 100);
            ConfigureChart(chart6, "Deterjan Miktar� �yelik Fonksiyonlar�", 0, 300);
        }

        private void InitializeAte�lemeChart()
        {
            ConfigureChart(chart7, "Ate�leme De�erleri", 1, 20); // 1-20 aras� kurallar i�in ayarlad�m
            chart7.ChartAreas[0].AxisX.Interval = 1; // Her kural numaras� 1 ad�m
        }

        private void ConfigureChart(Chart chart, string title, double minX, double maxX)
        {
            chart.Titles.Add(title);
            chart.ChartAreas[0].AxisX.Minimum = minX;
            chart.ChartAreas[0].AxisX.Maximum = maxX;
            chart.ChartAreas[0].AxisX.Title = "De�er";
            chart.ChartAreas[0].AxisY.Title = "�yelik Derecesi";
            chart.ChartAreas[0].AxisY.Maximum = 1.1;
            chart.ChartAreas[0].AxisY.Minimum = 0;
            chart.Series.Clear();
        }

      


        private void textDeterjanMiktar�_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnHesapla_Click(object sender, EventArgs e)
        {
            try
            {
                // Giri� de�erlerini al
                double hassaslik = double.Parse(TxtHassasl�k.Text);
                double miktar = double.Parse(txtMiktar.Text);
                double kirlilik = double.Parse(txtKirlilik.Text);

                // Ate�lenen kurallar� hesapla
                var firedRules = fuzzySystem.EvaluateRules(hassaslik, miktar, kirlilik);
                PopulateRulesTable(firedRules);

               
                // ��kt�lar� hesapla
                double donusHizi = fuzzySystem.CalculateDonusHizi(hassaslik, miktar, kirlilik);
                double sure = fuzzySystem.CalculateSure(hassaslik, miktar, kirlilik);
                double deterjanMiktari = fuzzySystem.CalculateDeterjanMiktari(hassaslik, miktar, kirlilik);

                // ��kt�lar� ekrana yazd�r
                textD�n��H�z�.Text = donusHizi.ToString("F2");
                txtS�re.Text = sure.ToString("F2");
                textDeterjanMiktar�.Text = deterjanMiktari.ToString("F2");

                // A��rl�kl� ortalama y�ntemi ile hesapla
                double donusHiziWA = CalculateWeightedAverage(firedRules, "DonusHizi");
                double sureWA = CalculateWeightedAverage(firedRules, "Sure");
                double deterjanMiktariWA = CalculateWeightedAverage(firedRules, "DeterjanMiktari");

                // Label'lara hesaplanan de�erleri ata
                lblD�n�sHizi.Text = $"A��rl�kl� Ort: {donusHiziWA:F2}";
                lblSure.Text = $"A��rl�kl� Ort: {sureWA:F2}";
                lblDeterjanMiktari.Text = $"A��rl�kl� Ort: {deterjanMiktariWA:F2}";

                // Ate�leme grafi�ini g�ncelle
                UpdateAte�lemeChart(firedRules);


                // Giri� grafiklerini g�ncelle
                UpdateInputChart(chart1, fuzzySystem.Hassaslik, hassaslik);
                UpdateInputChart(chart2, fuzzySystem.Miktar, miktar);
                UpdateInputChart(chart3, fuzzySystem.Kirlilik, kirlilik);

                // ��k�� grafikleri g�ncelle
                UpdateOutputChart(chart4, fuzzySystem.DonusHizi, donusHizi);
                UpdateOutputChart(chart5, fuzzySystem.Sure, sure);
                UpdateOutputChart(chart6, fuzzySystem.DeterjanMiktari, deterjanMiktari);
            }
            catch (FormatException)
            {
                MessageBox.Show("L�tfen t�m giri�leri do�ru formatta giriniz (�rne�in 5.0).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void PopulateRulesTable(List<Tuple<FuzzyRule, double>> firedRules)
        {
            dgvRules.Rows.Clear();
            dgvRules.Columns.Clear();

            dgvRules.Columns.Add("No", "No");
            dgvRules.Columns.Add("Hassaslik", "Hassasl�k");
            dgvRules.Columns.Add("Miktar", "Miktar");
            dgvRules.Columns.Add("Kirlilik", "Kirlilik");
            dgvRules.Columns.Add("DonusHizi", "D�n�� H�z�");
            dgvRules.Columns.Add("Sure", "S�re");
            dgvRules.Columns.Add("Deterjan", "Deterjan");
            dgvRules.Columns.Add("Ate�lemeG�c�", "Ate�leme G�c�"); // Son s�tun olarak ate�leme g�c�

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
                    firingStrength.ToString("F2") // Formatlanm�� ate�leme g�c� de�eri

                    );

                // Ate�lenen kurallar� mavi olarak i�aretle
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

            // Kolon geni�liklerini ayarla
            dgvRules.Columns[0].Width = 40; // No kolonu dar olsun
            dgvRules.Columns["Ate�lemeG�c�"].Width = 80; // Ate�leme g�c� kolonunun geni�li�i
            dgvRules.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
           // UpdateFiredRulesChart(firedRules);

        }

        private void UpdateAte�lemeChart(List<Tuple<FuzzyRule, double>> firedRules)
        {
            // �nce eski serileri temizle
            chart7.Series.Clear();

            // Yeni seri olu�tur
            var series = new Series
            {
                Name = "Ate�leme De�eri",
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = Color.MediumVioletRed, // Rengi de�i�tirebilirsin
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                MarkerColor = Color.Red
            };

            // X ekseni: Kural numaras�
            // Y ekseni: Ate�leme g�c�
            for (int i = 0; i < firedRules.Count; i++)
            {
                double ateslemeDegeri = firedRules[i].Item2;
                series.Points.AddXY(i + 1, ateslemeDegeri); // (Kural No, Ate�leme De�eri)
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

            // �yelik fonksiyonlar�n� �iz
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

            // Girdi de�eri i�aretle
            Series crispSeries = new Series("Input Value");
            crispSeries.ChartType = SeriesChartType.Point;
            crispSeries.Color = Color.Red;

            // Girdi de�eri i�in �yelik derecelerini i�aretle
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

            // ��k�� de�eri i�aretle
            Series crispSeries = new Series("Crisp Value");
            crispSeries.ChartType = SeriesChartType.Point;
            crispSeries.Color = Color.Red;
            crispSeries.Points.AddXY(crispValue, 0);
            crispSeries.Points.AddXY(crispValue, 1);
            chart.Series.Add(crispSeries);
        }



       // A��rl�kl� ortalama y�ntemi
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

                // ��k�� tipine g�re merkez de�erini al
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

                // A��rl�kl� toplam� hesapla
                numerator += centerValue * firingStrength;
                denominator += firingStrength;
            }

            // E�er hi�bir kural ate�lenmediyse
            if (denominator == 0)
                return 0;

            // A��rl�kl� ortalamay� d�nd�r
            return numerator / denominator;
        }

        // �yelik fonksiyonunun merkez de�erini d�nd�r�r
        private double GetCenterValueForOutput(string outputName, FuzzySet fuzzySet)
        {
            // Burada her ��k�� �yelik fonksiyonu i�in merkez de�eri tan�mlamal�s�n�z
            // Bu de�erler belgenizde verilen �yelik fonksiyonlar�n�n merkez de�erleridir

            // D�n�� h�z� i�in
            if (fuzzySet == fuzzySystem.DonusHizi)
            {
                switch (outputName.ToLower())
                {
                    case "hassas": return 0.5;
                    case "normal_hassas": return 2.75;
                    case "orta": return 5.0;
                    case "normal_g��l�": return 7.25;
                    case "g��l�": return 9.5;
                    default: return 5.0; // varsay�lan orta de�er
                }
            }
            // S�re i�in
            else if (fuzzySet == fuzzySystem.Sure)
            {
                switch (outputName.ToLower())
                {
                    case "k�sa": return 22.3;
                    case "normal_k�sa": return 39.9;
                    case "orta": return 57.5;
                    case "normal_uzun": return 75.1;
                    case "uzun": return 92.7;
                    default: return 57.5; // varsay�lan orta de�er
                }
            }
            // Deterjan miktar� i�in
            else if (fuzzySet == fuzzySystem.DeterjanMiktari)
            {
                switch (outputName.ToLower())
                {
                    case "�ok_az": return 20.0;
                    case "az": return 85.0;
                    case "orta": return 150.0;
                    case "fazla": return 215.0;
                    case "�ok_fazla": return 280.0;
                    default: return 150.0; // varsay�lan orta de�er
                }
            }

            return 0;
        }


    }
}
