using System;
using System.Collections.Generic;

namespace BulanikCamasirMakinesi
{
    public class FuzzyRule
    {
        public int RuleNumber { get; private set; }
        public string RuleText { get; private set; }

        // Giriş şartları (Hassaslık, Miktar, Kirlilik)
        public string HassaslikCondition { get; private set; }
        public string MiktarCondition { get; private set; }
        public string KirlilikCondition { get; private set; }

        // Çıkış sonuçları (Dönüş Hızı, Süre, Deterjan Miktarı)
        public string DonusHiziOutput { get; private set; }
        public string SureOutput { get; private set; }
        public string DeterjanOutput { get; private set; }

        public FuzzyRule(int ruleNumber, string hassaslikCondition, string miktarCondition, string kirlilikCondition,
                        string donusHiziOutput, string sureOutput, string deterjanOutput)
        {
            RuleNumber = ruleNumber;
            HassaslikCondition = hassaslikCondition;
            MiktarCondition = miktarCondition;
            KirlilikCondition = kirlilikCondition;
            DonusHiziOutput = donusHiziOutput;
            SureOutput = sureOutput;
            DeterjanOutput = deterjanOutput;

            // Kural metnini oluştur
            RuleText = $"IF Hassaslık = {hassaslikCondition} AND Miktar = {miktarCondition} AND Kirlilik = {kirlilikCondition} " +
                      $"THEN Dönüş Hızı = {donusHiziOutput} AND Süre = {sureOutput} AND Deterjan = {deterjanOutput}";
        }

        // Mamdani ateşleme değerini hesaplar (minimum operatörü)
        public double CalculateFiringStrength(double hassaslikDegree, double miktarDegree, double kirlilikDegree)
        {
            return Math.Min(Math.Min(hassaslikDegree, miktarDegree), kirlilikDegree);
        }

        public override string ToString()
        {
            return $"Kural {RuleNumber}: {RuleText}";
        }
    }
}