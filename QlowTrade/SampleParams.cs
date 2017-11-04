using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;

namespace QlowTrade
{
    class SampleParams
    {
        public string Instrument
        {
            get
            {
                return mInstrument;
            }
        }
        private string mInstrument;

        public string Timeframe
        {
            get
            {
                return mTimeframe;
            }
        }
        private string mTimeframe;

        public DateTime DateFrom
        {
            get
            {
                return mDateFrom;
            }
        }
        private DateTime mDateFrom;

        public DateTime DateTo
        {
            get
            {
                return mDateTo;
            }
        }
        private DateTime mDateTo;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="args"></param>
        public SampleParams(NameValueCollection args)
        {
            string sDateFormat = "MM/dd/yyyy/HH:mm:ss";
            mInstrument = GetRequiredArgument(args, "Instrument");
            mTimeframe = GetRequiredArgument(args, "Timeframe");

            string sDateFrom = args["DateFrom"];
            bool bIsDateFromNotSpecified = false;
            if (!DateTime.TryParseExact(sDateFrom, sDateFormat, CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out mDateFrom))
            {
                bIsDateFromNotSpecified = true;
             //   mDateFrom = DateTime.FromOADate(0); // ZERODATE
            }
            else
            {
                if (DateTime.Compare(mDateFrom, DateTime.UtcNow) >= 0)
                {
                    throw new Exception(string.Format("\"DateFrom\" value {0} is invalid; please fix the value in the configuration file", sDateFrom));
                }
            }

            string sDateTo = args["DateTo"];
            bool bIsDateToNotSpecified = false;
            if (!DateTime.TryParseExact(sDateTo, sDateFormat, CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out mDateTo))
            {
                bIsDateToNotSpecified = true;
          //      mDateTo = DateTime.FromOADate(0); // ZERODATE
            }
            else
            {
                if (bIsDateFromNotSpecified && DateTime.Compare(mDateFrom, mDateTo) >= 0)
                {
                    throw new Exception(string.Format("\"DateTo\" value {0} is invalid; please fix the value in the configuration file", sDateTo));
                }
            }
        }

        private string GetRequiredArgument(NameValueCollection args, string sArgumentName)
        {
            string sArgument = args[sArgumentName];
            if (!string.IsNullOrEmpty(sArgument))
            {
                sArgument = sArgument.Trim();
            }
            if (string.IsNullOrEmpty(sArgument))
            {
                throw new Exception(string.Format("Please provide {0} in configuration file", sArgumentName));
            }
            return sArgument;
        }
    }
    }
