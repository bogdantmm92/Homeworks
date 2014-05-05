using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Homework.Utils
{
	public class SubmissionHelper
	{
        private static SubmissionHelper instance;
        public static SubmissionHelper _Instance
        {
            get
            {
                if (instance == null)
                    instance = new SubmissionHelper();
                return instance;
            }
            private set { instance = value; }
        }

        private static string username = "bogdantmm";
        private static string password = "bogdanapi";

        public Ideone_ServiceService client;
        private SubmissionHelper()
        {
            client = new Ideone_ServiceService();
        }

        public Dictionary<string, string> uploadSource(string source, string input, int language = 1)
        {
            var submission = getResult(client.createSubmission(username, password, source, language, input, true, false));

            if("OK" != submission["error"])
                return null;

            Dictionary<string, string> submissionDetails = null;
			do{
                submissionDetails = getResult(client.getSubmissionDetails(username, password, submission["link"], false, false, true, false, true));
            }while(int.Parse(submissionDetails["status"]) != 0);

            return submissionDetails;
        }

        public Dictionary<string, string> getResult(Object[] result)
        {
            Dictionary<string, string> resultOut = new Dictionary<string, string>();
            foreach (object o in result)
            {
                if (o is XmlElement)
                {
                    XmlNodeList x = ((XmlElement)o).ChildNodes;
                    resultOut.Add(x.Item(0).InnerText, x.Item(1).InnerText);
                }
            }
            return resultOut;
        }


	}
}