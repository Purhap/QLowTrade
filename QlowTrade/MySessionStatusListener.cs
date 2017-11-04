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
        private O2GResponse mResponse;

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
        public void onRequestCompleted(String requestID, O2GResponse response)
        {
            O2GResponseReaderFactory responseFactory = mSession.getResponseReaderFactory();
            if(responseFactory!=null)
            {
                O2GMarketDataSnapshotResponseReader reader = responseFactory.createMarketDataSnapshotReader(response);
                for(int i = 0; i< reader.Count; i++)
                {

                    System.Console.WriteLine("\n\tAskOpen =" + reader.getAskOpen(i) +
                                      "\n\tAskHigh = " + reader.getAskHigh(i) +
                                      "\n\tAskLow = " + reader.getAskLow(i) +
                                      "\n\tAskClose =" + reader.getAskClose(i));
                }
            }
        }

        internal O2GResponse getResponse()
        {
            return mResponse;
            //throw new NotImplementedException();
        }
    }
}
