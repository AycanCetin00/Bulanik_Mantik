using System;
using System.Collections.Generic;
using System.Linq;

namespace BulanikCamasirMakinesi
{
    public class FuzzySet
    {
        public string Name { get; private set; }
        public List<MembershipFunction> MembershipFunctions { get; private set; }
        public double Min { get; private set; }
        public double Max { get; private set; }

        public FuzzySet(string name, double min, double max)
        {
            Name = name;
            MembershipFunctions = new List<MembershipFunction>();
            Min = min;
            Max = max;
        }

        // Üyelik fonksiyonu ekleme
        public void AddMembershipFunction(MembershipFunction membershipFunction)
        {
            MembershipFunctions.Add(membershipFunction);
        }

        // Verilen değer için bulanık küme adı ve üyelik derecesini döndürür
        public List<Tuple<string, double>> Fuzzify(double value)
        {
            List<Tuple<string, double>> result = new List<Tuple<string, double>>();

            foreach (var mf in MembershipFunctions)
            {
                double degree = mf.CalculateMembership(value);
                if (degree > 0)
                {
                    result.Add(new Tuple<string, double>(mf.Name, degree));
                }
            }

            return result;
        }

        // Belirli bir üyelik fonksiyonu için üyelik derecesini hesaplar
        public double GetMembershipDegree(string membershipFunctionName, double value)
        {
            var mf = MembershipFunctions.FirstOrDefault(m => m.Name == membershipFunctionName);
            if (mf != null)
            {
                return mf.CalculateMembership(value);
            }
            return 0;
        }

        // Belirli bir isimle üyelik fonksiyonunu döndürür
        public MembershipFunction GetMembershipFunction(string name)
        {
            return MembershipFunctions.FirstOrDefault(m => m.Name == name);
        }
    }
}