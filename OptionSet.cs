using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeventUDS_2_2
{
    public static class OptionSet
    {
        public static MyOptionSetValue[] GetArrOSValues(string entityLogicalName, string logicalName, CrmServiceClient serviceClient)
        {
            //get "Location' option set
            var attributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityLogicalName,
                LogicalName = logicalName,
                RetrieveAsIfPublished = true
            };

            var attributeResponse = (RetrieveAttributeResponse)serviceClient.Execute(attributeRequest);
            var attributeMetadata = (EnumAttributeMetadata)attributeResponse.AttributeMetadata;

            var optionList = (from o in attributeMetadata.OptionSet.Options
                              select new { Value = o.Value, Text = o.Label.UserLocalizedLabel.Label }).ToList();


            MyOptionSetValue[] arrOSValues = new MyOptionSetValue[optionList.Count];
            int cnt = 0;
            foreach (var osPair in optionList)
            {
                MyOptionSetValue optionSet = new MyOptionSetValue((int)osPair.Value, osPair.Text);
                arrOSValues[cnt] = optionSet;
                cnt++;
            }

            return arrOSValues;
        }
    }
}
