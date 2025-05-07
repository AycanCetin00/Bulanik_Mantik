using System;
using System.Collections.Generic;

namespace BulanikCamasirMakinesi
{
    // Üyelik fonksiyonu tipleri için enum
    public enum MembershipFunctionType
    {
        Triangular, // Üçgen
        Trapezoidal // Yamuk
    }

    public class MembershipFunction
    {
        public string Name { get; private set; }
        public MembershipFunctionType Type { get; private set; }
        public double[] Parameters { get; private set; }

        // Üçgen üyelik fonksiyonu için constructor
        public MembershipFunction(string name, double left, double center, double right)
        {
            Name = name;
            Type = MembershipFunctionType.Triangular;
            Parameters = new double[] { left, center, right };
        }

        // Yamuk üyelik fonksiyonu için constructor
        public MembershipFunction(string name, double left, double leftMiddle, double rightMiddle, double right)
        {
            Name = name;
            Type = MembershipFunctionType.Trapezoidal;
            Parameters = new double[] { left, leftMiddle, rightMiddle, right };
        }

        // Üyelik derecesini hesaplayan fonksiyon
        public double CalculateMembership(double value)
        {
            if (Type == MembershipFunctionType.Triangular)
            {
                // Üçgen üyelik fonksiyonu hesaplaması
                if (value <= Parameters[0] || value >= Parameters[2])
                    return 0;
                else if (value == Parameters[1])
                    return 1;
                else if (value < Parameters[1])
                    return (value - Parameters[0]) / (Parameters[1] - Parameters[0]);
                else
                    return (Parameters[2] - value) / (Parameters[2] - Parameters[1]);
            }
            else // Yamuk
            {
                // Yamuk üyelik fonksiyonu hesaplaması
                if (value <= Parameters[0] || value >= Parameters[3])
                    return 0;
                else if (value >= Parameters[1] && value <= Parameters[2])
                    return 1;
                else if (value < Parameters[1])
                    return (value - Parameters[0]) / (Parameters[1] - Parameters[0]);
                else
                    return (Parameters[3] - value) / (Parameters[3] - Parameters[2]);
            }
        }

        // Grafik için X,Y noktalarını döndüren fonksiyon
        public List<Tuple<double, double>> GetGraphPoints(double min, double max, int pointCount = 100)
        {
            List<Tuple<double, double>> points = new List<Tuple<double, double>>();
            double step = (max - min) / pointCount;

            for (int i = 0; i <= pointCount; i++)
            {
                double x = min + i * step;
                double y = CalculateMembership(x);
                points.Add(new Tuple<double, double>(x, y));
            }

            return points;
        }
    }
}