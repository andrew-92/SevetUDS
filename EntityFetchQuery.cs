using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeventUDS_2_2
{
    public static class EntityFetchQuery
    {
        public static string GetCarClassesQuery()
        {
            return @"<fetch version='1.0' output-format='xml - platform' mapping='logical' distinct='false'>
                <entity name = 'cr03e_carclass' >
                    <attribute name = 'createdon' />
                    <attribute name = 'cr03e_price' />
                    <attribute name = 'cr03e_classdescription' />
                    <attribute name = 'cr03e_classcode' />
                    <attribute name = 'cr03e_name' />
                    <attribute name = 'cr03e_carclassid' />
                    <order attribute = 'createdon' descending = 'false' />
                    <filter type = 'and' >
                        <condition attribute = 'statecode' operator= 'eq' value = '0' />
                    </filter >
                 </entity >
             </fetch > ";
        }

        public static string GetCarsQuery()
        {
            return @"<fetch version='1.0' output-format='xml - platform' mapping='logical' distinct='false'>
                    <entity name = 'cr03e_car' >
                        <attribute name = 'cr03e_name' />
                        <attribute name = 'createdon' />
                        <attribute name = 'cr03e_purchase' />
                        <attribute name = 'cr03e_production' />
                        <attribute name = 'cr03e_carmodel' />
                        <attribute name = 'cr03e_carmanufacturer' />
                        <attribute name = 'cr03e_carclass' />
                        <attribute name = 'cr03e_vin' />
                        <attribute name = 'cr03e_carid' />
                        <order attribute = 'cr03e_name' descending = 'false' />
                        <filter type = 'and' >
                            <condition attribute = 'statecode' operator= 'eq' value = '0' />
                        </filter >
                    </entity >
                </fetch > ";
        }

        public static string GetContactsQuery()
        {
            return @"<fetch version='1.0' output-format='xml - platform' mapping='logical' distinct='false'>
                <entity name = 'contact' >
                    <attribute name = 'fullname' />
                    <attribute name = 'parentcustomerid' />
                    <attribute name = 'telephone1' />
                    <attribute name = 'emailaddress1' />
                    <attribute name = 'contactid' />
                    <order attribute = 'fullname' descending = 'false' />
                    <filter type = 'and' >
                         <condition attribute = 'ownerid' operator= 'eq-userid' />
                         <condition attribute = 'statecode' operator= 'eq' value = '0' />
                    </filter >
                </entity >
            </fetch > ";
        }
    }
}
