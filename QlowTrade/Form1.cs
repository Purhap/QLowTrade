using System;
using System.Drawing;
using System.Windows.Forms;
using fxcore2;
using System.Threading;
using GetHistPrices;
using System.Configuration;



namespace QlowTrade
{
    public partial class Form1 : Form
    {
        public MySessionStatusListener statusListener;
        
        public static fxcore2.O2GSession mSession;
        private static string sInstrument = "";

        private static string sSessionID = "";
        public static string SessionID
        {
            get { return sSessionID; }
        }
        public Form1()
        {
           
            InitializeComponent();
            mSession = O2GTransport.createSession();
            

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectBt_Click(object sender, EventArgs e)
        {
            //mSession = O2GTransport.createSession();
            statusListener = new MySessionStatusListener(mSession, "tempDb", "pin");
            mSession.subscribeSessionStatus(statusListener);
            mSession.useTableManager(O2GTableManagerMode.Yes, null);

            mSession.login("70882056", "2945", "http://www.fxcorporate.com/Hosts.jsp", "Demo");

            while (statusListener.Status != O2GSessionStatusCode.Connected && statusListener.Status != O2GSessionStatusCode.Disconnected)
                Thread.Sleep(50);

            if(statusListener.Status== O2GSessionStatusCode.Connected)
            {
                O2GTableManager manager = mSession.getTableManager();
                while (manager.getStatus() == O2GTableManagerStatus.TablesLoading)
                    Thread.Sleep(50);

                O2GOffersTable offers = null;

                if(manager.getStatus() == O2GTableManagerStatus.TablesLoaded)
                {
                    offers = (O2GOffersTable)manager.getTable(O2GTableType.Offers);
                    printOffers(offers);
                    offers.RowChanged += new EventHandler<RowEventArgs>(offers_RowChanged);
                }
                else
                {
                    statusLabel.Text = "Tables loading failed!";
                }
                if(offers != null)
                {
                    offers.RowChanged -= new EventHandler<RowEventArgs>(offers_RowChanged);
             
                }
                //mSession.logout();
            }
            //mSession.unsubscribeSessionStatus(statusListener);
            //mSession.Dispose();
            
            statusLabel.Text = "Connected";
            statusLabel.BackColor = Color.GreenYellow;
        }

        public static void printOffers(O2GOffersTable offers)
        {
            O2GOfferTableRow row = null;
            O2GTableIterator iterator = new O2GTableIterator();
            while (offers.getNextRow(iterator, out row))
            {
                string sCurrentInstrument = row.Instrument;
                if ((sInstrument.Equals("")) || (sInstrument.Equals(sCurrentInstrument)))
                    PrintOffer(row);
            }
        }
        public static void PrintOffer(O2GOfferTableRow row)
        {
            Console.WriteLine("OfferID: {0}, Instrument: {1}, Bid: {2}, Ask: {3}, PipCost: {4}", row.OfferID, row.Instrument, row.Bid, row.Ask, row.PipCost);
        }
        static void offers_RowChanged(object sender, RowEventArgs e)
        {
            O2GOfferTableRow row = (O2GOfferTableRow)e.RowData;
            string sCurrentInstrument = row.Instrument;
            if ((sInstrument.Equals("")) || (sInstrument.Equals(sCurrentInstrument)))
                PrintOffer(row);
        }

        public static void PrintPrices(O2GSession session, O2GResponse response)
        {
            Console.WriteLine("Request with RequestID={0} is completed:", response.RequestID);
            O2GResponseReaderFactory factory = session.getResponseReaderFactory();
            if (factory != null)
            {
                O2GMarketDataSnapshotResponseReader reader = factory.createMarketDataSnapshotReader(response);
                for (int ii = reader.Count - 1; ii >= 0; ii--)
                {
                    if (reader.isBar)
                    {
                        Console.WriteLine("DateTime={0}, BidOpen={1}, BidHigh={2}, BidLow={3}, BidClose={4}, AskOpen={5}, AskHigh={6}, AskLow={7}, AskClose={8}, Volume={9}",
                                reader.getDate(ii), reader.getBidOpen(ii), reader.getBidHigh(ii), reader.getBidLow(ii), reader.getBidClose(ii),
                                reader.getAskOpen(ii), reader.getAskHigh(ii), reader.getAskLow(ii), reader.getAskClose(ii), reader.getVolume(ii));
                    }
                    else
                    {
                        Console.WriteLine("DateTime={0}, Bid={1}, Ask={2}", reader.getDate(ii), reader.getBidClose(ii), reader.getAskClose(ii));
                    }
                }
            }
        }

        public void GetHistoryPrices(O2GSession session, string sInstrument, string sTimeframe, DateTime dtFrom, DateTime dtTo, ResponseListener responseListener)
        {
            O2GRequestFactory factory = session.getRequestFactory();
            O2GTimeframe timeframe = factory.Timeframes[sTimeframe];
            if (timeframe == null)
            {
                throw new Exception(string.Format("Timeframe '{0}' is incorrect!", sTimeframe));
            }
            O2GRequest request = factory.createMarketDataSnapshotRequestInstrument(sInstrument, timeframe, 300);
            DateTime dtFirst = dtTo;
            do // cause there is limit for returned candles amount
            {
                factory.fillMarketDataSnapshotRequestTime(request, dtFrom, dtFirst, false);
                responseListener.SetRequestID(request.RequestID);
                session.sendRequest(request);
                if (!responseListener.WaitEvents())
                {
                    throw new Exception("Response waiting timeout expired");
                }
                // shift "to" bound to oldest datetime of returned data
                O2GResponse response = responseListener.GetResponse();
                if (response != null && response.Type == O2GResponseType.MarketDataSnapshot)
                {
                    O2GResponseReaderFactory readerFactory = session.getResponseReaderFactory();
                    if (readerFactory != null)
                    {
                        O2GMarketDataSnapshotResponseReader reader = readerFactory.createMarketDataSnapshotReader(response);
                        if (reader.Count > 0)
                        {
                            if (DateTime.Compare(dtFirst, reader.getDate(0)) != 0)
                            {
                                dtFirst = reader.getDate(0); // earliest datetime of returned data
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("0 rows received");
                            break;
                        }
                    }
                    PrintPrices(session, response);
                }
                else
                {
                    break;
                }
            } while (dtFirst > dtFrom);
        }

        private void disconnectBt_Click(object sender, EventArgs e)
        {
            mSession.logout();
            mSession.unsubscribeSessionStatus(statusListener);
            mSession.Dispose();
        }

        private void getHistoryDataBt_Click(object sender, EventArgs e)
        {
            O2GSession session = null;

            //try
            //{
                LoginParams loginParams = new LoginParams(ConfigurationManager.AppSettings);
                SampleParams sampleParams = new SampleParams(ConfigurationManager.AppSettings);

                PrintSampleParams("GetHistPrices", loginParams, sampleParams);

                session = O2GTransport.createSession();
                SessionStatusListener statusListener = new SessionStatusListener(session, loginParams.SessionID, loginParams.Pin);
                session.subscribeSessionStatus(statusListener);
                statusListener.Reset();
                session.login(loginParams.Login, loginParams.Password, loginParams.URL, loginParams.Connection);
                if (statusListener.WaitEvents() && statusListener.Connected)
                {
                    ResponseListener responseListener = new ResponseListener(session);
                    session.subscribeResponse(responseListener);
                    GetHistoryPrices(session, sampleParams.Instrument, sampleParams.Timeframe, sampleParams.DateFrom, sampleParams.DateTo, responseListener);
                    Console.WriteLine("Done!");

                    statusListener.Reset();
                    session.logout();
                    statusListener.WaitEvents();
                    session.unsubscribeResponse(responseListener);
                }
                session.unsubscribeSessionStatus(statusListener);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Exception: {0}", e.ToString());
            //}
            //finally
            //{
            //    if (session != null)
            //    {
            //        session.Dispose();
            //    }
            //}


        }

       
        private static void PrintSampleParams(string procName, LoginParams loginPrm, SampleParams prm)
        {
            Console.WriteLine("{0}: Instrument='{1}', Timeframe='{2}', DateFrom='{3}', DateTo='{4}'", procName, prm.Instrument, prm.Timeframe, prm.DateFrom.ToString("MM.dd.yyyy HH:mm:ss"), prm.DateTo.ToString("MM.dd.yyyy HH:mm:ss"));
        }

    }
}
