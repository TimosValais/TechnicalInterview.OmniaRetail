namespace TechnicalInterview.OmniaRetail.Application.Extensions
{
    internal static class MathExtensions
    {
        public static double StandardDeviation(this List<int> values)
        {
            //TODO: Could also throw an error here, it's a bit of a semantic issue (dividing by 0)
            if ((values.Count == 0))
            {
                return 0;
            }
            double average = values.Average();
            List<double> deviations = [];
            for (int i = 0; i < values.Count; i++)
            {
                deviations.Add(Math.Pow(values[i] - average, 2));
            }
            double variance = deviations.Average();
            return Math.Sqrt(variance);
        }
    }
}
