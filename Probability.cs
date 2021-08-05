using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeventUDS_2_2
{
    public static class Probability
    {
        public static List<StatusProbability> GetStatusProbabilitiesList(MyOptionSetValue[] arrOSStatusValues, int numberOfRents)
        {
            List<StatusProbability> statusProbabilities = new List<StatusProbability>();
            foreach (MyOptionSetValue setValue in arrOSStatusValues)
            {
                StatusProbability statusProbability = new StatusProbability();

                statusProbability.Name = setValue.Text;
                statusProbability.OSValue = setValue.Value;
                switch (setValue.Text)
                {
                    case "Created (Active)":
                        statusProbability.Probability = 0.05;
                        break;
                    case "Confirmed (Active)":
                        statusProbability.Probability = 0.05;
                        break;
                    case "Renting (Active)":
                        statusProbability.Probability = 0.05;
                        break;
                    case "Returned (Inactive)":
                        statusProbability.Probability = 0.75;
                        break;
                    case "Canceled (Inactive)":
                        statusProbability.Probability = 0.1;
                        break;
                }

                statusProbability.AmountOfRents = numberOfRents;

                statusProbabilities.Add(statusProbability);
            }

            return statusProbabilities;
        }

        public static List<PaidPobability> GetPaidProbabilitiesList(MyOptionSetValue[] arrOSStatusValues, int numberOfRents)
        {
            List<PaidPobability> paidProbabilities = new List<PaidPobability>();
            foreach (MyOptionSetValue setValue in arrOSStatusValues)
            {
                PaidPobability paidProbability = new PaidPobability();

                paidProbability.Name = setValue.Text;
                

                switch (setValue.Text)
                {
                    case "Confirmed (Active)":
                        paidProbability.Count = numberOfRents * 0.05 * 0.9;
                        paidProbabilities.Add(paidProbability);
                        break;
                    case "Renting (Active)":
                        paidProbability.Count = numberOfRents * 0.05 * 0.999;
                        paidProbabilities.Add(paidProbability);
                        break;
                    case "Returned (Inactive)":
                        paidProbability.Count = numberOfRents * 0.75 * 0.9998;
                        paidProbabilities.Add(paidProbability);
                        break;
                }
            }

            return paidProbabilities;
        }

    }
}
