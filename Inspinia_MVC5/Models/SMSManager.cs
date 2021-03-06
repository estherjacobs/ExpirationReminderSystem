﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Inspinia_MVC5.Models
{
    public class SMSManager
    {
        public void Notification(string phoneNumber, string message)
        {
            string html = string.Empty;
            string formattedPhone = string.Empty;

            if(phoneNumber.Substring(0,1) != "1")
            {
                formattedPhone = "1" + phoneNumber;
            }
            else
            {
                formattedPhone = phoneNumber;
            }
            string url = "https://rest.nexmo.com/sms/json?api_key=b1ee06e1&api_secret=94dc48fb49e1dae6&from=12027353717&to=" + formattedPhone + "&text=" + message;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                html = reader.ReadToEnd();
            }
        }
     
    }
}