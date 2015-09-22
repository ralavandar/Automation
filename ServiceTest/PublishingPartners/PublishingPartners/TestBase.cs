using ConfigurationSettings;
using NUnit.Framework;
using ServiceTestEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishingPartners
{
    [TestFixture]
    public class TestBase
    {
        protected HTTPContext httpContext;
        protected ConfigSettings configuration;
        protected String environmentSettings;

        [TestFixtureSetUp]
        public void SetUpRequestHostSettings()
        {
            httpContext = new HTTPContext();
            environmentSettings = ConfigurationManager.AppSettings.Get("EnvironmentSettings");
            String[] configFiles = new String[] {Directory.GetCurrentDirectory() + @"\TestConfig\AppTestData",
                                                  Directory.GetCurrentDirectory() + @"\TestConfig\" + environmentSettings};

            ConfigSettings configSettings = new ConfigSettings(configFiles);
            configuration = configSettings.getConfiguration();
        }
    }
}
