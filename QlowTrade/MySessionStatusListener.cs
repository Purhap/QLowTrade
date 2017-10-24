using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fxcore2;



namespace QlowTrade
{
    public class MySessionStatusListener : fxcore2.IO2GSessionStatus
    {
        
        private O2GSessionStatusCode mCode = O2GSessionStatusCode.Unknown;
        private O2GSession mSession = null;

        private string errorStr;
        private String mDBName = "";
        private String mPin = "";
       
    
        private static string sSessionID = "";
        public static string SessionID
        {
            get { return sSessionID; }
        }
        public MySessionStatusListener(O2GSession session, String dbName, String pin)
        {
            mSession = session;
            mDBName = dbName;
            mPin = pin;
        }
  
        public O2GSessionStatusCode Status
        {
            get
            {
                return mCode;
            }
        }

        public void onLoginFailed(string error)
        {
            errorStr = error;
        }

        public void onSessionStatusChanged(O2GSessionStatusCode code)
        {
            mCode = code;
            Console.WriteLine(code.ToString());
            if (code == O2GSessionStatusCode.TradingSessionRequested)
            {
                if (SessionID == "")
                    Console.WriteLine("Argument for trading session ID is missing");
                else
                    mSession.setTradingSession(SessionID, mPin);
            }
        }
    }
}
