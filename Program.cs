using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeventUDS_2_2
{
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfRents = 30;

            //connect to CRM
            CrmServiceClient serviceClient = new CrmServiceClient(ConfigurationManager.ConnectionStrings["MyCRMServer"].ConnectionString);

            //get option sets
            MyOptionSetValue[] arrOSLocationValues = OptionSet.GetArrOSValues("cr03e_rent", "cr03e_pickuplocation", serviceClient);
            MyOptionSetValue[] arrOSStatusValues = OptionSet.GetArrOSValues("cr03e_rent", "cr03e_status", serviceClient);
            MyOptionSetValue[] arrOSReportTypeValues = OptionSet.GetArrOSValues("cr03e_cartransferreport", "cr03e_type", serviceClient);

            //get entity collections from CRM
            string carClassesQuery = EntityFetchQuery.GetCarClassesQuery();
            string carsQuery = EntityFetchQuery.GetCarsQuery();
            string contactsQuery = EntityFetchQuery.GetContactsQuery();

            EntityCollection carClasses = serviceClient.RetrieveMultiple(new FetchExpression(carClassesQuery));
            EntityCollection cars = serviceClient.RetrieveMultiple(new FetchExpression(carsQuery));
            EntityCollection contacts = serviceClient.RetrieveMultiple(new FetchExpression(contactsQuery));

            //
            DateTime StartOfPeriod = new DateTime(2019, 1, 1);
            DateTime EndOfPeriod = new DateTime(2020, 12, 31);

            //calculate probabilities 
            List<StatusProbability> statusProbabilities = Probability.GetStatusProbabilitiesList(arrOSStatusValues, numberOfRents);
            List<PaidPobability> paidProbabilities = Probability.GetPaidProbabilitiesList(arrOSStatusValues, numberOfRents);
            DamageProbability damageProbability = new DamageProbability("Returned (Inactive)", numberOfRents * 0.75 * 0.05);

            Console.WriteLine("Operation start at " + DateTime.Now.ToString());
            for (int i = 0; i < numberOfRents; i++)
            {
                //pickupDate, handoverDate
                DateTime pickupDate = GetRandomDateTime(StartOfPeriod, EndOfPeriod);
                DateTime handoverDate = GetRandomDateTime(pickupDate, pickupDate.AddDays(new Random().Next(1, 30)));

                //carClass, car
                Entity carClass = carClasses.Entities[new Random().Next(1, carClasses.Entities.Count)];
                var carsByClass = cars.Entities.Where(c => c.Attributes["cr03e_carclass"].Equals(carClass.ToEntityReference())).ToArray();
                Entity car = carsByClass[new Random().Next(0, carsByClass.Count() - 1)];

                //get Price
                Money currency = (Money)carClass.Attributes["cr03e_price"];
                double pricePerDay = (double)currency.Value;
                int rentDays = (handoverDate.Date - pickupDate.Date).Days;

                decimal price = (decimal)(pricePerDay * rentDays);

                //get contact
                var contact = contacts.Entities[new Random().Next(1, contacts.Entities.Count)];

                //get pickup/return locations
                int pickupLocation = (int)arrOSLocationValues[new Random().Next(0, arrOSLocationValues.Count() - 1)].Value;
                int returnLocation = (int)arrOSLocationValues[new Random().Next(0, arrOSLocationValues.Count() - 1)].Value;

                //get status
                StatusProbability status = GetStatus(statusProbabilities);
                while (!AvailableStatus(statusProbabilities, status))
                {
                    status = GetStatus(statusProbabilities);
                }

                bool damages = CheckDamages(damageProbability, status);

                //check paid
                bool paidValue = CheckPaid(paidProbabilities, status); 

                Parameters parameters = new Parameters
                {
                    CarClass = carClass.ToEntityReference(), 
                    Car = car.ToEntityReference(),
                    Contact = contact.ToEntityReference(),
                    PickupDate = pickupDate,
                    HandoverDate = handoverDate,
                    PickupLocation = pickupLocation,
                    ReturnLocation = returnLocation,
                    Status = status,
                    Paid = paidValue,
                    Price = price,
                    ReportTypeValues = arrOSReportTypeValues,
                    ServiceClient = serviceClient,
                    Damages = damages
                };

                //
                RentFactory.CreateRent(parameters);

            }

            Console.WriteLine("Operation complete at " + DateTime.Now.ToString());
            Console.ReadKey();
        }

        public static bool CheckDamages(DamageProbability damageProbability, StatusProbability status)
        {
           if(damageProbability.Name == status.Name && damageProbability.Count > 0)
            {
                damageProbability.Count--;
                return true;
            }

            return false;
        }

        public static bool CheckPaid(List<PaidPobability> paidProbabilities, StatusProbability status)
        {
            var statusInList = paidProbabilities.Where(o => o.Name == status.Name).FirstOrDefault();
            if (statusInList != null && statusInList.Count > 0)
            {
                statusInList.Count--;
                return true;
            }

            return false;
        }

        public static StatusProbability GetStatus(List<StatusProbability> probabilities)
        {
            Random random = new Random();
            var temp = probabilities.Where(x => x.Available);
            return temp.ElementAt(random.Next(0, temp.Count() - 1));
        }

        public static bool AvailableStatus(List<StatusProbability> probabilities, StatusProbability set_status)
        {
            StatusProbability temp = probabilities.FirstOrDefault(x => x == set_status);
            if (temp.Counter > 0)
            {
                temp.Counter--;
                return true;
            }
            else
            {
                temp.Available = false;
                return false;
            }
        }

        static DateTime GetRandomDateTime(DateTime StartOfPeriod, DateTime EndOfPeriod)
        {
            if (StartOfPeriod >= EndOfPeriod)
            {
                throw new Exception("Параметр \"from\" должен быть меньше параметра \"to\"!");
            }

            Random random = new Random();
            TimeSpan range = EndOfPeriod - StartOfPeriod;
            var randomTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));
            return StartOfPeriod + randomTimeSpan;
        }

    }
}
