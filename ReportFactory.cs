using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeventUDS_2_2
{
    public static class ReportFactory
    {
        public static Entity CreatePickupReport(EntityReference car, DateTime date, MyOptionSetValue[] optionSet)
        {
            Entity pickupReport = new Entity("cr03e_cartransferreport");

            pickupReport.Attributes.Add("cr03e_name", "Pickup report " + date.ToString());
            pickupReport.Attributes.Add("cr03e_car", car);
            pickupReport.Attributes.Add("cr03e_type", new OptionSetValue(optionSet.Where(o => o.Text == "Pickup").FirstOrDefault().Value.Value));
            pickupReport.Attributes.Add("cr03e_date", date);

            return pickupReport;
        }

        public static Entity CreateReturnReport(EntityReference car, DateTime date, MyOptionSetValue[] optionSet, bool damaged)
        {
            Entity returnReport = new Entity("cr03e_cartransferreport");

            returnReport.Attributes.Add("cr03e_name", "Return report " + date.ToString());
            returnReport.Attributes.Add("cr03e_car", car);
            returnReport.Attributes.Add("cr03e_type", new OptionSetValue(optionSet.Where(o => o.Text == "Return").FirstOrDefault().Value.Value));
            returnReport.Attributes.Add("cr03e_date", date);

            if (damaged)
            {
                returnReport.Attributes.Add("cr03e_damages", damaged);
                returnReport.Attributes.Add("cr03e_damagedescription", "damage");
            }

            return returnReport;
        }

        public static EntityReference GetReportRef(Entity report, CrmServiceClient serviceClient)
        {
            Guid guid = serviceClient.Create(report);

            EntityReference entityReference = new EntityReference(report.LogicalName, guid);

            return entityReference;
        }
    }
}
