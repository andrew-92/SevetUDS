using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace SeventUDS_2_2
{
    public static class RentFactory
    {
        public static void CreateRent(Parameters parameters)
        {
            Entity rent = new Entity("cr03e_rent");

            rent.Attributes.Add("cr03e_carclass", parameters.CarClass);
            rent.Attributes.Add("cr03e_car", parameters.Car);
            rent.Attributes.Add("cr03e_customer", parameters.Contact) ;
            rent.Attributes.Add("cr03e_reservedpickup", parameters.PickupDate);
            rent.Attributes.Add("cr03e_reservedhandover", parameters.HandoverDate);
            rent.Attributes.Add("cr03e_pickuplocation", new OptionSetValue(parameters.PickupLocation));
            rent.Attributes.Add("cr03e_returnlocation", new OptionSetValue(parameters.ReturnLocation));
            rent.Attributes.Add("cr03e_actualpickup", parameters.PickupDate);
            rent.Attributes.Add("cr03e_actualreturn", parameters.HandoverDate);
            rent.Attributes.Add("cr03e_paid", parameters.Paid);
            rent.Attributes.Add("cr03e_price", parameters.Price);
            rent.Attributes.Add("cr03e_status", new OptionSetValue((int)parameters.Status.OSValue));

            if(parameters.Status.Name == "Renting(Active)")
            {
                Entity pickupReport = ReportFactory.CreatePickupReport(parameters.Car, parameters.PickupDate, parameters.ReportTypeValues);
                EntityReference pickupReportRef = ReportFactory.GetReportRef(pickupReport, parameters.ServiceClient);

                rent.Attributes.Add("cr03e_pickup", pickupReportRef);
            }
            else if(parameters.Status.Name == "Returned (Inactive)")
            {
                Entity pickupReport = ReportFactory.CreatePickupReport(parameters.Car, parameters.PickupDate, parameters.ReportTypeValues);
                Entity returnReport = ReportFactory.CreateReturnReport(parameters.Car, parameters.HandoverDate, parameters.ReportTypeValues, parameters.Damages);

                EntityReference pickupReportRef = ReportFactory.GetReportRef(pickupReport, parameters.ServiceClient);
                EntityReference returnReportRef = ReportFactory.GetReportRef(returnReport, parameters.ServiceClient);

                rent.Attributes.Add("cr03e_pickup", pickupReportRef);
                rent.Attributes.Add("cr03e_return", returnReportRef);
            }

            //
            Guid guid = parameters.ServiceClient.Create(rent);
        }
    }
}
