using System;
using System.Collections.Generic;
using System.Linq;

namespace BulanikCamasirMakinesi
{
    public class FuzzyInferenceSystem
    {
        // Bulanık kümeler
        public FuzzySet Hassaslik { get; private set; }
        public FuzzySet Miktar { get; private set; }
        public FuzzySet Kirlilik { get; private set; }
        public FuzzySet DonusHizi { get; private set; }
        public FuzzySet Sure { get; private set; }
        public FuzzySet DeterjanMiktari { get; private set; }

        // Bulanık kurallar
        public List<FuzzyRule> Rules { get; private set; }

        public FuzzyInferenceSystem()
        {
            InitializeFuzzySets();
            InitializeRules();
        }

        private void InitializeFuzzySets()
        {
            // Hassaslık bulanık kümesi
            Hassaslik = new FuzzySet("Hassaslık", 0, 10);
            Hassaslik.AddMembershipFunction(new MembershipFunction("sağlam", -4, -1.5, 2, 4));
            Hassaslik.AddMembershipFunction(new MembershipFunction("orta", 3, 5, 7));
            Hassaslik.AddMembershipFunction(new MembershipFunction("hassas", 5.5, 8, 12.5, 14));

            // Miktar bulanık kümesi
            Miktar = new FuzzySet("Miktar", 0, 10);
            Miktar.AddMembershipFunction(new MembershipFunction("küçük", -4, -1.5, 2, 4));
            Miktar.AddMembershipFunction(new MembershipFunction("orta", 3, 5, 7));
            Miktar.AddMembershipFunction(new MembershipFunction("büyük", 5.5, 8, 12.5, 14));

            // Kirlilik bulanık kümesi
            Kirlilik = new FuzzySet("Kirlilik", 0, 10);
            Kirlilik.AddMembershipFunction(new MembershipFunction("küçük", -4.5, -2.5, 2, 4.5));
            Kirlilik.AddMembershipFunction(new MembershipFunction("orta", 3, 5, 7));
            Kirlilik.AddMembershipFunction(new MembershipFunction("büyük", 5.5, 8, 12.5, 15));

            // Dönüş Hızı bulanık kümesi
            DonusHizi = new FuzzySet("Dönüş Hızı", 0, 10);
            DonusHizi.AddMembershipFunction(new MembershipFunction("hassas", -5.8, -2.8, 0.5, 1.5));
            DonusHizi.AddMembershipFunction(new MembershipFunction("normal_hassas", 0.5, 2.75, 5));
            DonusHizi.AddMembershipFunction(new MembershipFunction("orta", 2.75, 5, 7.25));
            DonusHizi.AddMembershipFunction(new MembershipFunction("normal_güçlü", 5, 7.25, 9.5));
            DonusHizi.AddMembershipFunction(new MembershipFunction("güçlü", 8.5, 9.5, 12.8, 15.2));

            // Süre bulanık kümesi
            Sure = new FuzzySet("Süre", 0, 100);
            Sure.AddMembershipFunction(new MembershipFunction("kısa", -46.5, -25.28, 22.3, 39.9));
            Sure.AddMembershipFunction(new MembershipFunction("normal_kısa", 22.3, 39.9, 57.5));
            Sure.AddMembershipFunction(new MembershipFunction("orta", 39.9, 57.5, 75.1));
            Sure.AddMembershipFunction(new MembershipFunction("normal_uzun", 57.5, 75.1, 92.7));
            Sure.AddMembershipFunction(new MembershipFunction("uzun", 75, 92.7, 111.6, 130));

            // Deterjan Miktarı bulanık kümesi
            DeterjanMiktari = new FuzzySet("Deterjan Miktarı", 0, 300);
            DeterjanMiktari.AddMembershipFunction(new MembershipFunction("çok_az", 0, 0, 20, 85));
            DeterjanMiktari.AddMembershipFunction(new MembershipFunction("az", 20, 85, 150));
            DeterjanMiktari.AddMembershipFunction(new MembershipFunction("orta", 85, 150, 215));
            DeterjanMiktari.AddMembershipFunction(new MembershipFunction("fazla", 150, 215, 280));
            DeterjanMiktari.AddMembershipFunction(new MembershipFunction("çok_fazla", 215, 280, 300, 300));
        }

