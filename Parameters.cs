using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;

namespace SeventUDS_2_2
{
    public struct Parameters
    {
        public EntityReference CarClass;
        public EntityReference Car;
        public EntityReference Contact;
        public DateTime PickupDate;
        public DateTime HandoverDate;
        public int PickupLocation;
        public int ReturnLocation;
        public StatusProbability Status;
        public bool Paid;
        public bool Damages;
        public decimal Price;
        public MyOptionSetValue[] ReportTypeValues;
        public CrmServiceClient ServiceClient;
    }
}