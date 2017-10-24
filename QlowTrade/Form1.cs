using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fxcore2;
using System.Threading;


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
         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectBt_Click(object sender, EventArgs e)
        {
            mSession = O2GTransport.createSession();
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
                mSession.logout();
            }
            mSession.unsubscribeSessionStatus(statusListener);
            mSession.Dispose();
            
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
        private void disconnectBt_Click(object sender, EventArgs e)
        {
           
        }
    }
}
