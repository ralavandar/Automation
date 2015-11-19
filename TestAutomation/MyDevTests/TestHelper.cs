using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestAutomation.MyDevTests
{
    class BrowserTestHelper
    {
        public int OSNum { get; set; }
        public int BrowserNum { get; set; }
        public List<List<string>> OSes { get; set; }
        public List<List<string>> Browsers { get; set; }

        public BrowserTestHelper(List<List<string>> oses, List<List<string>> browsers)
        {
            OSNum = 0;
            BrowserNum = 0;
            OSes = oses;
            Browsers = browsers;
        }

        public List<string> GetOS()
        {
            return OSes[OSNum];
        }

        public List<string> GetBrowser()
        {
            return Browsers[BrowserNum];
        }

        public void GoToNextBrowser()
        {
            if (++BrowserNum == Browsers.Count)
            {
                OSNum++;
                BrowserNum = 0;

                Console.WriteLine("Desktop OS num: " + OSNum);

                if (OSNum == OSes.Count)
                {
                    OSNum = 0;
                }
            }
        }
    }
}