        private void InitializeRules()
        {
            Rules = new List<FuzzyRule>();

            // Dökümanda verilen 27 kural burada tanımlanıyor
            Rules.Add(new FuzzyRule(1, "hassas", "küçük", "küçük", "hassas", "kısa", "çok_az"));
            Rules.Add(new FuzzyRule(2, "hassas", "küçük", "orta", "normal_hassas", "kısa", "az"));
            Rules.Add(new FuzzyRule(3, "hassas", "küçük", "büyük", "orta", "normal_kısa", "orta"));
            Rules.Add(new FuzzyRule(4, "hassas", "orta", "küçük", "hassas", "kısa", "orta"));
            Rules.Add(new FuzzyRule(5, "hassas", "orta", "orta", "normal_hassas", "normal_kısa", "orta"));
            Rules.Add(new FuzzyRule(6, "hassas", "orta", "büyük", "orta", "orta", "fazla"));
            Rules.Add(new FuzzyRule(7, "hassas", "büyük", "küçük", "normal_hassas", "normal_kısa", "orta"));
            Rules.Add(new FuzzyRule(8, "hassas", "büyük", "orta", "normal_hassas", "orta", "fazla"));
            Rules.Add(new FuzzyRule(9, "hassas", "büyük", "büyük", "orta", "normal_uzun", "fazla"));
            Rules.Add(new FuzzyRule(10, "orta", "küçük", "küçük", "normal_hassas", "normal_kısa", "az"));
            Rules.Add(new FuzzyRule(11, "orta", "küçük", "orta", "orta", "kısa", "orta"));
            Rules.Add(new FuzzyRule(12, "orta", "küçük", "büyük", "normal_güçlü", "orta", "fazla"));
            Rules.Add(new FuzzyRule(13, "orta", "orta", "küçük", "normal_hassas", "normal_kısa", "orta"));
            Rules.Add(new FuzzyRule(14, "orta", "orta", "orta", "orta", "orta", "orta"));
            Rules.Add(new FuzzyRule(15, "orta", "orta", "büyük", "hassas", "uzun", "fazla"));
            Rules.Add(new FuzzyRule(16, "orta", "büyük", "küçük", "hassas", "orta", "orta"));
            Rules.Add(new FuzzyRule(17, "orta", "büyük", "orta", "hassas", "normal_uzun", "fazla"));
            Rules.Add(new FuzzyRule(18, "orta", "büyük", "büyük", "hassas", "uzun", "çok_fazla"));
            Rules.Add(new FuzzyRule(19, "sağlam", "küçük", "küçük", "orta", "orta", "az"));
            Rules.Add(new FuzzyRule(20, "sağlam", "küçük", "orta", "normal_güçlü", "orta", "orta"));
            Rules.Add(new FuzzyRule(21, "sağlam", "küçük", "büyük", "güçlü", "normal_uzun", "fazla"));
            Rules.Add(new FuzzyRule(22, "sağlam", "orta", "küçük", "orta", "orta", "orta"));
            Rules.Add(new FuzzyRule(23, "sağlam", "orta", "orta", "normal_güçlü", "normal_uzun", "orta"));
            Rules.Add(new FuzzyRule(24, "sağlam", "orta", "büyük", "güçlü", "orta", "çok_fazla"));
            Rules.Add(new FuzzyRule(25, "sağlam", "büyük", "küçük", "normal_güçlü", "normal_uzun", "fazla"));
            Rules.Add(new FuzzyRule(26, "sağlam", "büyük", "orta", "normal_güçlü", "uzun", "fazla"));
            Rules.Add(new FuzzyRule(27, "sağlam", "büyük", "büyük", "güçlü", "uzun", "çok_fazla"));
        }

        // Verilen giriş değerlerine göre ateşlenen kuralları ve Mamdani değerlerini hesaplar
        public List<Tuple<FuzzyRule, double>> EvaluateRules(double hassaslikValue, double miktarValue, double kirlilikValue)
        {
            List<Tuple<FuzzyRule, double>> firedRules = new List<Tuple<FuzzyRule, double>>();

            // Her kural için ateşleme gücünü hesapla
            foreach (var rule in Rules)
            {
                double hassaslikDegree = Hassaslik.GetMembershipDegree(rule.HassaslikCondition, hassaslikValue);
                double miktarDegree = Miktar.GetMembershipDegree(rule.MiktarCondition, miktarValue);
                double kirlilikDegree = Kirlilik.GetMembershipDegree(rule.KirlilikCondition, kirlilikValue);

                double firingStrength = rule.CalculateFiringStrength(hassaslikDegree, miktarDegree, kirlilikDegree);

                if (firingStrength > 0)
                {
                    firedRules.Add(new Tuple<FuzzyRule, double>(rule, firingStrength));
                }
            }

            return firedRules;
        }

