// <copyright file="ServiceConstants.cs" company="Rabobank">
//  Copyright (c) 2022 Rabobank Training
// </copyright>

namespace Rabobank.Training.ClassLibrary.Constants
{
    /// <summary>
    /// ServiceConstants class contains basic static constants which can be used across various projects.
    /// </summary>
    public static class ServiceConstants
    {
        public const string XMLNamespace = "http://amt.rnss.rabobank.nl/";
        public const string ConfigurationFileName = "appsettings.json";
        public const string XMLFileName = "FundsOfMandatesData.xml";
        public const string XMLDataFilePath = "XMLDataFilePath";
        public const string Liquidity = "Liquidity";
        public const string IsTestService = "IsTestService";
        public const string PositionsJson = "[{\"Code\":\"NL0000009165\",\"Name\":\"Heineken\",\"Value\":12345.0,\"Mandates\":[]},{\"Code\":\"NL0000287100\",\"Name\":\"Optimix Mix Fund\",\"Value\":23456.0,\"Mandates\":[]},{\"Code\":\"LU0035601805\",\"Name\":\"DP Global Strategy L High\",\"Value\":34567.0,\"Mandates\":[]},{\"Code\":\"NL0000292332\",\"Name\":\"Rabobank Core Aandelen Fonds T2\",\"Value\":45678.0,\"Mandates\":[]},{\"Code\":\"LU0042381250\",\"Name\":\"Morgan Stanley Invest US Gr Fnd\",\"Value\":56789.0,\"Mandates\":[]}]";
    }
}