        // Ağırlıklı Ortalama durulaştırma yöntemi
        public double DefuzzifyWeightedAverage(List<Tuple<FuzzyRule, double>> firedRules, FuzzySet outputSet, Func<FuzzyRule, string> getOutputMembershipFunction)
        {
            double weightedSum = 0;
            double sumOfWeights = 0;

            foreach (var firedRule in firedRules)
            {
                string outputMfName = getOutputMembershipFunction(firedRule.Item1);
                MembershipFunction mf = outputSet.GetMembershipFunction(outputMfName);

                // Üçgen üyelik fonksiyonu için merkez noktası
                double center;
                if (mf.Type == MembershipFunctionType.Triangular)
                {
                    center = mf.Parameters[1]; // Üçgen için ortadaki değer
                }
                else // Yamuk
                {
                    // Yamuğun merkezini orta noktalarının ortalaması olarak al
                    center = (mf.Parameters[1] + mf.Parameters[2]) / 2;
                }

                weightedSum += firedRule.Item2 * center;
                sumOfWeights += firedRule.Item2;
            }

            if (sumOfWeights == 0)
                return 0;

            return weightedSum / sumOfWeights;
        }

        // Centroid (Ağırlık Merkezi) durulaştırma yöntemi
        public double DefuzzifyCentroid(List<Tuple<FuzzyRule, double>> firedRules, FuzzySet outputSet, Func<FuzzyRule, string> getOutputMembershipFunction, int sampleCount = 100)
        {
            // Çıktı alanını örnekleme
            double step = (outputSet.Max - outputSet.Min) / sampleCount;
            double numerator = 0;
            double denominator = 0;

            for (int i = 0; i <= sampleCount; i++)
            {
                double x = outputSet.Min + i * step;
                double aggregatedMembership = 0;

                foreach (var firedRule in firedRules)
                {
                    string outputMfName = getOutputMembershipFunction(firedRule.Item1);
                    MembershipFunction mf = outputSet.GetMembershipFunction(outputMfName);
                    double membership = Math.Min(firedRule.Item2, mf.CalculateMembership(x));
                    aggregatedMembership = Math.Max(aggregatedMembership, membership);
                }

                numerator += x * aggregatedMembership;
                denominator += aggregatedMembership;
            }

            if (denominator == 0)
                return 0;

            return numerator / denominator;
        }

        // Mamdani çıkarım sistemi ile dönüş hızını hesaplar
        public double CalculateDonusHizi(double hassaslikValue, double miktarValue, double kirlilikValue)
        {
            var firedRules = EvaluateRules(hassaslikValue, miktarValue, kirlilikValue);
            return DefuzzifyWeightedAverage(firedRules, DonusHizi, r => r.DonusHiziOutput);
        }

        // Mamdani çıkarım sistemi ile süreyi hesaplar
        public double CalculateSure(double hassaslikValue, double miktarValue, double kirlilikValue)
        {
            var firedRules = EvaluateRules(hassaslikValue, miktarValue, kirlilikValue);
            return DefuzzifyWeightedAverage(firedRules, Sure, r => r.SureOutput);
        }

        // Mamdani çıkarım sistemi ile deterjan miktarını hesaplar
        public double CalculateDeterjanMiktari(double hassaslikValue, double miktarValue, double kirlilikValue)
        {
            var firedRules = EvaluateRules(hassaslikValue, miktarValue, kirlilikValue);
            return DefuzzifyWeightedAverage(firedRules, DeterjanMiktari, r => r.DeterjanOutput);
        }

        // Centroid durulaştırma ile dönüş hızını hesaplar
        public double CalculateDonusHiziCentroid(double hassaslikValue, double miktarValue, double kirlilikValue)
        {
            var firedRules = EvaluateRules(hassaslikValue, miktarValue, kirlilikValue);
            return DefuzzifyCentroid(firedRules, DonusHizi, r => r.DonusHiziOutput);
        }

        // Centroid durulaştırma ile süreyi hesaplar
        public double CalculateSureCentroid(double hassaslikValue, double miktarValue, double kirlilikValue)
        {
            var firedRules = EvaluateRules(hassaslikValue, miktarValue, kirlilikValue);
            return DefuzzifyCentroid(firedRules, Sure, r => r.SureOutput);
        }

        // Centroid durulaştırma ile deterjan miktarını hesaplar
        public double CalculateDeterjanMiktariCentroid(double hassaslikValue, double miktarValue, double kirlilikValue)
        {
            var firedRules = EvaluateRules(hassaslikValue, miktarValue, kirlilikValue);
            return DefuzzifyCentroid(firedRules, DeterjanMiktari, r => r.DeterjanOutput);
        }
    }
}