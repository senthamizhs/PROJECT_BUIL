using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;


namespace RetrievePNRs
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dtResponse = new DataTable();
            string Response = "";
            string Error = "";
            //GetBookingResponse
            string[] str = Directory.GetFiles(@"C:\Users\prasanna\Desktop\6E\", "*.xml", SearchOption.AllDirectories);
            foreach (string a in str)
            {
                dtResponse = new DataTable();
                if (!a.Contains("G8RetrievePNRResponse"))
                {
                    continue;
                }
                //string ab = @"C:\Users\prasanna\Desktop\ReScheduleGetBooking.xml";
                Response = File.ReadAllText(a);
                int abc = 0;
                DataTable dtFlights = new DataTable();
                ParsePnrRetrive("", "", null, Response, "", ref dtFlights, ref Error);
                //ParseRetrivePNR("", "", Response, ref dtResponse, ref Error);
                //ParseBooking("", "", "", Response, ref Error, ref abc, ref dtResponse, ref Error);
                //ParseBookingNew("", "", "", Response, ref Error, ref abc, ref dtResponse, ref Error);
            }
        }
        private static bool ParseBooking(string strSequence, string strUserTrackID, string Office, string strResponse, ref string PNR, ref int BookingInd,
                ref DataTable dtBookResponse, ref string strError)
        {
            try
            {
                XNamespace xns = "http://www.opentravel.org/OTA/2003/05";
                XNamespace xns2 = "http://www.isaaviation.com/thinair/webservices/OTA/Extensions/2003/05";
                if (XDocument.Parse(strResponse).Descendants(xns + "Errors").Count() > 0 && XDocument.Parse(strResponse).Descendants(xns + "Errors").Elements().Count() > 0)
                {
                    var Error = from Errors in XDocument.Parse(strResponse).Descendants(xns + "Errors")
                                select new
                                {
                                    Code = Errors.Element(xns + "Error").Attribute("Code").Value,
                                    ShortText = Errors.Element(xns + "Error").Attribute("ShortText").Value,
                                    Type = Errors.Element(xns + "Error").Attribute("Type").Value
                                };
                    if (Error.Count() > 0)
                    {
                        BookingInd = 0;
                        //strError = Error.ToArray()[0].Code.Equals("167") ? "No fare available. Please refine your search" :
                        //            (Error.ToArray()[0].Code.Equals("29") ? "Supplier balance for " + hshCredential["OFFICEID"].ToString() + "(" + ItenaryRQ.ItineraryFlights[0].Stock + ") is low, Please TopUp supplier balance." : "Unable to sell segment");
                        return false;
                    }
                }



                //var result = from OTA_AirBookRS in XDocument.Parse(strResponse).Descendants(xns + "OTA_AirBookRS")
                //             from FlightSegment in OTA_AirBookRS.Descendants(xns + "FlightSegment")
                //             from PTC_FareBreakdowns in OTA_AirBookRS.Descendants(xns + "PTC_FareBreakdowns")
                //             from TravelerInfo in OTA_AirBookRS.Descendants(xns + "TravelerInfo")
                //             from AirTraveler in TravelerInfo.Descendants(xns + "AirTraveler")
                //             from SpecialReqDetails in TravelerInfo.Elements(xns + "SpecialReqDetails").Count() == 0 ? TravelerInfo.Descendants(xns + "AirTraveler") : TravelerInfo.Descendants(xns + "SpecialReqDetails")
                //             select new
                //             {
                //                 //AIRLINECATEGORY = "LCC",
                //                 //SPECIALFARE = "N",
                //                 //AIRLINECODE = FlightSegment.Attribute("FlightNumber").Value.Substring(0, 2),
                //                 AIRLINEPNR = OTA_AirBookRS.Element(xns + "AirReservation").Element(xns + "BookingReferenceID").Attribute("ID").Value,

                //                 //DEPARTUREDATE = DateTime.ParseExact(FlightSegment.Attribute("ArrivalDateTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy"),
                //                 //ARRIVALDATE = DateTime.ParseExact(FlightSegment.Attribute("DepartureDateTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy"),
                //                 //ARRIVALTIME = DateTime.ParseExact(FlightSegment.Attribute("ArrivalDateTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("HH:mm"),
                //                 //DEPARTURETIME = DateTime.ParseExact(FlightSegment.Attribute("DepartureDateTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("HH:mm"),

                //                 DEPARTUREDATE = FlightSegment.Attribute("ArrivalDateTime").Value,
                //                 ARRIVALDATE = FlightSegment.Attribute("DepartureDateTime").Value,
                //                 ARRIVALTIME = FlightSegment.Attribute("ArrivalDateTime").Value,
                //                 DEPARTURETIME = FlightSegment.Attribute("DepartureDateTime").Value,

                //                 //CRSPNR = "N/A",
                //                 FLIGHTNO = FlightSegment.Attribute("FlightNumber").Value.Replace("G9", "").Replace("3O", "").Trim(),
                //                 ORIGIN = FlightSegment.Element(xns + "DepartureAirport").Attribute("LocationCode").Value,
                //                 DESTINATION = FlightSegment.Element(xns + "ArrivalAirport").Attribute("LocationCode").Value,
                //                 //DATEOFBIRTH = "",
                //                 //FQTV = "",
                //                 FIRSTNAME = AirTraveler.Element(xns + "PersonName").Element(xns + "GivenName").Value,
                //                 TITLE = (AirTraveler.Attribute("PassengerTypeCode").Value.Equals("INF")) ? "Mstr" : AirTraveler.Element(xns + "PersonName").Element(xns + "NameTitle").Value,
                //                 LASTNAME = AirTraveler.Element(xns + "PersonName").Element(xns + "Surname").Value,


                //                 //ITINREF = (from ETicketInfo in AirTraveler.Element(xns + "ETicketInfo").Elements().AsEnumerable()
                //                 //           where ETicketInfo.Attribute("flightSegmentRPH").Value.Equals(FlightSegment.Attribute("RPH").Value.Split('$').ToArray()[2])
                //                 //           select ETicketInfo.Attribute("couponNo").Value).ToArray()[0],

                //                 //           MEALS = (TravelerInfo.Element(xns + "SpecialReqDetails") != null &&
                //                 //        TravelerInfo.Elements(xns + "SpecialReqDetails").Count() > 0 &&
                //                 //        TravelerInfo.Element(xns + "SpecialReqDetails").Elements(xns + "MealRequests").Count() > 0) ?
                //                 //        (from Ml in SpecialReqDetails.Element(xns + "MealRequests").Elements(xns + "MealRequest").AsEnumerable()
                //                 //         where FlightSegment.Attribute("FlightNumber").Value.Equals(Ml.Attribute("FlightNumber").Value) &&
                //                 //         AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(Ml.Attribute("TravelerRefNumberRPHList").Value)
                //                 //         select Ml.Attribute("mealCode").Value).ToArray().FirstOrDefault() : null,

                //                 //BAGGAGE = (TravelerInfo.Element(xns + "SpecialReqDetails") != null &&
                //                 //           TravelerInfo.Elements(xns + "SpecialReqDetails").Count() > 0 &&
                //                 //           TravelerInfo.Element(xns + "SpecialReqDetails").Elements(xns + "BaggageRequests").Count() > 0) ?
                //                 //           (from Bg in SpecialReqDetails.Element(xns + "BaggageRequests").Elements(xns + "BaggageRequest").AsEnumerable()
                //                 //            where FlightSegment.Attribute("FlightNumber").Value.Equals(Bg.Attribute("FlightNumber").Value) &&
                //                 //            AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(Bg.Attribute("TravelerRefNumberRPHList").Value)
                //                 //            select Bg.Attribute("baggageCode").Value).ToArray().FirstOrDefault() : null,
                //                 //OFFICEID = Office,
                //                 //PAXTYPE = AirTraveler.Attribute("PassengerTypeCode").Value,
                //                 //REFERENCE = "",
                //                 //SEQNO = (AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Split('|')[1]).Substring(1, 1),
                //                 //SEQNO = TravelerInfo.Elements().ToList().IndexOf(AirTraveler) + 1,
                //                 //TICKETINGCARRIER = FlightSegment.Attribute("FlightNumber").Value.Substring(0, 2),
                //                 //TICKETNO = (from ETicketInfo in AirTraveler.Element(xns + "ETicketInfo").Elements().AsEnumerable()
                //                 //            where ETicketInfo.Attribute("flightSegmentRPH").Value.Equals(FlightSegment.Attribute("RPH").Value)
                //                 //            select ETicketInfo.Attribute("eTicketNo").Value).ToArray()[0],
                //                 //TICKETNO = (from ETicketInfo in AirTraveler.Element(xns + "ETicketInfo").Elements().AsEnumerable()
                //                 //            where ETicketInfo.Attribute("flightSegmentRPH").Value.Equals(FlightSegment.Attribute("RPH").Value.Split('$').ToArray()[2])
                //                 //            select ETicketInfo.Attribute("eTicketNo").Value).ToArray()[0],
                //                 //TRIPTYPE = OTA_AirBookRS.Elements(xns + "OriginDestinationOptions").Count() > 2 ? "M" : (OTA_AirBookRS.Elements(xns + "OriginDestinationOptions").Count() > 1 ? "R" : "O"),
                //                 //USERTRACKID = strUserTrackID,

                //                 //CLASS = FlightSegment.Attribute("ResCabinClass").Value,

                //                 //FAREBASISCODE = PTC_FareBreakdowns.Element(xns + "PTC_FareBreakdown").Element(xns + "FareBasisCodes").Element(xns + "FareBasisCode").Value.Equals("P") ? "Promo" : PTC_FareBreakdowns.Element(xns + "PTC_FareBreakdown").Element(xns + "FareBasisCodes").Element(xns + "FareBasisCode").Value,
                //                 //PTC_FareBreakdowns.Element(xns + "PTC_FareBreakdown").Element(xns + "FareBasisCodes").Element(xns + "FareBasisCode").Value,
                //                 //OFFLINEFLAG = "0",

                //                 //TOKEN = FlightSegment.Element(xns + "Comment").Value,

                //                 //BASEAMT = (from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                //                 //           where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                //                 //           select PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "BaseFare").Attribute("Amount").Value).ToArray()[0],

                //                 //GROSSAMT = (from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                //                 //            where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value
                //                 //            .Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                //                 //            select Convert.ToDouble(PTC_FareBreakdown.Element(xns + "PassengerFare")
                //                 //            .Element(xns + "TotalFare").Attribute("Amount").Value)).ToArray()[0] - 
                //                 //            (from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                //                 //                                                                                                                                                            where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                //                 //                                                                                                                                                            select PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Fees").Elements(xns + "Fee").Where(mlBg => mlBg.Attribute("FeeCode").Value.Equals("ML") || mlBg.Attribute("FeeCode").Value.Equals("BG") || mlBg.Value.Contains("Baggage") || mlBg.Value.Contains("Meal") || mlBg.Value.Contains("Seat")).Select(Tax => Convert.ToDouble(Tax.Attribute("Amount").Value)).ToArray().Sum()).ToArray()[0],
                //                 //TOTALTAXAMT = (Convert.ToDouble((from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                //                 //                                 where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                //                 //                                 select PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "TotalFare").Attribute("Amount").Value).ToArray()[0]) - Convert.ToDouble((from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                //                 //                                                                                                                                                                                 where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                //                 //                                                                                                                                                                                 select PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "BaseFare").Attribute("Amount").Value).ToArray()[0])
                //                 //                                                                                                                                                                                 - (from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                //                 //                                                                                                                                                                                    where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                //                 //                                                                                                                                                                                    select PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Fees").Elements(xns + "Fee").Where(mlBg => mlBg.Attribute("FeeCode").Value.Equals("ML") || mlBg.Attribute("FeeCode").Value.Equals("BG") || mlBg.Value.Contains("Baggage") || mlBg.Value.Contains("Meal") || mlBg.Value.Contains("Seat")).Select(Tax => Convert.ToDouble(Tax.Attribute("Amount").Value)).ToArray().Sum()).ToArray()[0]),

                //                 //TAXBREAKUP = (from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                //                 //              where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value
                //                 //                    .Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                //                 //              select string.Join("/",
                //                 //                               (PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Taxes").Elements(xns + "Tax").Where(mlBg => !mlBg.Attribute("TaxCode").Value.Equals("ML") && !mlBg.Attribute("TaxCode").Value.Equals("BG") && !mlBg.Value.Contains("Baggage") && !mlBg.Value.Contains("Meal") && !mlBg.Value.Contains("Seat")).Select(Tax => Tax.Attribute("TaxCode").Value + ":" + Tax.Attribute("Amount").Value).ToArray()))
                //                 //                               + "/" +
                //                 //                               string.Join("/", (PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Fees").Elements(xns + "Fee").Where(mlBg => !mlBg.Attribute("FeeCode").Value.Equals("ML") && !mlBg.Attribute("FeeCode").Value.Equals("BG") && !mlBg.Value.Contains("Baggage") && !mlBg.Value.Contains("Meal") && !mlBg.Value.Contains("Seat")).Select(Tax => Tax.Attribute("FeeCode").Value.Split('/')[0].Trim() + ":" + Tax.Attribute("Amount").Value).ToArray())
                //                 //                               )).ToArray()[0],
                //                 //SUPPLIERPNR = ""
                //             };

                var result = from OTA_AirBookRS in XDocument.Parse(strResponse).Descendants(xns + "OTA_AirBookRS")
                             from AirReservation in OTA_AirBookRS.Descendants(xns + "AirReservation")
                             from OriginDestinationOptions in AirReservation.Descendants(xns + "OriginDestinationOptions")
                             from OriginDestinationOption in OriginDestinationOptions.Descendants(xns + "OriginDestinationOption")

                             from FlightSegment in OriginDestinationOption.Descendants(xns + "FlightSegment")
                             from TravelerInfo in AirReservation.Descendants(xns + "TravelerInfo")
                             from AirTraveler in TravelerInfo.Descendants(xns + "AirTraveler")
                             from ETicketInfo in AirTraveler.Descendants(xns + "ETicketInfo")
                             from ETicketInfomation in ETicketInfo.Descendants(xns + "ETicketInfomation")
                             where FlightSegment.Attribute("RPH").Value.Contains(ETicketInfomation.Attribute("flightSegmentCode").Value + "$")
                             && FlightSegment.Attribute("RPH").Value.Contains(ETicketInfomation.Attribute("flightSegmentRPH").Value + "$")

                             select new
                             {

                                 ItinRef = OriginDestinationOptions.Descendants(xns + "OriginDestinationOption").ToList().IndexOf(OriginDestinationOption),
                                 SegRef = OriginDestinationOptions.Descendants(xns + "FlightSegment").ToList().IndexOf(FlightSegment),

                                 SegmentRef = OriginDestinationOption.Descendants(xns + "FlightSegment").ToList().IndexOf(FlightSegment),

                                 eTicketNo = ETicketInfomation.Attribute("eTicketNo").Value,
                                 usedStatus = ETicketInfomation.Attribute("usedStatus").Value,
                                 status = ETicketInfomation.Attribute("status").Value,
                                 couponNo = ETicketInfomation.Attribute("couponNo").Value,

                                 ArrivalDateTime = FlightSegment.Attribute("ArrivalDateTime").Value,
                                 DepartureDateTime = FlightSegment.Attribute("DepartureDateTime").Value,
                                 //ARRIVALTIME = FlightSegment.Attribute("ArrivalDateTime").Value,
                                 //DEPARTURETIME = FlightSegment.Attribute("DepartureDateTime").Value,
                                 BookingReferenceID = OTA_AirBookRS.Element(xns + "AirReservation").Element(xns + "BookingReferenceID").Attribute("ID").Value,

                                 DepartureAirport = FlightSegment.Element(xns + "DepartureAirport").Attribute("LocationCode").Value,
                                 ArrivalAirport = FlightSegment.Element(xns + "ArrivalAirport").Attribute("LocationCode").Value,

                                 GivenName = AirTraveler.Element(xns + "PersonName").Element(xns + "GivenName").Value,
                                 Surname = AirTraveler.Element(xns + "PersonName").Element(xns + "Surname").Value,
                                 NameTitle = (AirTraveler.Attribute("PassengerTypeCode").Value.Equals("INF"))
                                          ? "Mstr" : AirTraveler.Element(xns + "PersonName").Element(xns + "NameTitle").Value,

                                 AirlineCode = FlightSegment.Attribute("FlightNumber").Value.Substring(0, 2),
                                 FlightNumber = FlightSegment.Attribute("FlightNumber").Value.Substring(2),


                                 Status = FlightSegment.Attribute("Status").Value,
                                 ResCabinClass = FlightSegment.Attribute("ResCabinClass").Value,

                                 OrgTerminal = FlightSegment.Element(xns + "DepartureAirport").Attribute("Terminal").Value,
                                 DestTerminal = FlightSegment.Element(xns + "ArrivalAirport").Attribute("Terminal").Value,

                                 Comment = FlightSegment.Element(xns + "Comment").Value

                                 //<ns1:DepartureAirport Terminal="TerminalX" LocationCode="MAA"/><ns1:ArrivalAirport Terminal="TerminalX" LocationCode="SHJ"/>
                                 //<ns1:Comment>airport_short_names:MAA=Chenna,SHJ=Sharja</ns1:Comment>
                             };

                dtBookResponse = new DataTable();
                dtBookResponse = ConvertToDataTable(result);

                var SpecialServices = from OTA_AirBookRS in XDocument.Parse(strResponse).Descendants(xns + "OTA_AirBookRS")
                                      from AirReservation in OTA_AirBookRS.Descendants(xns + "AirReservation")
                                      from TravelerInfo in AirReservation.Descendants(xns + "TravelerInfo")
                                      from SpecialReqDetails in TravelerInfo.Descendants(xns + "SpecialReqDetails")
                                      from SeatRequests in SpecialReqDetails.Descendants(xns + "SeatRequests")
                                      from SeatRequest in SeatRequests.Descendants(xns + "SeatRequest")
                                      //from BaggageRequests in SpecialReqDetails.Descendants(xns + "BaggageRequests")
                                      //from BaggageRequest in BaggageRequests.Descendants(xns + "BaggageRequest")
                                      select new
                                      {
                                          SRDepartureDate = SeatRequest.Attribute("DepartureDate").Value,
                                          SRFlightNumber = SeatRequest.Attribute("FlightNumber").Value,
                                          SRFlightRefNumberRPHList = SeatRequest.Attribute("FlightRefNumberRPHList").Value,
                                          SRTravelerRefNumberRPHList = SeatRequest.Attribute("TravelerRefNumberRPHList").Value,

                                          //BGDepartureDate = BaggageRequest.Attribute("DepartureDate").Value,
                                          //BGFlightNumber = BaggageRequest.Attribute("FlightNumber").Value,
                                          //BGFlightRefNumberRPHList = BaggageRequest.Attribute("FlightRefNumberRPHList").Value,
                                          //BGTravelerRefNumberRPHList = BaggageRequest.Attribute("TravelerRefNumberRPHList").Value,

                                          //BGBaggageCode = BaggageRequest.Attribute("baggageCode").Value,

                                      };
                DataTable dtSSR = new DataTable();
                dtSSR = ConvertToDataTable(SpecialServices);

                var SpecialServicesBG = from OTA_AirBookRS in XDocument.Parse(strResponse).Descendants(xns + "OTA_AirBookRS")
                                        from AirReservation in OTA_AirBookRS.Descendants(xns + "AirReservation")
                                        from TravelerInfo in AirReservation.Descendants(xns + "TravelerInfo")
                                        from SpecialReqDetails in TravelerInfo.Descendants(xns + "SpecialReqDetails")
                                        //from SeatRequests in SpecialReqDetails.Descendants(xns + "SeatRequests")
                                        //from SeatRequest in SeatRequests.Descendants(xns + "SeatRequest")
                                        from BaggageRequests in SpecialReqDetails.Descendants(xns + "BaggageRequests")
                                        from BaggageRequest in BaggageRequests.Descendants(xns + "BaggageRequest")
                                        select new
                                        {
                                            //SRDepartureDate = SeatRequest.Attribute("DepartureDate").Value,
                                            //SRFlightNumber = SeatRequest.Attribute("FlightNumber").Value,
                                            //SRFlightRefNumberRPHList = SeatRequest.Attribute("FlightRefNumberRPHList").Value,
                                            //SRTravelerRefNumberRPHList = SeatRequest.Attribute("TravelerRefNumberRPHList").Value,

                                            BGDepartureDate = BaggageRequest.Attribute("DepartureDate").Value,
                                            BGFlightNumber = BaggageRequest.Attribute("FlightNumber").Value,
                                            BGFlightRefNumberRPHList = BaggageRequest.Attribute("FlightRefNumberRPHList").Value,
                                            BGTravelerRefNumberRPHList = BaggageRequest.Attribute("TravelerRefNumberRPHList").Value,

                                            BGBaggageCode = BaggageRequest.Attribute("baggageCode").Value,

                                        };
                DataTable dtBGSSR = new DataTable();
                dtBGSSR = ConvertToDataTable(SpecialServicesBG);


                var Payments = from OTA_AirBookRS in XDocument.Parse(strResponse).Descendants(xns + "OTA_AirBookRS")
                               from AirReservation in OTA_AirBookRS.Descendants(xns + "AirReservation")
                               from Fulfillment in AirReservation.Descendants(xns + "Fulfillment")
                               from PaymentDetails in Fulfillment.Descendants(xns + "PaymentDetails")
                               from PaymentDetail in PaymentDetails.Descendants(xns + "PaymentDetail")
                               select new
                               {
                                   CompanyName = PaymentDetail.Element(xns + "DirectBill").Element(xns + "CompanyName").Attribute("Code").Value,
                                   Amount = PaymentDetail.Element(xns + "PaymentAmount").Attribute("Amount").Value,
                                   AmountCurr = PaymentDetail.Element(xns + "PaymentAmount").Attribute("CurrencyCode").Value,
                                   AmountPay = PaymentDetail.Element(xns + "PaymentAmountInPayCur").Attribute("Amount").Value,
                                   AmountPayCurr = PaymentDetail.Element(xns + "PaymentAmountInPayCur").Attribute("CurrencyCode").Value,
                                   TicketType = AirReservation.Element(xns + "Ticketing").Attribute("TicketingStatus").Value
                               };
                DataTable dtPayment = new DataTable();
                dtPayment = ConvertToDataTable(Payments);

                var PriceDetails = from OTA_AirBookRS in XDocument.Parse(strResponse).Descendants(xns + "OTA_AirBookRS")
                                   from AirReservation in OTA_AirBookRS.Descendants(xns + "AirReservation")
                                   from PriceInfo in AirReservation.Descendants(xns + "PriceInfo")
                                   from ItinTotalFare in PriceInfo.Descendants(xns + "ItinTotalFare")
                                   select new
                                   {
                                       BaseFare = ItinTotalFare.Element(xns + "BaseFare").Attribute("Amount").Value,
                                       BaseFareCurr = ItinTotalFare.Element(xns + "BaseFare").Attribute("CurrencyCode").Value,

                                       TotalFare = ItinTotalFare.Element(xns + "TotalFare").Attribute("Amount").Value,
                                       TotalFareCurr = ItinTotalFare.Element(xns + "TotalFare").Attribute("CurrencyCode").Value,

                                       TotalEquivFare = ItinTotalFare.Element(xns + "TotalEquivFare").Attribute("Amount").Value,
                                       TotalEquivFareCurr = ItinTotalFare.Element(xns + "TotalEquivFare").Attribute("CurrencyCode").Value,

                                       Taxes = ItinTotalFare.Elements(xns + "Taxes").Elements(xns + "Tax")
                                               .Select(P => Convert.ToDecimal(P.Attribute("Amount").Value)).Sum(),


                                       TaxesCurr = ItinTotalFare.Element(xns + "Taxes").Element(xns + "Tax").Attribute("CurrencyCode").Value,

                                       Fees = ItinTotalFare.Elements(xns + "Fees").Elements(xns + "Fee")
                                               .Select(P => Convert.ToDecimal(P.Attribute("Amount").Value)).Sum(),

                                       FeeCurr = ItinTotalFare.Element(xns + "Fees").Element(xns + "Fee").Attribute("CurrencyCode").Value,


                                   };
                DataTable dtPricement = new DataTable();
                dtPricement = ConvertToDataTable(PriceDetails);
                string[] str_Excludes = new string[] { "Baggage Selection", "JN Tax for Ancillaries" };
                var PriceDetailsNew = from OTA_AirBookRS in XDocument.Parse(strResponse).Descendants(xns + "OTA_AirBookRS")
                                      from AirReservation in OTA_AirBookRS.Descendants(xns + "AirReservation")
                                      from PriceInfo in AirReservation.Descendants(xns + "PriceInfo")
                                      from PTC_FareBreakdowns in PriceInfo.Descendants(xns + "PTC_FareBreakdowns")
                                      from PTC_FareBreakdown in PriceInfo.Descendants(xns + "PTC_FareBreakdown")
                                      select new
                                      {
                                          //Quantity Code
                                          PassengerTypeQuantity = PTC_FareBreakdown.Element(xns + "PassengerTypeQuantity").Attribute("Quantity").Value,
                                          PassengerTypeCode = PTC_FareBreakdown.Element(xns + "PassengerTypeQuantity").Attribute("Code").Value,
                                          TravelerRefNumber = PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value,

                                          FareBasisCode = PTC_FareBreakdown.Element(xns + "FareBasisCodes").Element(xns + "FareBasisCode").Value,

                                          BaseFare = PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "BaseFare").Attribute("Amount").Value,

                                          Tax = PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Taxes").Elements(xns + "Tax")
                                                .Select(P => Convert.ToDecimal(P.Attribute("Amount").Value)).Sum(),

                                          TaxExcludes = PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Taxes").Elements(xns + "Tax")
                                                .Where(P => !str_Excludes.Contains(P.Attribute("TaxName").Value))
                                                .Select(P => Convert.ToDecimal(P.Attribute("Amount").Value)).Sum(),

                                          TaxBreakExcludes = string.Join("/", PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Taxes").Elements(xns + "Tax")
                                                            .Where(P => !str_Excludes.Contains(P.Attribute("TaxName").Value))
                                                            .Select(P => P.Attribute("TaxName").Value + ":" + P.Attribute("Amount").Value).ToArray()),


                                          TaxBrakup = string.Join("/", PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Taxes").Elements(xns + "Tax")
                                                            .Select(P => P.Attribute("TaxName").Value + ":" + P.Attribute("Amount").Value).ToArray()),

                                          Fee = PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Fees").Elements(xns + "Fee")
                                                .Select(P => Convert.ToDecimal(P.Attribute("Amount").Value)).Sum(),

                                          FeeBrakup = string.Join("/", PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Fees").Elements(xns + "Fee")
                                                      .Select(P => P.Value + ":" + P.Attribute("Amount").Value).ToArray()),

                                          FeeExclude = PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Fees").Elements(xns + "Fee")
                                                      .Where(P => !str_Excludes.Contains(P.Value))
                                                      .Select(P => Convert.ToDecimal(P.Attribute("Amount").Value)).Sum(),

                                          FeeBrakupExclude = string.Join("/", PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Fees").Elements(xns + "Fee")
                                                      .Where(P => !str_Excludes.Contains(P.Value))
                                                      .Select(P => P.Value + ":" + P.Attribute("Amount").Value).ToArray()),

                                          TotalFare = PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "TotalFare").Attribute("Amount").Value,
                                      };

                DataTable dtPriceDetails = ConvertToDataTable(PriceDetailsNew);

                var PassengerDetails = from OTA_AirBookRS in XDocument.Parse(strResponse).Descendants(xns + "OTA_AirBookRS")
                                       from AirReservation in OTA_AirBookRS.Descendants(xns + "AirReservation")
                                       from TPA_Extensions in AirReservation.Descendants(xns + "TPA_Extensions")
                                       from AAAirReservationExt in TPA_Extensions.Descendants(xns2 + "AAAirReservationExt")
                                       from ResSummary in AAAirReservationExt.Descendants(xns2 + "ResSummary")
                                       from PTCCounts in ResSummary.Descendants(xns2 + "PTCCounts")
                                       //from PTCCount in PTCCounts.Descendants(xns2 + "PTCCount")
                                       //ResSummary
                                       select new
                                       {
                                           ADT = PTCCounts.Elements(xns2 + "PTCCount").Where(P => P.Element(xns2 + "PassengerTypeCode").Value == "ADT")
                                                 .Select(P => Convert.ToDecimal(P.Element(xns2 + "PassengerTypeQuantity").Value)).Sum(),

                                           CHD = PTCCounts.Elements(xns2 + "PTCCount").Where(P => P.Element(xns2 + "PassengerTypeCode").Value == "CHD")
                                           .Select(P => Convert.ToDecimal(P.Element(xns2 + "PassengerTypeQuantity").Value)).Sum(),

                                           INF = PTCCounts.Elements(xns2 + "PTCCount").Where(P => P.Element(xns2 + "PassengerTypeCode").Value == "INF")
                                                 .Select(P => Convert.ToDecimal(P.Element(xns2 + "PassengerTypeQuantity").Value)).Sum()
                                       };

                DataTable dtPassengerDetails = ConvertToDataTable(PassengerDetails);

                var PTCdetails = from OTA_AirBookRS in XDocument.Parse(strResponse).Descendants(xns + "OTA_AirBookRS")
                                 from AirReservation in OTA_AirBookRS.Descendants(xns + "AirReservation")
                                 from TPA_Extensions in AirReservation.Descendants(xns + "TPA_Extensions")
                                 from AAAirReservationExt in TPA_Extensions.Descendants(xns2 + "AAAirReservationExt")
                                 from ResSummary in AAAirReservationExt.Descendants(xns2 + "ResSummary")
                                 from PTCCounts in ResSummary.Descendants(xns2 + "PTCCounts")
                                 //from PTCCount in PTCCounts.Descendants(xns2 + "PTCCount")
                                 //ResSummary
                                 select new
                                 {
                                     ADT = PTCCounts.Elements(xns2 + "PTCCount").Where(P => P.Element(xns2 + "PassengerTypeCode").Value == "ADT")
                                           .Select(P => Convert.ToDecimal(P.Element(xns2 + "PassengerTypeQuantity").Value)).Sum(),

                                     CHD = PTCCounts.Elements(xns2 + "PTCCount").Where(P => P.Element(xns2 + "PassengerTypeCode").Value == "CHD")
                                     .Select(P => Convert.ToDecimal(P.Element(xns2 + "PassengerTypeQuantity").Value)).Sum(),

                                     INF = PTCCounts.Elements(xns2 + "PTCCount").Where(P => P.Element(xns2 + "PassengerTypeCode").Value == "INF")
                                           .Select(P => Convert.ToDecimal(P.Element(xns2 + "PassengerTypeQuantity").Value)).Sum()
                                 };

                DataTable PassengerCount = ConvertToDataTable(PTCdetails);

                return !(dtBookResponse == null || dtBookResponse.Rows.Count == 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static bool ParseBookingNew(string strSequence, string strUserTrackID, string Office, string strResponse, ref string PNR, ref int BookingInd,
                ref DataTable dtBookResponse, ref string strError)
        {
            try
            {
                XNamespace xns = "http://www.opentravel.org/OTA/2003/05";
                if (XDocument.Parse(strResponse).Descendants(xns + "Errors").Count() > 0 && XDocument.Parse(strResponse).Descendants(xns + "Errors").Elements().Count() > 0)
                {
                    var Error = from Errors in XDocument.Parse(strResponse).Descendants(xns + "Errors")
                                select new
                                {
                                    Code = Errors.Element(xns + "Error").Attribute("Code").Value,
                                    ShortText = Errors.Element(xns + "Error").Attribute("ShortText").Value,
                                    Type = Errors.Element(xns + "Error").Attribute("Type").Value
                                };
                    if (Error.Count() > 0)
                    {
                        BookingInd = 0;
                        //strError = Error.ToArray()[0].Code.Equals("167") ? "No fare available. Please refine your search" :
                        //            (Error.ToArray()[0].Code.Equals("29") ? "Supplier balance for " + hshCredential["OFFICEID"].ToString() + "(" + ItenaryRQ.ItineraryFlights[0].Stock + ") is low, Please TopUp supplier balance." : "Unable to sell segment");
                        return false;
                    }
                }

                var result = from OTA_AirBookRS in XDocument.Parse(strResponse).Descendants(xns + "OTA_AirBookRS")
                             from FlightSegment in OTA_AirBookRS.Descendants(xns + "FlightSegment")
                             from PTC_FareBreakdowns in OTA_AirBookRS.Descendants(xns + "PTC_FareBreakdowns")
                             from TravelerInfo in OTA_AirBookRS.Descendants(xns + "TravelerInfo")
                             from AirTraveler in TravelerInfo.Descendants(xns + "AirTraveler")
                             from SpecialReqDetails in TravelerInfo.Elements(xns + "SpecialReqDetails").Count() == 0 ? TravelerInfo.Descendants(xns + "AirTraveler") : TravelerInfo.Descendants(xns + "SpecialReqDetails")
                             select new
                             {
                                 AIRLINECATEGORY = "LCC",
                                 SPECIALFARE = "N",
                                 AIRLINECODE = FlightSegment.Attribute("FlightNumber").Value.Substring(0, 2),
                                 AIRLINEPNR = OTA_AirBookRS.Element(xns + "AirReservation").Element(xns + "BookingReferenceID").Attribute("ID").Value,
                                 //DEPARTUREDATE = DateTime.ParseExact(FlightSegment.Attribute("ArrivalDateTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy"),
                                 //ARRIVALDATE = DateTime.ParseExact(FlightSegment.Attribute("DepartureDateTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy"),
                                 //ARRIVALTIME = DateTime.ParseExact(FlightSegment.Attribute("ArrivalDateTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("HH:mm"),
                                 //DEPARTURETIME = DateTime.ParseExact(FlightSegment.Attribute("DepartureDateTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("HH:mm"),

                                 DEPARTUREDATE = FlightSegment.Attribute("ArrivalDateTime").Value,
                                 ARRIVALDATE = FlightSegment.Attribute("DepartureDateTime").Value,
                                 //ARRIVALTIME = FlightSegment.Attribute("ArrivalDateTime").Value,
                                 //DEPARTURETIME = FlightSegment.Attribute("DepartureDateTime").Value,

                                 CRSPNR = "N/A",
                                 FLIGHTNO = FlightSegment.Attribute("FlightNumber").Value.Replace("G9", "").Replace("3O", "").Trim(),
                                 ORIGIN = FlightSegment.Element(xns + "DepartureAirport").Attribute("LocationCode").Value,
                                 DESTINATION = FlightSegment.Element(xns + "ArrivalAirport").Attribute("LocationCode").Value,
                                 DATEOFBIRTH = "",
                                 FQTV = "",
                                 FIRSTNAME = AirTraveler.Element(xns + "PersonName").Element(xns + "GivenName").Value,
                                 TITLE = (AirTraveler.Attribute("PassengerTypeCode").Value.Equals("INF")) ? "Mstr" : AirTraveler.Element(xns + "PersonName").Element(xns + "NameTitle").Value,
                                 LASTNAME = AirTraveler.Element(xns + "PersonName").Element(xns + "Surname").Value,
                                 ITINREF = (from ETicketInfo in AirTraveler.Element(xns + "ETicketInfo").Elements().AsEnumerable()
                                            where ETicketInfo.Attribute("flightSegmentRPH").Value.Equals(FlightSegment.Attribute("RPH").Value.Split('$').ToArray()[2])
                                            select ETicketInfo.Attribute("couponNo").Value).ToArray()[0],
                                 MEALS = (TravelerInfo.Element(xns + "SpecialReqDetails") != null &&
                                         TravelerInfo.Elements(xns + "SpecialReqDetails").Count() > 0 &&
                                         TravelerInfo.Element(xns + "SpecialReqDetails").Elements(xns + "MealRequests").Count() > 0) ?
                                         (from Ml in SpecialReqDetails.Element(xns + "MealRequests").Elements(xns + "MealRequest").AsEnumerable()
                                          where FlightSegment.Attribute("FlightNumber").Value.Equals(Ml.Attribute("FlightNumber").Value) &&
                                          AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(Ml.Attribute("TravelerRefNumberRPHList").Value)
                                          select Ml.Attribute("mealCode").Value).ToArray().FirstOrDefault() : null,

                                 BAGGAGE = (TravelerInfo.Element(xns + "SpecialReqDetails") != null &&
                                            TravelerInfo.Elements(xns + "SpecialReqDetails").Count() > 0 &&
                                            TravelerInfo.Element(xns + "SpecialReqDetails").Elements(xns + "BaggageRequests").Count() > 0) ?
                                            (from Bg in SpecialReqDetails.Element(xns + "BaggageRequests").Elements(xns + "BaggageRequest").AsEnumerable()
                                             where FlightSegment.Attribute("FlightNumber").Value.Equals(Bg.Attribute("FlightNumber").Value) &&
                                             AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(Bg.Attribute("TravelerRefNumberRPHList").Value)
                                             select Bg.Attribute("baggageCode").Value).ToArray().FirstOrDefault() : null,
                                 OFFICEID = Office,
                                 PAXTYPE = AirTraveler.Attribute("PassengerTypeCode").Value,
                                 REFERENCE = "",
                                 SEQNO = (AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Split('|')[1]).Substring(1, 1),
                                 //SEQNO = TravelerInfo.Elements().ToList().IndexOf(AirTraveler) + 1,
                                 TICKETINGCARRIER = FlightSegment.Attribute("FlightNumber").Value.Substring(0, 2),
                                 //TICKETNO = (from ETicketInfo in AirTraveler.Element(xns + "ETicketInfo").Elements().AsEnumerable()
                                 //            where ETicketInfo.Attribute("flightSegmentRPH").Value.Equals(FlightSegment.Attribute("RPH").Value)
                                 //            select ETicketInfo.Attribute("eTicketNo").Value).ToArray()[0],
                                 TICKETNO = (from ETicketInfo in AirTraveler.Element(xns + "ETicketInfo").Elements().AsEnumerable()
                                             where ETicketInfo.Attribute("flightSegmentRPH").Value.Equals(FlightSegment.Attribute("RPH").Value.Split('$').ToArray()[2])
                                             select ETicketInfo.Attribute("eTicketNo").Value).ToArray()[0],
                                 TRIPTYPE = OTA_AirBookRS.Elements(xns + "OriginDestinationOptions").Count() > 2 ? "M" : (OTA_AirBookRS.Elements(xns + "OriginDestinationOptions").Count() > 1 ? "R" : "O"),
                                 USERTRACKID = strUserTrackID,
                                 CLASS = FlightSegment.Attribute("ResCabinClass").Value,
                                 FAREBASISCODE = PTC_FareBreakdowns.Element(xns + "PTC_FareBreakdown").Element(xns + "FareBasisCodes").Element(xns + "FareBasisCode").Value.Equals("P") ? "Promo" : PTC_FareBreakdowns.Element(xns + "PTC_FareBreakdown").Element(xns + "FareBasisCodes").Element(xns + "FareBasisCode").Value,
                                 //PTC_FareBreakdowns.Element(xns + "PTC_FareBreakdown").Element(xns + "FareBasisCodes").Element(xns + "FareBasisCode").Value,
                                 OFFLINEFLAG = "0",

                                 TOKEN = FlightSegment.Element(xns + "Comment").Value,
                                 BASEAMT = (from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                                            where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                                            select PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "BaseFare").Attribute("Amount").Value).ToArray()[0],
                                 GROSSAMT = (from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                                             where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                                             select Convert.ToDouble(PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "TotalFare").Attribute("Amount").Value)).ToArray()[0] - (from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                                                                                                                                                                                             where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                                                                                                                                                                                             select PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Fees").Elements(xns + "Fee").Where(mlBg => mlBg.Attribute("FeeCode").Value.Equals("ML") || mlBg.Attribute("FeeCode").Value.Equals("BG") || mlBg.Value.Contains("Baggage") || mlBg.Value.Contains("Meal") || mlBg.Value.Contains("Seat")).Select(Tax => Convert.ToDouble(Tax.Attribute("Amount").Value)).ToArray().Sum()).ToArray()[0],
                                 TOTALTAXAMT = (Convert.ToDouble((from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                                                                  where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                                                                  select PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "TotalFare").Attribute("Amount").Value).ToArray()[0]) - Convert.ToDouble((from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                                                                                                                                                                                                                  where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                                                                                                                                                                                                                  select PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "BaseFare").Attribute("Amount").Value).ToArray()[0])
                                                                                                                                                                                                                  - (from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                                                                                                                                                                                                                     where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                                                                                                                                                                                                                     select PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Fees").Elements(xns + "Fee").Where(mlBg => mlBg.Attribute("FeeCode").Value.Equals("ML") || mlBg.Attribute("FeeCode").Value.Equals("BG") || mlBg.Value.Contains("Baggage") || mlBg.Value.Contains("Meal") || mlBg.Value.Contains("Seat")).Select(Tax => Convert.ToDouble(Tax.Attribute("Amount").Value)).ToArray().Sum()).ToArray()[0]),

                                 TAXBREAKUP = (from PTC_FareBreakdown in PTC_FareBreakdowns.Elements(xns + "PTC_FareBreakdown").AsEnumerable()
                                               where PTC_FareBreakdown.Element(xns + "TravelerRefNumber").Attribute("RPH").Value.Equals(AirTraveler.Element(xns + "TravelerRefNumber").Attribute("RPH").Value)
                                               select string.Join("/",
                                                                (PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Taxes").Elements(xns + "Tax").Where(mlBg => !mlBg.Attribute("TaxCode").Value.Equals("ML") && !mlBg.Attribute("TaxCode").Value.Equals("BG") && !mlBg.Value.Contains("Baggage") && !mlBg.Value.Contains("Meal") && !mlBg.Value.Contains("Seat")).Select(Tax => Tax.Attribute("TaxCode").Value + ":" + Tax.Attribute("Amount").Value).ToArray()))
                                                                + "/" +
                                                                string.Join("/", (PTC_FareBreakdown.Element(xns + "PassengerFare").Element(xns + "Fees").Elements(xns + "Fee").Where(mlBg => !mlBg.Attribute("FeeCode").Value.Equals("ML") && !mlBg.Attribute("FeeCode").Value.Equals("BG") && !mlBg.Value.Contains("Baggage") && !mlBg.Value.Contains("Meal") && !mlBg.Value.Contains("Seat")).Select(Tax => Tax.Attribute("FeeCode").Value.Split('/')[0].Trim() + ":" + Tax.Attribute("Amount").Value).ToArray())
                                                                )).ToArray()[0],
                                 SUPPLIERPNR = ""
                             };
                dtBookResponse = new DataTable();
                dtBookResponse = ConvertToDataTable(result);
                return !(dtBookResponse == null || dtBookResponse.Rows.Count == 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool ParseRetrivePNR(string Terminal, string lstrSequence, string Response, ref DataTable dtResponse, ref string Error)
        {
            try
            {
                string[] OrgDest = new string[]{"IXA","AGR","AMD","IXD","ATQ","IXU","IXB","BLR","BHU","BHO","BBI","BHJ","CCU","IXC","MAA","COK","CJB","NMB","DED","DIB",
                                           "DMU","DIU","GAU","GOI","GWL","HBX","HYD","IMF","IDR","JAI","IXJ","JGA","IXW","JDH","JRH","KNU","HJR","CCJ","IXL","LKO",
                                           "LUH","IXM","IXE","BOM","NAG","NDC","ISK","DEL","PAT","PNY","PNQ","PBD","IXZ","PUT","BEK","RAJ","IXR","SHL","IXS","SXR",
                                           "STV","TEZ","TRZ","TIR","TRV","UDR","BDQ","VNS","VGA","VTZ"};

                XNamespace xmlns = "http://schemas.navitaire.com/WebServices/ServiceContracts/BookingService";
                XNamespace a = "http://schemas.navitaire.com/WebServices/DataContracts/Booking";
                XNamespace b = "http://schemas.navitaire.com/WebServices/DataContracts/Common";
                XNamespace i = "http://www.w3.org/2001/XMLSchema-instance";

                //Leg count Logic

                #region CheckPayment & Status,Chnage Allowed
                var BookingDetails = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
                                     //from Pax in Booking.Element(a + "BookingInfo")
                                     select new
                                     {
                                         PnrStatus = Booking.Element(a + "BookingInfo").Element(a + "BookingStatus").Value,
                                         ChangeAllowed = Booking.Element(a + "BookingInfo").Element(a + "ChangeAllowed").Value,
                                         PaidStatus = Booking.Element(a + "BookingInfo").Element(a + "PaidStatus").Value,
                                         ChannelType = Booking.Element(a + "BookingInfo").Element(a + "ChannelType").Value,
                                         BookingDate = Booking.Element(a + "BookingInfo").Element(a + "BookingDate").Value,
                                         AgentCode = Booking.Element(a + "POS").Element(b + "AgentCode").Value,
                                         PaymentCount = Booking.Elements(a + "Payments").Elements(a + "Payment").Count(),
                                         PaymentAmount = Booking.Elements(a + "Payments").Elements(a + "Payment").Select(P => Convert.ToDecimal(P.Element(a + "PaymentAmount").Value)).ToArray().Sum(),
                                         PaxCount = Booking.Element(a + "PaxCount").Value,
                                         BalanceDue = Booking.Element(a + "BookingSum").Element(a + "BalanceDue").Value,
                                         SegmentCount = Booking.Element(a + "BookingSum").Element(a + "SegmentCount").Value,
                                         TotalCost = Booking.Element(a + "BookingSum").Element(a + "TotalCost").Value
                                     };
                DataTable dtFare = ConvertToDataTable(BookingDetails);

                #endregion

                #region
                var PassengerDetails = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
                                       from Passenger in Booking.Element(a + "Passengers").Elements(a + "Passenger")
                                       from PassengerFees in Passenger.Elements(a + "PassengerFees")
                                       select new
                                       {
                                           PaxType = Passenger.Element(a + "PassengerTypeInfos").Element(a + "PassengerTypeInfo").Element(a + "PaxType").Value,

                                           Title = Passenger.Element(a + "Names").Element(a + "BookingName").Element(a + "Title").Value.Equals("CHD")
                                                    ? (Passenger.Element(a + "PassengerInfo").Element(a + "Gender").Value.Equals("Male") ? "MSTR" : "MISS")
                                                    : Passenger.Element(a + "Names").Element(a + "BookingName").Element(a + "Title").Value,

                                           FirstName = Passenger.Element(a + "Names").Element(a + "BookingName").Element(a + "FirstName").Value,
                                           LastName = Passenger.Element(a + "Names").Element(a + "BookingName").Element(a + "LastName").Value,
                                           PassengerNumber = Passenger.Element(a + "PassengerNumber").Value,


                                           BalanceDue = Passenger.Element(a + "PassengerInfo").Element(a + "BalanceDue").Value,
                                           Gender = Passenger.Element(a + "PassengerInfo").Element(a + "Gender").Value,

                                           TotalCost = Passenger.Element(a + "PassengerInfo").Element(a + "TotalCost").Value,
                                           Infant = Passenger.Element(a + "Infant").Attribute(i + "nil") != null ? Passenger.Element(a + "Infant").Attribute(i + "nil").Value.ToUpper() : "FALSE",

                                           SSRAmount = PassengerFees.Elements(a + "PassengerFee").Where(Code => Code.Element(a + "FeeCode").Value != "INFT")
                                                    .Elements(a + "ServiceCharges").Elements(a + "BookingServiceCharge")
                                                    .Select(Amount => Convert.ToDecimal(Amount.Element(a + "Amount").Value)).Sum()

                                       };

                DataTable dtPax = ConvertToDataTable(PassengerDetails);

                #endregion

                #region Segment
                int SegmentIndex = 0;
                var JourneyDetails = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
                                     from Journey in Booking.Element(a + "Journeys").Elements(a + "Journey")
                                     from Segment in Journey.Element(a + "Segments").Elements(a + "Segment")
                                     select new
                                     {
                                         JourneySellKey = Journey.Element(a + "JourneySellKey").Value,
                                         SegmentSellKey = Segment.Element(a + "SegmentSellKey").Value,
                                         ActionStatusCode = Segment.Element(a + "ActionStatusCode").Value,
                                         CabinOfService = Segment.Element(a + "CabinOfService").Value,

                                         DepartureStation = Segment.Element(a + "DepartureStation").Value,
                                         ArrivalStation = Segment.Element(a + "ArrivalStation").Value,
                                         STD = Segment.Element(a + "STD").Value,
                                         STA = Segment.Element(a + "STA").Value,

                                         SalesDate = Segment.Element(a + "SalesDate").Value,
                                         International = Segment.Element(a + "International").Value,

                                         FlightNumber = Segment.Element(a + "FlightDesignator").Element(b + "FlightNumber").Value.Trim(),
                                         CarrierCode = Segment.Element(a + "FlightDesignator").Element(b + "CarrierCode").Value.Trim(),

                                         EquipmentType = Segment.Element(a + "Legs").Element(a + "Leg").Element(a + "LegInfo")
                                                         .Element(a + "EquipmentType").Value,

                                         DepartureTerminal = Segment.Element(a + "Legs").Element(a + "Leg").Element(a + "LegInfo")
                                                         .Element(a + "DepartureTerminal").Value,
                                         ArrivalTerminal = Segment.Element(a + "Legs").Element(a + "Leg").Element(a + "LegInfo")
                                                         .Element(a + "ArrivalTerminal").Value,
                                         JourneyIndex = Booking.Elements(a + "Journeys").Elements(a + "Journey").ToList().IndexOf(Journey) + 1,

                                         SegmentIndex = Journey.Elements(a + "Segments").Elements(a + "Segment").ToList().IndexOf(Segment) + 1,

                                         SegmentIndexAll = (++SegmentIndex)

                                     };
                DataTable dtSegment = ConvertToDataTable(JourneyDetails);


                #endregion

                #region Fares
                var FareDetails = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
                                  from Journey in Booking.Element(a + "Journeys").Elements(a + "Journey")
                                  from Segment in Journey.Element(a + "Segments").Elements(a + "Segment")
                                  from Fares in Segment.Element(a + "Fares").Elements(a + "Fare")
                                  from PaxFare in Fares.Element(a + "PaxFares").Elements(a + "PaxFare")
                                  select new
                                  {
                                      JourneySellKey = Journey.Element(a + "JourneySellKey").Value,
                                      SegmentSellKey = Segment.Element(a + "SegmentSellKey").Value,
                                      FareSellKey = Fares.Element(a + "FareSellKey").Value,

                                      ClassOfService = Fares.Element(a + "ClassOfService").Value,
                                      FareBasisCode = Fares.Element(a + "FareBasisCode").Value,

                                      RuleNumber = Fares.Element(a + "RuleNumber").Value,

                                      ProductClass = Fares.Element(a + "ProductClass").Value,
                                      FareApplicationType = Fares.Element(a + "FareApplicationType").Value,
                                      TravelClassCode = Fares.Element(a + "TravelClassCode").Value,

                                      PaxType = PaxFare.Element(a + "PaxType").Value,

                                      BaseFare = PaxFare.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge")
                                                .Where(Avl => Avl.Element(a + "ChargeCode").Value.Equals(""))
                                                .Select(Avl => Convert.ToDouble(Avl.Element(a + "Amount").Value)).Sum(),

                                      GrossAmount = PaxFare.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge")
                                                  .Select(Avl => Convert.ToDouble(Avl.Element(a + "Amount").Value)).Sum(),

                                      TaxAmount = PaxFare.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge")
                                                    .Where(Avl => !Avl.Element(a + "ChargeCode").Value.Equals(""))
                                                    .Select(Avl => Convert.ToDouble(Avl.Element(a + "Amount").Value)).Sum(),

                                      BreakUp = string.Join("/", PaxFare.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge")
                                                   .Where(Avl => !Avl.Element(a + "ChargeCode").Value.Equals(""))
                                                   .Select(Avl => Avl.Element(a + "ChargeCode").Value + ":" + Avl.Element(a + "Amount").Value).ToArray()),

                                      CurrencyCode = PaxFare.Element(a + "ServiceCharges").Element(a + "BookingServiceCharge").Element(a + "CurrencyCode").Value,
                                  };

                DataTable dtFaredetails = ConvertToDataTable(FareDetails);
                #endregion

                #region PaxSegment
                var PaxSegmentdetails = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
                                        from Journey in Booking.Element(a + "Journeys").Elements(a + "Journey")
                                        from Segment in Journey.Element(a + "Segments").Elements(a + "Segment")
                                        from PaxSegment in Segment.Element(a + "PaxSegments").Elements(a + "PaxSegment")
                                        select new
                                        {
                                            JourneySellKey = Journey.Element(a + "JourneySellKey").Value,
                                            SegmentSellKey = Segment.Element(a + "SegmentSellKey").Value,

                                            PassengerNumber = PaxSegment.Element(a + "PassengerNumber").Value,
                                            BoardingSequence = PaxSegment.Element(a + "BoardingSequence").Value,
                                            BaggageAllowanceWeight = PaxSegment.Element(a + "BaggageAllowanceWeight").Value,
                                            BaggageAllowanceWeightType = PaxSegment.Element(a + "BaggageAllowanceWeightType").Value
                                        };
                DataTable dtPaxSegmentdetails = ConvertToDataTable(PaxSegmentdetails);
                #endregion

                #region SSR
                var SSRDetails = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
                                 from Passenger in Booking.Element(a + "Passengers").Elements(a + "Passenger")
                                 from PassengerFee in Passenger.Element(a + "PassengerFees").Elements(a + "PassengerFee")
                                 select new
                                 {
                                     PassengerNumber = Passenger.Element(a + "PassengerNumber").Value,
                                     Code = (PassengerFee.Element(a + "FeeCode").Value),
                                     ActionStatusCode = PassengerFee.Element(a + "ActionStatusCode").Value,
                                     SSRNumber = PassengerFee.Element(a + "SSRNumber").Value,
                                     FlightReference = PassengerFee.Element(a + "FlightReference").Value,
                                     SSRAmount = PassengerFee.Elements(a + "ServiceCharges").Elements(a + "BookingServiceCharge")
                                            .Select(Amount => Convert.ToDecimal(Amount.Element(a + "Amount").Value)).Sum(),
                                     SSRBreakup = String.Join("/", PassengerFee.Elements(a + "ServiceCharges").Elements(a + "BookingServiceCharge")
                                            .Select(Amount => Amount.Element(a + "ChargeCode").Value + ":" + Amount.Element(a + "Amount").Value).ToArray()),

                                     FeeNumber = PassengerFee.Element(a + "FeeNumber").Value,

                                     #region ExtraCheck
                                     Date = PassengerFee.Element(a + "FlightReference").Value.Substring(0, 8),
                                     Sector = PassengerFee.Element(a + "FlightReference").Value.Substring(PassengerFee.Element(a + "FlightReference").Value.Length - 6),
                                     FlightNumber = PassengerFee.Element(a + "FlightReference").Value.Substring(8, PassengerFee.Element(a + "FlightReference").Value.Length - 6 - 8)
                                     #endregion

                                 };

                DataTable dtSSR = ConvertToDataTable(SSRDetails);
                #endregion

                string Check = "";

                #region InfantDetails
                var InfantDetails = PassengerDetails.AsEnumerable().Where(P => P.Infant.ToString().ToUpper().Equals("FALSE"));
                DataTable dtInfant = new DataTable();
                int InfantCount = 0;
                object obj = null;
                if (InfantDetails.Count() != 0)
                {
                    var InfantPax = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
                                    from Passenger in Booking.Element(a + "Passengers").Elements(a + "Passenger")
                                    from Infant in Passenger.Elements(a + "Infant")
                                    from PassengerFees in Passenger.Elements(a + "PassengerFees")
                                    from InfPax in InfantDetails
                                    where InfPax.PassengerNumber == Passenger.Element(a + "PassengerNumber").Value
                                    select new
                                    {
                                        PaxType = "INF",
                                        Gender = Infant.Element(a + "Gender").Value,
                                        Title = Infant.Element(a + "Names").Element(a + "BookingName").Element(a + "Title").Value,
                                        FirstName = Infant.Element(a + "Names").Element(a + "BookingName").Element(a + "FirstName").Value,
                                        LastName = Infant.Element(a + "Names").Element(a + "BookingName").Element(a + "LastName").Value,
                                        TotalCost = PassengerFees.Elements(a + "PassengerFee").Where(Code => Code.Element(a + "FeeCode").Value == "INFT")
                                                    .Elements(a + "ServiceCharges").Elements(a + "BookingServiceCharge")
                                                    .Select(Amount => Convert.ToDecimal(Amount.Element(a + "Amount").Value)).Sum(),
                                        SSRAmount = 0,
                                        BalanceDue = 0,
                                        PassengerNumber = Booking.Element(a + "Passengers").Elements(a + "Passenger").Count() + (InfPax.PassengerNumber != Passenger.Element(a + "PassengerNumber").Value
                                                            ? 0 : InfantCount++)
                                    };

                    dtInfant = ConvertToDataTable(InfantPax);
                    obj = InfantPax;

                }
                if (InfantDetails.Count() != InfantCount)
                {
                    return false;
                }
                #endregion

                Decimal dcPaymentAmount = Convert.ToDecimal(BookingDetails.ToArray()[0].PaymentAmount);
                Decimal dcPaxWise = PassengerDetails.Sum(P => Convert.ToDecimal(P.TotalCost));
                if (dcPaymentAmount != dcPaxWise)
                {
                    Check = "Trur;";
                }



                DataSet ds = new DataSet();
                ds.Merge(dtFare);
                ds.Merge(dtFaredetails);
                ds.Merge(dtPax);
                ds.Merge(dtPaxSegmentdetails);
                ds.Merge(dtSegment);
                ds.Merge(dtSSR);
                ds.Merge(dtInfant);


                #region Comnbine Flights

                var JoinFlightPax = from SegmentDetails in JourneyDetails
                                    from Passenger in PassengerDetails
                                    from Fare in FareDetails
                                    from PaxSegment in PaxSegmentdetails
                                    from SSR in SSRDetails
                                    where PaxSegment.PassengerNumber == Passenger.PassengerNumber

                                        && Fare.JourneySellKey == PaxSegment.JourneySellKey
                                        && Fare.SegmentSellKey == PaxSegment.SegmentSellKey

                                        && SSR.FlightNumber.Replace(" ", "") == "6E" + SegmentDetails.FlightNumber.Replace(" ", "")
                                        && SSR.Code != "INFT"
                                        && Passenger.PaxType == Fare.PaxType

                                    select new
                                    {
                                        Name = Passenger.FirstName + " " + Passenger.LastName,

                                        Secor = SegmentDetails.DepartureStation + SegmentDetails.ArrivalStation,
                                        FlightNumber = SegmentDetails.FlightNumber,

                                        BaggageWeight = PaxSegment.BaggageAllowanceWeight,

                                        FareAmmount = Fare.GrossAmount,

                                        SSRAmount = SSR.SSRAmount,
                                        SSRBreakup = SSR.SSRBreakup


                                    };


                DataTable dtPaxDetails = ConvertToDataTable(JoinFlightPax);
                #endregion



                return true;
            }
            catch (Exception ex)
            {
                string strLineNo = string.Empty;
                if (ex.StackTrace != null)
                {
                    if (ex.StackTrace.Contains("cs:line"))
                    {
                        strLineNo = ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line"));
                    }
                }
                //Logging.StoreLog(Terminal, lstrSequence, StoreType.Xml,
                //                      LogType.X, pAppType, "",
                //                      strLineNo + ex.Message.ToString(), "6E", (new StackTrace()).GetFrame(0).GetMethod().Name, ex, false, Level.BelowHigh);
                return false;
            }
        }
        public static DataTable ConvertToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names   
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow   
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;

        }

        public static bool ParsePnrRetrive(string Terminal, string Sequence, Hashtable hshCredentials, string Response, string AirlinePNR, ref DataTable dtResponse, ref string Error)
        {
            try
            {
                XNamespace xmlns = "http://tempuri.org/";
                XNamespace a = "http://schemas.datacontract.org/2004/07/Radixx.ConnectPoint.Reservation.Response";
                XNamespace b = "http://schemas.datacontract.org/2004/07/Radixx.ConnectPoint.Exceptions";

                string Test = "BAGB,BAGL,BAGX,BUPL,BUPX,BUPZ,JBAG,AIR";
                Hashtable BaggIndex = new Hashtable();
                Hashtable FareIndex = new Hashtable();

                Hashtable lhs = new Hashtable();

                var CommonDetails = from RetrievePNRResponse in XDocument.Parse(Response).Descendants(xmlns + "RetrievePNRResponse")
                                          .Elements(xmlns + "RetrievePNRResult").AsEnumerable()
                                    select new
                                    {
                                        PNRNo = RetrievePNRResponse.Element(a + "ConfirmationNumber").Value,
                                        IATANumber = RetrievePNRResponse.Element(a + "IATANumber").Value,
                                        BookDate = RetrievePNRResponse.Element(a + "BookDate").Value,
                                        ReservationType = RetrievePNRResponse.Element(a + "ReservationType").Value,
                                        LastModified = RetrievePNRResponse.Element(a + "LastModified").Value,

                                        Cabin = RetrievePNRResponse.Element(a + "Cabin").Value,
                                        ChangeFee = RetrievePNRResponse.Element(a + "ChangeFee").Value,
                                        ReservationBalance = RetrievePNRResponse.Element(a + "ReservationBalance").Value,
                                        LogicalFlightCount = RetrievePNRResponse.Element(a + "LogicalFlightCount").Value,
                                        ActivePassengerCount = RetrievePNRResponse.Element(a + "ActivePassengerCount").Value,

                                        PaymentCount = RetrievePNRResponse.Element(a + "Payments").Elements(a + "Payment").Count(),

                                        PaymentAmount = RetrievePNRResponse.Element(a + "Payments").Elements(a + "Payment")
                                                        .Select(P => Convert.ToDecimal(P.Element(a + "PaymentAmount").Value)).Sum()
                                    };

                DataTable dt = ConvertToDataTable(CommonDetails);


                var Flights = from RetrievePNRResponse in XDocument.Parse(Response).Descendants(xmlns + "RetrievePNRResponse")
                                 .Elements(xmlns + "RetrievePNRResult").AsEnumerable()
                              from LogicalFlight in RetrievePNRResponse.Elements(a + "Airlines").Elements(a + "Airline").Elements(a + "LogicalFlight")
                                  .Elements(a + "LogicalFlight").AsEnumerable()
                              select new
                              {
                                  Key = LogicalFlight.Element(a + "Key").Value,
                                  RecordNumber = LogicalFlight.Element(a + "RecordNumber").Value,
                                  LogicalFlightID = LogicalFlight.Element(a + "LogicalFlightID").Value,
                                  DepartureDate = LogicalFlight.Element(a + "DepartureDate").Value,
                                  Origin = LogicalFlight.Element(a + "Origin").Value,
                                  OriginDefaultTerminal = LogicalFlight.Element(a + "OriginDefaultTerminal").Value,
                                  OriginName = LogicalFlight.Element(a + "OriginName").Value,
                                  Destination = LogicalFlight.Element(a + "Destination").Value,
                                  DestinationName = LogicalFlight.Element(a + "DestinationName").Value,
                                  DestinationDefaultTerminal = LogicalFlight.Element(a + "DestinationDefaultTerminal").Value,
                                  OriginMetroGroup = LogicalFlight.Element(a + "OriginMetroGroup").Value,
                                  DestinationMetroGroup = LogicalFlight.Element(a + "DestinationMetroGroup").Value,

                                  OperatingCarrier = LogicalFlight.Element(a + "OperatingCarrier").Value,
                                  OperatingFlightNumber = LogicalFlight.Element(a + "OperatingFlightNumber").Value,

                                  DepartureTime = LogicalFlight.Element(a + "DepartureTime").Value,
                                  Arrivaltime = LogicalFlight.Element(a + "Arrivaltime").Value,

                                  Active = LogicalFlight.Element(a + "Active").Value,
                              };

                DataTable dtNew = ConvertToDataTable(Flights);

                var PhysicalFlights = from RetrievePNRResponse in XDocument.Parse(Response).Descendants(xmlns + "RetrievePNRResponse")
                                          .Elements(xmlns + "RetrievePNRResult").AsEnumerable()
                                      from LogicalFlight1 in RetrievePNRResponse.Elements(a + "Airlines").Elements(a + "Airline").Elements(a + "LogicalFlight")
                                          .Elements(a + "LogicalFlight").AsEnumerable()
                                      from PhysicalFlight in LogicalFlight1.Elements(a + "PhysicalFlights")
                                          .Elements(a + "PhysicalFlight").AsEnumerable()
                                      select new
                                      {
                                          Key = PhysicalFlight.Element(a + "Key").Value,
                                          RecordNumber = PhysicalFlight.Element(a + "RecordNumber").Value,
                                          LogicalFlightID = PhysicalFlight.Element(a + "LogicalFlightID").Value,
                                          PhysicalFlightID = PhysicalFlight.Element(a + "PhysicalFlightID").Value,
                                          FlightOrder = PhysicalFlight.Element(a + "FlightOrder").Value,
                                          DepartureDate = PhysicalFlight.Element(a + "DepartureDate").Value,
                                          Origin = PhysicalFlight.Element(a + "Origin").Value,
                                          OriginDefaultTerminal = PhysicalFlight.Element(a + "OriginDefaultTerminal").Value,
                                          OriginName = PhysicalFlight.Element(a + "OriginName").Value,
                                          Destination = PhysicalFlight.Element(a + "Destination").Value,
                                          DestinationName = PhysicalFlight.Element(a + "DestinationName").Value,
                                          DestinationDefaultTerminal = PhysicalFlight.Element(a + "DestinationDefaultTerminal").Value,
                                          OriginMetroGroup = PhysicalFlight.Element(a + "OriginMetroGroup").Value,
                                          DestinationMetroGroup = PhysicalFlight.Element(a + "DestinationMetroGroup").Value,

                                          OperatingCarrier = PhysicalFlight.Element(a + "OperatingCarrier").Value,
                                          OperatingFlightNumber = PhysicalFlight.Element(a + "OperatingFlightNumber").Value,

                                          DepartureTime = PhysicalFlight.Element(a + "DepartureTime").Value,
                                          Arrivaltime = PhysicalFlight.Element(a + "Arrivaltime").Value,
                                          FlightDuration = PhysicalFlight.Element(a + "FlightDuration").Value,
                                          Active = PhysicalFlight.Element(a + "Active").Value,
                                      };
                DataTable dtNewPhy = ConvertToDataTable(PhysicalFlights);

                var PassengerInfo = from RetrievePNRResponse in XDocument.Parse(Response).Descendants(xmlns + "RetrievePNRResponse")
                                          .Elements(xmlns + "RetrievePNRResult").AsEnumerable()
                                    from LogicalFlight1 in RetrievePNRResponse.Elements(a + "Airlines").Elements(a + "Airline").Elements(a + "LogicalFlight")
                                        .Elements(a + "LogicalFlight").AsEnumerable()
                                    from PhysicalFlight in LogicalFlight1.Elements(a + "PhysicalFlights")
                                        .Elements(a + "PhysicalFlight").AsEnumerable()
                                    from AirlinePerson in PhysicalFlight.Elements(a + "Customers").Elements(a + "Customer").Elements(a + "AirlinePersons")
                                                     .Elements(a + "AirlinePerson").AsEnumerable()
                                    select new
                                    {
                                        Key = PhysicalFlight.Element(a + "Key").Value,
                                        RecordNumber = PhysicalFlight.Element(a + "RecordNumber").Value,
                                        LogicalFlightID = PhysicalFlight.Element(a + "LogicalFlightID").Value,
                                        PhysicalFlightID = PhysicalFlight.Element(a + "PhysicalFlightID").Value,

                                        PaxType = AirlinePerson.Element(a + "PTCID").Value.Trim().Equals("1") ? "ADT" : AirlinePerson.Element(a + "PTCID").Value.Trim().Equals("6")
                                                ? "CHD" : "INF",
                                        Title = AirlinePerson.Element(a + "Title").Value.Trim(),
                                        FirstName = AirlinePerson.Element(a + "FirstName").Value.Trim(),
                                        LastName = AirlinePerson.Element(a + "LastName").Value.Trim(),
                                        DOB = (string.IsNullOrEmpty(AirlinePerson.Element(a + "DOB").Value.Trim()) ? "" : AirlinePerson.Element(a + "DOB").Value.Trim()),
                                        FareClassCode = AirlinePerson.Element(a + "FareClassCode").Value.Trim(),
                                        FareBasisCode = AirlinePerson.Element(a + "FareBasisCode").Value.Trim(),
                                        WebFareType = AirlinePerson.Element(a + "WebFareType").Value.Trim(),
                                        Cabin = AirlinePerson.Element(a + "Cabin").Value.Trim(),

                                        BaseFare = AirlinePerson.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR"))
                                                          .Select(P => Convert.ToDouble(P.Element(a + "Amount").Value.Trim())).Sum(),
                                        TaxFare = AirlinePerson.Elements(a + "Charges").Elements(a + "Charge").Where(bf => !bf.Element(a + "CodeType").Value.Trim().Equals("AIR"))
                                                    .Select(P => Convert.ToDouble(P.Element(a + "Amount").Value.Trim())).Sum(),

                                        GrossAmount = AirlinePerson.Elements(a + "Charges").Elements(a + "Charge")
                                              .Select(P => Convert.ToDouble(P.Element(a + "Amount").Value.Trim())).Sum(),

                                        TaxBreakUp = String.Join("/", AirlinePerson.Elements(a + "Charges").Elements(a + "Charge")
                                                                .Where(bf => !bf.Element(a + "CodeType").Value.Trim().Equals("AIR"))
                                                                .Select(P => P.Element(a + "CodeType").Value + ":" + P.Element(a + "Amount").Value.Trim()).ToArray()),

                                    };
                DataTable dtPassenger = new DataTable();
                dtPassenger = ConvertToDataTable(PassengerInfo);

                DataTable dtt = new DataTable();

                //var result = from RetrievePNRResponse in XDocument.Parse(Response).Descendants(xmlns + "RetrievePNRResponse")
                //                 .Elements(xmlns + "RetrievePNRResult").AsEnumerable()
                //             from LogicalFlight in RetrievePNRResponse.Elements(a + "Airlines").Elements(a + "Airline").Elements(a + "LogicalFlight")
                //                 .Elements(a + "LogicalFlight").AsEnumerable().Select((item, index) => new { item, index })
                //             from PhysicalFlight in LogicalFlight.item.Elements(a + "PhysicalFlights").Elements(a + "PhysicalFlight").AsEnumerable()
                //                 .Select((item, index) => new { item, index })
                //             from AirlinePrs in PhysicalFlight.item.Elements(a + "Customers").Elements(a + "Customer").Elements(a + "AirlinePersons")
                //                 .Elements(a + "AirlinePerson").AsEnumerable().Select((item, index) => new { item, index })

                //             orderby Convert.ToDecimal(AirlinePrs.item.Element(a + "PersonOrgID").Value.Trim()) ascending
                //             orderby Convert.ToInt32(DateTime.ParseExact(PhysicalFlight.item.Element(a + "DepartureTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("yyyyMMdd").Trim()) ascending
                //             select new
                //             {
                //                 AIRLINEPNR = RetrievePNRResponse.Element(a + "ConfirmationNumber").Value,
                //                 TICKETNO = RetrievePNRResponse.Element(a + "ConfirmationNumber").Value + "-" + (AirlinePrs.index + 1),

                //                 FLIGHTNO = PhysicalFlight.item.Element(a + "FlightNumber").Value.Trim(),
                //                 ORIGIN = PhysicalFlight.item.Element(a + "Origin").Value.Trim(),
                //                 DESTINATION = PhysicalFlight.item.Element(a + "Destination").Value.Trim(),
                //                 ORGINTERMINAL = "",
                //                 DESTTERMINAL = "",
                //                 DEPARTUREDATE = DateTime.ParseExact(PhysicalFlight.item.Element(a + "DepartureTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy").Trim(),
                //                 ARRIVALDATE = DateTime.ParseExact(PhysicalFlight.item.Element(a + "Arrivaltime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy").Trim(),
                //                 DEPARTURETIME = DateTime.ParseExact(PhysicalFlight.item.Element(a + "DepartureTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("HH:mm").Trim(),
                //                 ARRIVALTIME = DateTime.ParseExact(PhysicalFlight.item.Element(a + "Arrivaltime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("HH:mm").Trim(),
                //                 AIRLINECODE = PhysicalFlight.item.Element(a + "CarrierCode").Value.Trim(),
                //                 TICKETINGCARRIER = PhysicalFlight.item.Element(a + "SellingCarrier").Value.Trim(),
                //                 CLASS = AirlinePrs.item.Element(a + "FareClassCode").Value.Trim(),
                //                 FAREBASISCODE = AirlinePrs.item.Element(a + "FareBasisCode").Value.Trim(),
                //                 FAREBASIS = AirlinePrs.item.Element(a + "FareBasisCode").Value.Trim(),

                //                 STOCK = PhysicalFlight.item.Element(a + "CarrierCode").Value.Trim(),

                //                 BAGGAGE = GetBaggage(string.Join("/", AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Select(tax => tax.Element(a + "CodeType").Value.Trim() + " : " + tax.Element(a + "OriginalAmount").Value.Trim()).ToArray()), LogicalFlight.index, PhysicalFlight.item.Elements(a + "Customers").Elements(a + "Customer").Elements(a + "AirlinePersons").Elements(a + "AirlinePerson").ToList().IndexOf(AirlinePrs.item) + 1, ref BaggIndex),
                //                 BAGGAGEAMT = GetBaggageFare(string.Join("/", AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Select(tax => tax.Element(a + "CodeType").Value.Trim() + " : " + tax.Element(a + "OriginalAmount").Value.Trim()).ToArray()), LogicalFlight.index, PhysicalFlight.item.Elements(a + "Customers").Elements(a + "Customer").Elements(a + "AirlinePersons").Elements(a + "AirlinePerson").ToList().IndexOf(AirlinePrs.item) + 1, ref FareIndex),

                //                 SEQNO = AirlinePrs.index + 1,
                //                 Condition = AirlinePrs.item.Element(a + "PersonOrgID").Value.Trim(),
                //                 PAXTYPE = AirlinePrs.item.Element(a + "PTCID").Value.Trim().Equals("1") ? "ADT" : AirlinePrs.item.Element(a + "PTCID").Value.Trim().Equals("6") ? "CHD" : "INF",
                //                 TITLE = AirlinePrs.item.Element(a + "Title").Value.Trim(),
                //                 FIRSTNAME = AirlinePrs.item.Element(a + "FirstName").Value.Trim(),
                //                 LASTNAME = AirlinePrs.item.Element(a + "LastName").Value.Trim(),
                //                 DATEOFBIRTH = (string.IsNullOrEmpty(PhysicalFlight.item.Element(a + "Customers").Element(a + "Customer").Element(a + "AirlinePersons").Element(a + "AirlinePerson").Element(a + "DOB").Value.Trim()) ? "" : (DateTime.ParseExact(PhysicalFlight.item.Element(a + "Customers").Element(a + "Customer").Element(a + "AirlinePersons").Element(a + "AirlinePerson").Element(a + "DOB").Value.Trim(), "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy").Trim().Equals("01/01/0001") ? "" : DateTime.ParseExact(PhysicalFlight.item.Element(a + "Customers").Element(a + "Customer").Element(a + "AirlinePersons").Element(a + "AirlinePerson").Element(a + "DOB").Value.Trim(), "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy").Trim())),

                //                 BASEAMT = Math.Ceiling(Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).Count() == 0
                //                            ? "0" : AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).ElementAt(0).Element(a + "Amount").Value.Trim())),
                //                 GROSSAMT = Math.Ceiling(Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => !bf.Element(a + "CodeType").Value.Trim().Equals("AIR") && !Test.Contains(bf.Element(a + "CodeType").Value.Trim())).Sum(TX => Convert.ToDouble(TX.Element(a + "Amount").Value.Trim())))
                //                                                         + Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).Count() == 0 ? "0" :
                //                                                         AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).ElementAt(0).Element(a + "Amount").Value.Trim())),
                //                 TOTALTAXAMT = Math.Ceiling(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => !bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).Sum(TX => Convert.ToDouble(TX.Element(a + "Amount").Value.Trim()))),
                //                 TAXBREAKUP = string.Join("/", (from air in AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge")
                //                                                where (!Test.Contains(air.Element(a + "CodeType").Value.Trim()))
                //                                                select (air.Element(a + "CodeType").Value.Trim() + " : " + Math.Ceiling(Convert.ToDouble(air.Element(a + "OriginalAmount").Value.Trim())))).ToArray()),
                //                 CURRENCY = RetrievePNRResponse.Element(a + "Payments").Element(a + "Payment").Element(a + "CurrencyPaid").Value.Trim(),

                //                 ITINREF = LogicalFlight.index + 1,

                //                 APIBASEFARE = Math.Ceiling(Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).Count() == 0
                //                            ? "0" : AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).ElementAt(0).Element(a + "Amount").Value.Trim())),
                //                 APIGROSSFARE = Math.Ceiling(Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => !bf.Element(a + "CodeType").Value.Trim().Equals("AIR") && !Test.Contains(bf.Element(a + "CodeType").Value.Trim())).Sum(TX => Convert.ToDouble(TX.Element(a + "Amount").Value.Trim())))
                //                                                         + Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).Count() == 0 ? "0" :
                //                                                         AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).ElementAt(0).Element(a + "Amount").Value.Trim())),
                //                 APIYQFARE = "0",
                //                 APITAXBREAKUP = string.Join("/", (from air in AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge")
                //                                                   where (!Test.Contains(air.Element(a + "CodeType").Value.Trim()))
                //                                                   select (air.Element(a + "CodeType").Value.Trim() + " : " + Math.Ceiling(Convert.ToDouble(air.Element(a + "OriginalAmount").Value.Trim())))).ToArray()),
                //                 APICURRENCY = RetrievePNRResponse.Element(a + "Payments").Element(a + "Payment").Element(a + "CurrencyPaid").Value.Trim(),
                //                 FAREID = LogicalFlight.index + 1,
                //             };
                //dtResponse = ConvertToDataTable(result);

                //Hashtable _hshItnResp = new Hashtable();
                //Hashtable _hshSeqResp = new Hashtable();
                //int itn = 1;
                //int seq = 1;

                //for (int i = 0; i < dtResponse.Rows.Count; i++)
                //{
                //    if (_hshSeqResp.Contains(dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()))
                //    {
                //        dtResponse.Rows[i]["SEQNO"] = _hshSeqResp[dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                //        dtResponse.Rows[i]["TICKETNO"] = dtResponse.Rows[i]["AIRLINEPNR"].ToString() + "-" + _hshSeqResp[dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                //    }
                //    else
                //    {
                //        _hshSeqResp[dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()] = seq++;
                //        dtResponse.Rows[i]["SEQNO"] = _hshSeqResp[dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                //        dtResponse.Rows[i]["TICKETNO"] = dtResponse.Rows[i]["AIRLINEPNR"].ToString() + "-" + _hshSeqResp[dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                //    }

                //    if (_hshItnResp.Contains(dtResponse.Rows[i]["SEQNO"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()))
                //    {
                //        dtResponse.Rows[i]["ITINREF"] = _hshItnResp[dtResponse.Rows[i]["SEQNO"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                //        dtResponse.Rows[i]["FAREID"] = _hshItnResp[dtResponse.Rows[i]["SEQNO"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                //    }
                //    else
                //    {
                //        dtResponse.Rows[i]["ITINREF"] = itn;
                //        dtResponse.Rows[i]["FAREID"] = itn;
                //        _hshItnResp[dtResponse.Rows[i]["SEQNO"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()] = (itn + 1);
                //    }
                //}



                //if (dtResponse == null)
                //{
                //    Error = "Unable to retrive PNR details, Please contact supplier team.";
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                string strLineNo = string.Empty;
                if (ex.StackTrace != null)
                {
                    if (ex.StackTrace.Contains("cs:line"))
                    {
                        strLineNo = ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line"));
                    }
                }
                Error = "Unable to retrive PNR details, Please contact supplier team.";
                //Logging.StoreLog(Terminal, Sequence, StoreType.Xml,
                //LogType.X, _AppType, "",
                //strLineNo + ex.Message.ToString(), "FZ", (new StackTrace()).GetFrame(0).GetMethod().Name, ex, false, Level.BelowHigh);
                return false;
            }
        }
        public static bool ParsePnrRetriveNew(string Terminal, string Sequence, Hashtable hshCredentials, string Response, string AirlinePNR, ref DataTable dtResponse, ref string Error)
        {
            try
            {
                XNamespace xmlns = "http://tempuri.org/";
                XNamespace a = "http://schemas.datacontract.org/2004/07/Radixx.ConnectPoint.Reservation.Response";
                XNamespace b = "http://schemas.datacontract.org/2004/07/Radixx.ConnectPoint.Exceptions";

                string Test = "BAGB,BAGL,BAGX,BUPL,BUPX,BUPZ,JBAG,AIR";
                Hashtable BaggIndex = new Hashtable();
                Hashtable FareIndex = new Hashtable();

                Hashtable lhs = new Hashtable();
                var result = from RetrievePNRResponse in XDocument.Parse(Response).Descendants(xmlns + "RetrievePNRResponse").Elements(xmlns + "RetrievePNRResult").AsEnumerable()
                             from LogicalFlight in RetrievePNRResponse.Elements(a + "Airlines").Elements(a + "Airline").Elements(a + "LogicalFlight").Elements(a + "LogicalFlight").AsEnumerable().Select((item, index) => new { item, index })
                             from PhysicalFlight in LogicalFlight.item.Elements(a + "PhysicalFlights").Elements(a + "PhysicalFlight").AsEnumerable().Select((item, index) => new { item, index })
                             from AirlinePrs in PhysicalFlight.item.Elements(a + "Customers").Elements(a + "Customer").Elements(a + "AirlinePersons").Elements(a + "AirlinePerson").AsEnumerable().Select((item, index) => new { item, index })
                             orderby Convert.ToDecimal(AirlinePrs.item.Element(a + "PersonOrgID").Value.Trim()) ascending
                             orderby Convert.ToInt32(DateTime.ParseExact(PhysicalFlight.item.Element(a + "DepartureTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("yyyyMMdd").Trim()) ascending
                             select new
                             {
                                 SUPPLIERPNR = "N/A",
                                 CRSPNR = "N/A",
                                 AIRLINEPNR = RetrievePNRResponse.Element(a + "ConfirmationNumber").Value,
                                 TICKETNO = RetrievePNRResponse.Element(a + "ConfirmationNumber").Value + "-" + (AirlinePrs.index + 1),

                                 FLIGHTNO = PhysicalFlight.item.Element(a + "FlightNumber").Value.Trim(),
                                 ORIGIN = PhysicalFlight.item.Element(a + "Origin").Value.Trim(),
                                 DESTINATION = PhysicalFlight.item.Element(a + "Destination").Value.Trim(),
                                 ORGINTERMINAL = "",
                                 DESTTERMINAL = "",
                                 DEPARTUREDATE = DateTime.ParseExact(PhysicalFlight.item.Element(a + "DepartureTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy").Trim(),
                                 ARRIVALDATE = DateTime.ParseExact(PhysicalFlight.item.Element(a + "Arrivaltime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy").Trim(),
                                 DEPARTURETIME = DateTime.ParseExact(PhysicalFlight.item.Element(a + "DepartureTime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("HH:mm").Trim(),
                                 ARRIVALTIME = DateTime.ParseExact(PhysicalFlight.item.Element(a + "Arrivaltime").Value, "yyyy-MM-ddTHH:mm:ss", null).ToString("HH:mm").Trim(),
                                 AIRLINECODE = PhysicalFlight.item.Element(a + "CarrierCode").Value.Trim(),
                                 TICKETINGCARRIER = PhysicalFlight.item.Element(a + "SellingCarrier").Value.Trim(),
                                 CLASS = AirlinePrs.item.Element(a + "FareClassCode").Value.Trim(),
                                 CABIN = "",
                                 FAREBASISCODE = AirlinePrs.item.Element(a + "FareBasisCode").Value.Trim(),
                                 FAREBASIS = AirlinePrs.item.Element(a + "FareBasisCode").Value.Trim(),
                                 GROUP = "",
                                 FQTV = "",
                                 STOCK = PhysicalFlight.item.Element(a + "CarrierCode").Value.Trim(),
                                 STATUS = "",

                                 BAGGAGE = GetBaggage(string.Join("/", AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Select(tax => tax.Element(a + "CodeType").Value.Trim() + " : " + tax.Element(a + "OriginalAmount").Value.Trim()).ToArray()), LogicalFlight.index, PhysicalFlight.item.Elements(a + "Customers").Elements(a + "Customer").Elements(a + "AirlinePersons").Elements(a + "AirlinePerson").ToList().IndexOf(AirlinePrs.item) + 1, ref BaggIndex),
                                 BAGGAGEAMT = GetBaggageFare(string.Join("/", AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Select(tax => tax.Element(a + "CodeType").Value.Trim() + " : " + tax.Element(a + "OriginalAmount").Value.Trim()).ToArray()), LogicalFlight.index, PhysicalFlight.item.Elements(a + "Customers").Elements(a + "Customer").Elements(a + "AirlinePersons").Elements(a + "AirlinePerson").ToList().IndexOf(AirlinePrs.item) + 1, ref FareIndex),
                                 MEALS = "",
                                 MEALSAMT = "0",
                                 SEAT = "",
                                 SEATAMT = "0",
                                 WHEEL = "",
                                 WHEELAMT = "0",

                                 REFERENCE = "",
                                 TRIPTYPE = "",
                                 AIRLINECATEGORY = "LCC",

                                 USERTRACKID = "",
                                 //OFFICEID = hshCredentials.Contains("OFFICEID") ? hshCredentials["OFFICEID"].ToString() : "",

                                 SEQNO = AirlinePrs.index + 1,
                                 Condition = AirlinePrs.item.Element(a + "PersonOrgID").Value.Trim(),
                                 PAXTYPE = AirlinePrs.item.Element(a + "PTCID").Value.Trim().Equals("1") ? "ADT" : AirlinePrs.item.Element(a + "PTCID").Value.Trim().Equals("6") ? "CHD" : "INF",
                                 TITLE = AirlinePrs.item.Element(a + "Title").Value.Trim(),
                                 FIRSTNAME = AirlinePrs.item.Element(a + "FirstName").Value.Trim(),
                                 LASTNAME = AirlinePrs.item.Element(a + "LastName").Value.Trim(),
                                 DATEOFBIRTH = (string.IsNullOrEmpty(PhysicalFlight.item.Element(a + "Customers").Element(a + "Customer").Element(a + "AirlinePersons").Element(a + "AirlinePerson").Element(a + "DOB").Value.Trim()) ? "" : (DateTime.ParseExact(PhysicalFlight.item.Element(a + "Customers").Element(a + "Customer").Element(a + "AirlinePersons").Element(a + "AirlinePerson").Element(a + "DOB").Value.Trim(), "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy").Trim().Equals("01/01/0001") ? "" : DateTime.ParseExact(PhysicalFlight.item.Element(a + "Customers").Element(a + "Customer").Element(a + "AirlinePersons").Element(a + "AirlinePerson").Element(a + "DOB").Value.Trim(), "yyyy-MM-ddTHH:mm:ss", null).ToString("dd/MM/yyyy").Trim())),

                                 BASEAMT = Math.Ceiling(Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).Count() == 0
                                            ? "0" : AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).ElementAt(0).Element(a + "Amount").Value.Trim())),
                                 GROSSAMT = Math.Ceiling(Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => !bf.Element(a + "CodeType").Value.Trim().Equals("AIR") && !Test.Contains(bf.Element(a + "CodeType").Value.Trim())).Sum(TX => Convert.ToDouble(TX.Element(a + "Amount").Value.Trim())))
                                                                         + Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).Count() == 0 ? "0" :
                                                                         AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).ElementAt(0).Element(a + "Amount").Value.Trim())),
                                 TOTALTAXAMT = Math.Ceiling(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => !bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).Sum(TX => Convert.ToDouble(TX.Element(a + "Amount").Value.Trim()))),
                                 TAXBREAKUP = string.Join("/", (from air in AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge")
                                                                where (!Test.Contains(air.Element(a + "CodeType").Value.Trim()))
                                                                select (air.Element(a + "CodeType").Value.Trim() + " : " + Math.Ceiling(Convert.ToDouble(air.Element(a + "OriginalAmount").Value.Trim())))).ToArray()),
                                 CURRENCY = RetrievePNRResponse.Element(a + "Payments").Element(a + "Payment").Element(a + "CurrencyPaid").Value.Trim(),
                                 COMMISSION = "0",
                                 SERVICECHARGE = "0",
                                 MARKUP = "0",
                                 INCENTIVE = "0",
                                 SPECIALFARE = "N",
                                 ITINREF = LogicalFlight.index + 1,

                                 APIBASEFARE = Math.Ceiling(Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).Count() == 0
                                            ? "0" : AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).ElementAt(0).Element(a + "Amount").Value.Trim())),
                                 APIGROSSFARE = Math.Ceiling(Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => !bf.Element(a + "CodeType").Value.Trim().Equals("AIR") && !Test.Contains(bf.Element(a + "CodeType").Value.Trim())).Sum(TX => Convert.ToDouble(TX.Element(a + "Amount").Value.Trim())))
                                                                         + Convert.ToDouble(AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).Count() == 0 ? "0" :
                                                                         AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge").Where(bf => bf.Element(a + "CodeType").Value.Trim().Equals("AIR")).ElementAt(0).Element(a + "Amount").Value.Trim())),
                                 APIYQFARE = "0",
                                 APITAXBREAKUP = string.Join("/", (from air in AirlinePrs.item.Elements(a + "Charges").Elements(a + "Charge")
                                                                   where (!Test.Contains(air.Element(a + "CodeType").Value.Trim()))
                                                                   select (air.Element(a + "CodeType").Value.Trim() + " : " + Math.Ceiling(Convert.ToDouble(air.Element(a + "OriginalAmount").Value.Trim())))).ToArray()),
                                 APICURRENCY = RetrievePNRResponse.Element(a + "Payments").Element(a + "Payment").Element(a + "CurrencyPaid").Value.Trim(),
                                 SUPPCOMMISSION = "0",
                                 SUPPINCENTIVE = "0",
                                 SUPPMARKUP = "0",

                                 ROE_VALUE = "1",
                                 BOOK_SUPID = "",
                                 TKT_SUPID = "",
                                 LOGINTYPE = "N",
                                 FAREID = LogicalFlight.index + 1,
                             };
                dtResponse = ConvertToDataTable(result);

                Hashtable _hshItnResp = new Hashtable();
                Hashtable _hshSeqResp = new Hashtable();
                int itn = 1;
                int seq = 1;

                for (int i = 0; i < dtResponse.Rows.Count; i++)
                {
                    if (_hshSeqResp.Contains(dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()))
                    {
                        dtResponse.Rows[i]["SEQNO"] = _hshSeqResp[dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                        dtResponse.Rows[i]["TICKETNO"] = dtResponse.Rows[i]["AIRLINEPNR"].ToString() + "-" + _hshSeqResp[dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                    }
                    else
                    {
                        _hshSeqResp[dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()] = seq++;
                        dtResponse.Rows[i]["SEQNO"] = _hshSeqResp[dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                        dtResponse.Rows[i]["TICKETNO"] = dtResponse.Rows[i]["AIRLINEPNR"].ToString() + "-" + _hshSeqResp[dtResponse.Rows[i]["Condition"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                    }

                    if (_hshItnResp.Contains(dtResponse.Rows[i]["SEQNO"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()))
                    {
                        dtResponse.Rows[i]["ITINREF"] = _hshItnResp[dtResponse.Rows[i]["SEQNO"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                        dtResponse.Rows[i]["FAREID"] = _hshItnResp[dtResponse.Rows[i]["SEQNO"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()].ToString();
                    }
                    else
                    {
                        dtResponse.Rows[i]["ITINREF"] = itn;
                        dtResponse.Rows[i]["FAREID"] = itn;
                        _hshItnResp[dtResponse.Rows[i]["SEQNO"].ToString() + dtResponse.Rows[i]["FIRSTNAME"].ToString() + dtResponse.Rows[i]["LASTNAME"].ToString()] = (itn + 1);
                    }
                }



                if (dtResponse == null)
                {
                    Error = "Unable to retrive PNR details, Please contact supplier team.";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                string strLineNo = string.Empty;
                if (ex.StackTrace != null)
                {
                    if (ex.StackTrace.Contains("cs:line"))
                    {
                        strLineNo = ex.StackTrace.Substring(ex.StackTrace.IndexOf("cs:line"));
                    }
                }
                Error = "Unable to retrive PNR details, Please contact supplier team.";
                //Logging.StoreLog(Terminal, Sequence, StoreType.Xml,
                //LogType.X, _AppType, "",
                //strLineNo + ex.Message.ToString(), "FZ", (new StackTrace()).GetFrame(0).GetMethod().Name, ex, false, Level.BelowHigh);
                return false;
            }
        }
        private static int GetFareID(int a, int b, int c, ref Hashtable lhsData)
        {

            if (lhsData.Count == 0)
            {
                lhsData.Add(b.ToString() + c.ToString(), a);
                return a;
            }
            else if (lhsData.Contains((b - 1).ToString() + c.ToString()))
            {
                if (!lhsData.Contains(b.ToString() + c.ToString()))
                    lhsData.Add(b.ToString() + c.ToString(), a + Convert.ToInt32(lhsData[(b - 1).ToString() + c.ToString()]));
                else
                    lhsData[b.ToString() + c.ToString()] = a + Convert.ToInt32(lhsData[(b - 1).ToString() + c.ToString()]);

                return Convert.ToInt32(lhsData[b.ToString() + c.ToString()]);
            }
            else
            {
                lhsData[b.ToString() + c.ToString()] = a;
                return a;

            }
        }
        private static string GetBaggage(string BreakUp, int index, int PaxIndex, ref Hashtable _Temp)
        {
            try
            {
                if (_Temp.Contains(index.ToString() + PaxIndex.ToString()))
                {
                    return _Temp[index.ToString() + PaxIndex.ToString()].ToString();
                }
                string[] bagg = BreakUp.Split('/');
                string[] lstrBagg = new string[] { "BAGB", "BAGL", "BAGX", "BUPL", "BUPX", "BUPZ", "JBAG" };

                string[] lstrBaggValue = new string[] { "20 Kg", "30 Kg", "40 Kg", "30 Kg", "40 Kg", "40 Kg", "40Kg" };

                for (int i = 0; i < bagg.Length; i++)
                {
                    for (int lindex = 0; lindex < lstrBagg.Length; lindex++)
                    {
                        if (bagg[i].Contains(lstrBagg[lindex].ToString()))
                        {
                            _Temp.Add(index.ToString() + PaxIndex.ToString(), lstrBaggValue[lindex]);
                            return lstrBaggValue[lindex];
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return "";
        }
        private static string GetBaggageFare(string BreakUp, int index, int PaxIndex, ref Hashtable _Temp)
        {
            try
            {
                if (_Temp.Contains(index.ToString() + PaxIndex.ToString()))
                {
                    return _Temp[index.ToString() + PaxIndex.ToString()].ToString();
                }
                string[] bagg = BreakUp.Split('/');
                string[] lstrBagg = new string[] { "BAGB", "BAGL", "BAGX", "BUPL", "BUPX", "BUPZ", "JBAG" };

                string[] lstrBaggValue = new string[] { "20 Kg", "30 Kg", "40 Kg", "30 Kg", "40 Kg", "40 Kg", "40Kg" };

                for (int i = 0; i < bagg.Length; i++)
                {
                    for (int lindex = 0; lindex < lstrBagg.Length; lindex++)
                    {
                        if (bagg[i].Contains(lstrBagg[lindex].ToString()))
                        {
                            _Temp.Add(index.ToString() + PaxIndex.ToString(), bagg[i].Split(':')[1].ToString().Trim());
                            return bagg[i].Split(':')[1].ToString().Trim();
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return "";
        }
        #region

        //var PassengerDetails = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
        //                       from Pax in Booking.Element(a + "Passengers").Elements(a + "Passenger").Select((Val, index) => new { Val, index })
        //                       select new
        //                       {
        //                           SEQNO = Pax.index + 1,
        //                           PAXTYPE = Pax.Val.Element(a + "PassengerTypeInfos").Element(a + "PassengerTypeInfo").Element(a + "PaxType").Value,
        //                           TITLE = Pax.Val.Element(a + "Names").Element(a + "BookingName").Element(a + "Title").Value.Equals("CHD") ? (Pax.Val.Element(a + "PassengerInfo").Element(a + "Gender").Value.Equals("Male") ? "MSTR" : "MISS") : Pax.Val.Element(a + "Names").Element(a + "BookingName").Element(a + "Title").Value,
        //                           FIRSTNAME = Pax.Val.Element(a + "Names").Element(a + "BookingName").Element(a + "FirstName").Value,
        //                           LASTNAME = Pax.Val.Element(a + "Names").Element(a + "BookingName").Element(a + "LastName").Value,
        //                           MEALS = Pax.Val.Element(a + "PassengerFees").Element(a + "PassengerFee") == null ? "" : (Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => (Avl.Element(a + "SSRCode").Value.Equals("VGML") || Avl.Element(a + "SSRCode").Value.Equals("NVML"))).Count() > 0 ? Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => (Avl.Element(a + "SSRCode").Value.Equals("VGML") || Avl.Element(a + "SSRCode").Value.Equals("NVML"))).First().Element(a + "ServiceCharges").Element(a + "BookingServiceCharge").Element(a + "ChargeCode").Value : ""),
        //                           MEALSAMT = Pax.Val.Element(a + "PassengerFees").Element(a + "PassengerFee") == null ? "" : (Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => (Avl.Element(a + "SSRCode").Value.Equals("VGML") || Avl.Element(a + "SSRCode").Value.Equals("NVML"))).Count() > 0 ? Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => (Avl.Element(a + "SSRCode").Value.Equals("VGML") || Avl.Element(a + "SSRCode").Value.Equals("NVML"))).First().Element(a + "ServiceCharges").Element(a + "BookingServiceCharge").Element(a + "Amount").Value : ""),
        //                       };

        //DataTable dtPax = ConvertToDataTable(PassengerDetails);

        //var FareDetails = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
        //                  from Journey in Booking.Element(a + "Journeys").Elements(a + "Journey").Select((Val, index) => new { Val, index })
        //                  from Segment in Journey.Val.Element(a + "Segments").Elements(a + "Segment").Select((Val, index) => new { Val, index })
        //                  from PaxFare in Segment.Val.Element(a + "Fares").Element(a + "Fare").Element(a + "PaxFares").Elements(a + "PaxFare").Select((Val, index) => new { Val, index })
        //                  join PaxRef in PassengerDetails
        //                  on PaxFare.Val.Element(a + "PaxType").Value equals PaxRef.PAXTYPE
        //                  select new
        //                  {
        //                      PAXTYPE = PaxFare.Val.Element(a + "PaxType").Value,
        //                      ITINREF = Journey.index,

        //                      BASEAMT = PaxFare.Val.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Avl => Avl.Element(a + "ChargeCode").Value.Equals("")).First().Element(a + "Amount").Value,
        //                      GROSSAMT = PaxFare.Val.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Select(Avl => Convert.ToDouble(Avl.Element(a + "Amount").Value)).Sum(),
        //                      TOTALTAXAMT = PaxFare.Val.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Select(Avl => Convert.ToDouble(Avl.Element(a + "Amount").Value)).Sum(),
        //                      TAXBREAKUP = string.Join("/", PaxFare.Val.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Avl => !Avl.Element(a + "ChargeCode").Value.Equals("")).Select(Avl => Avl.Element(a + "ChargeCode").Value + ":" + Avl.Element(a + "Amount").Value).ToArray()),
        //                      CURRENCY = PaxFare.Val.Element(a + "ServiceCharges").Element(a + "BookingServiceCharge").Element(a + "CurrencyCode").Value,

        //                      APIBASEFARE = PaxFare.Val.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Avl => Avl.Element(a + "ChargeCode").Value.Equals("")).First().Element(a + "Amount").Value,
        //                      APIGROSSFARE = PaxFare.Val.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Select(Avl => Convert.ToDouble(Avl.Element(a + "Amount").Value)).Sum(),
        //                      APIYQFARE = PaxFare.Val.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Avl => Avl.Element(a + "ChargeCode").Value.Equals("YQ")).Count() > 0 ?
        //                                 (PaxFare.Val.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Avl => Avl.Element(a + "ChargeCode").Value.Equals("YQ")).First().Element(a + "Amount").Value) : "0",
        //                      APICURRENCY = PaxFare.Val.Element(a + "ServiceCharges").Element(a + "BookingServiceCharge").Element(a + "CurrencyCode").Value,
        //                      APITAXBREAKUP = string.Join("/", PaxFare.Val.Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Avl => (!Avl.Element(a + "ChargeCode").Value.Equals("") && !Avl.Element(a + "ChargeCode").Value.Equals("YQ"))).Select(Avl => Avl.Element(a + "ChargeCode").Value + ":" + Avl.Element(a + "Amount").Value).ToArray()),

        //                      SEQNO = PaxRef.SEQNO,
        //                      TITLE = PaxRef.TITLE,
        //                      FIRSTNAME = PaxRef.FIRSTNAME,
        //                      LASTNAME = PaxRef.LASTNAME,
        //                      MEALS = PaxRef.MEALS,
        //                      MEALSAMT = PaxRef.MEALSAMT,
        //                  };
        //DataTable dtFare = ConvertToDataTable(FareDetails);

        //var result = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
        //             from Journey in Booking.Element(a + "Journeys").Elements(a + "Journey").Select((Val, index) => new { Val, index })
        //             from Segment in Journey.Val.Element(a + "Segments").Elements(a + "Segment").Select((Val, index) => new { Val, index })
        //             join PaxRef in FareDetails
        //             on Journey.index equals PaxRef.ITINREF
        //             select new
        //             {
        //                 CRSPNR = "N/A",
        //                 AIRLINEPNR = Booking.Element(a + "RecordLocator").Value,
        //                 TICKETNO = Booking.Element(a + "RecordLocator").Value + "-" + PaxRef.SEQNO,
        //                 SUPPLIERPNR = "N/A",
        //                 FLIGHTNO = Segment.Val.Element(a + "FlightDesignator").Element(b + "FlightNumber").Value.Trim(),
        //                 ORIGIN = Segment.Val.Element(a + "DepartureStation").Value,
        //                 DESTINATION = Segment.Val.Element(a + "ArrivalStation").Value,
        //                 ORGINTERMINAL = "",
        //                 DESTTERMINAL = "",
        //                 DEPARTUREDATE = DateTime.ParseExact(Segment.Val.Element(a + "STD").Value.Split('T')[0], "yyyy-MM-dd", null).ToString("dd/MM/yyyy"),
        //                 ARRIVALDATE = DateTime.ParseExact(Segment.Val.Element(a + "STA").Value.Split('T')[0], "yyyy-MM-dd", null).ToString("dd/MM/yyyy"),
        //                 DEPARTURETIME = DateTime.ParseExact(Segment.Val.Element(a + "STD").Value.Split('T')[1], "HH:mm:ss", null).ToString("HH:mm"),
        //                 ARRIVALTIME = DateTime.ParseExact(Segment.Val.Element(a + "STA").Value.Split('T')[1], "HH:mm:ss", null).ToString("HH:mm"),
        //                 USERTRACKID = "",
        //                 //OFFICEID = hshCredential["OFFICEID"].ToString(),
        //                 AIRLINECODE = Segment.Val.Element(a + "FlightDesignator").Element(b + "CarrierCode").Value.Trim(),
        //                 TICKETINGCARRIER = Segment.Val.Element(a + "FlightDesignator").Element(b + "CarrierCode").Value.Trim(),
        //                 GROUP = "",
        //                 FQTV = "N/A",
        //                 STOCK = "6E",
        //                 STATUS = Segment.Val.Element(a + "ActionStatusCode").Value,

        //                 BAGGAGE = ((Journey.Val.Element(a + "Segments").Elements(a + "Segment").First().Element(a + "DepartureStation").Value + "," + Journey.Val.Element(a + "Segments").Elements(a + "Segment").Last().Element(a + "ArrivalStation").Value).Split(',').Except(OrgDest)).Count() > 0 ? ((("DXB,MCT").Contains(Segment.Val.Element(a + "DepartureStation").Value) || ("DXB,MCT").Contains(Segment.Val.Element(a + "ArrivalStation").Value)) ? "30 Kg" : "20 Kg") : "15 Kg",
        //                 BAGGAGEAMT = "0",
        //                 MEALS = PaxRef.MEALS,
        //                 MEALSAMT = PaxRef.MEALSAMT,
        //                 SEAT = "",
        //                 SEATAMT = "0",
        //                 WHEEL = "",
        //                 WHEELAMT = "0",

        //                 CLASS = Segment.Val.Element(a + "Fares").Element(a + "Fare").Element(a + "ClassOfService").Value,
        //                 CABIN = "",
        //                 FAREBASISCODE = Segment.Val.Element(a + "Fares").Element(a + "Fare").Element(a + "FareBasisCode").Value,
        //                 REFERENCE = "",

        //                 SEQNO = PaxRef.SEQNO,
        //                 PAXTYPE = PaxRef.PAXTYPE,
        //                 TITLE = PaxRef.TITLE,
        //                 FIRSTNAME = PaxRef.FIRSTNAME,
        //                 LASTNAME = PaxRef.LASTNAME,
        //                 DATEOFBIRTH = "",

        //                 BASEAMT = PaxRef.BASEAMT,
        //                 GROSSAMT = PaxRef.GROSSAMT,
        //                 TOTALTAXAMT = PaxRef.TOTALTAXAMT,
        //                 TAXBREAKUP = PaxRef.TAXBREAKUP,
        //                 CURRENCY = PaxRef.CURRENCY,

        //                 ITINREF = Journey.index + 1,
        //                 TRIPTYPE = Booking.Element(a + "Journeys").Elements(a + "Journey").Count() == 1 ? "O" : (Booking.Element(a + "Journeys").Elements(a + "Journey").Count() == 2 ? "R" : "M"),
        //                 AIRLINECATEGORY = "LCC",

        //                 APIBASEFARE = PaxRef.APIBASEFARE,
        //                 APIGROSSFARE = PaxRef.APIGROSSFARE,
        //                 APIYQFARE = PaxRef.APIYQFARE,
        //                 APICURRENCY = PaxRef.APICURRENCY,
        //                 APITAXBREAKUP = PaxRef.APITAXBREAKUP,

        //                 ROE_VALUE = "1",
        //                 SPECIALFARE = "N",
        //                 COMMISSION = "0",
        //                 SERVICECHARGE = "0",
        //                 MARKUP = "0",
        //                 INCENTIVE = "0",
        //                 SUPPCOMMISSION = "0",
        //                 SUPPINCENTIVE = "0",
        //                 SUPPMARKUP = "0",

        //                 BOOK_SUPID = "",
        //                 TKT_SUPID = "",
        //                 FAREID = Journey.index + 1,
        //                 EXCESSBAGGAGE = "",
        //             };
        //if (result.Count() == 0)
        //    return false;
        //int PaxCount = result.Where(Avl => Avl.ITINREF.ToString().Equals("1")).Select(A => A).Count();
        //dtResponse = ConvertToDataTable(result);

        //#region InfantFare
        //var InfantResult = from Booking in XDocument.Parse(Response).Descendants(xmlns + "GetBookingResponse").Elements(a + "Booking")
        //                   from Pax in Booking.Element(a + "Passengers").Elements(a + "Passenger").Select((Val, index) => new { Val, index })
        //                   from Journey in Booking.Element(a + "Journeys").Elements(a + "Journey").Select((Val, index) => new { Val, index })
        //                   from Segment in Journey.Val.Element(a + "Segments").Elements(a + "Segment").Select((Val, index) => new { Val, index })
        //                   from Inf in Pax.Val.Elements(a + "Infant").Select((Val, index) => new { Val, index })
        //                   where !Inf.ToString().Contains("nil")
        //                   select new
        //                   {
        //                       CRSPNR = "N/A",
        //                       AIRLINEPNR = Booking.Element(a + "RecordLocator").Value,
        //                       TICKETNO = "",
        //                       SUPPLIERPNR = "N/A",
        //                       FLIGHTNO = Segment.Val.Element(a + "FlightDesignator").Element(b + "FlightNumber").Value.Trim(),
        //                       ORIGIN = Segment.Val.Element(a + "DepartureStation").Value,
        //                       DESTINATION = Segment.Val.Element(a + "ArrivalStation").Value,
        //                       ORGINTERMINAL = "",
        //                       DESTTERMINAL = "",
        //                       DEPARTUREDATE = DateTime.ParseExact(Segment.Val.Element(a + "STD").Value.Split('T')[0], "yyyy-MM-dd", null).ToString("dd/MM/yyyy"),
        //                       ARRIVALDATE = DateTime.ParseExact(Segment.Val.Element(a + "STA").Value.Split('T')[0], "yyyy-MM-dd", null).ToString("dd/MM/yyyy"),
        //                       DEPARTURETIME = DateTime.ParseExact(Segment.Val.Element(a + "STD").Value.Split('T')[1], "HH:mm:ss", null).ToString("HH:mm"),
        //                       ARRIVALTIME = DateTime.ParseExact(Segment.Val.Element(a + "STA").Value.Split('T')[1], "HH:mm:ss", null).ToString("HH:mm"),
        //                       USERTRACKID = "",
        //                       //OFFICEID = hshCredential["OFFICEID"].ToString(),
        //                       AIRLINECODE = Segment.Val.Element(a + "FlightDesignator").Element(b + "CarrierCode").Value.Trim(),
        //                       TICKETINGCARRIER = Segment.Val.Element(a + "FlightDesignator").Element(b + "CarrierCode").Value.Trim(),
        //                       GROUP = "",
        //                       FQTV = "N/A",
        //                       STOCK = "6E",
        //                       STATUS = Segment.Val.Element(a + "ActionStatusCode").Value,

        //                       BAGGAGE = "",
        //                       BAGGAGEAMT = "",
        //                       MEALS = "",
        //                       MEALSAMT = "",

        //                       CLASS = Segment.Val.Element(a + "Fares").Element(a + "Fare").Element(a + "ClassOfService").Value,
        //                       CABIN = "",
        //                       FAREBASISCODE = Segment.Val.Element(a + "Fares").Element(a + "Fare").Element(a + "FareBasisCode").Value,
        //                       REFERENCE = "",

        //                       SEQNO = Inf.index + 1,
        //                       PAXTYPE = "INF",
        //                       TITLE = Inf.Val.Element(a + "Names").Element(a + "BookingName").Element(a + "Title").Value,
        //                       FIRSTNAME = Inf.Val.Element(a + "Names").Element(a + "BookingName").Element(a + "FirstName").Value,
        //                       LASTNAME = Inf.Val.Element(a + "Names").Element(a + "BookingName").Element(a + "LastName").Value,
        //                       DATEOFBIRTH = "",

        //                       BASEAMT = Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => Avl.Element(a + "SSRCode").Value.Equals("INFT")).First().Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Av => Av.Element(a + "ChargeCode").Value.Equals("INFT")).First().Element(a + "Amount").Value,
        //                       GROSSAMT = Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => Avl.Element(a + "SSRCode").Value.Equals("INFT")).First().Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Select(Avl => Convert.ToDouble(Avl.Element(a + "Amount").Value)).Sum(),
        //                       TOTALTAXAMT = Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => Avl.Element(a + "SSRCode").Value.Equals("INFT")).First().Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Select(Avl => Convert.ToDouble(Avl.Element(a + "Amount").Value)).Sum(),
        //                       TAXBREAKUP = "TAX:0",//string.Join("/", Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => Avl.Element(a + "SSRCode").Value.Equals("INFT")).First().Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Av => !Av.Element(a + "ChargeCode").Value.Equals("INFT")).Select(A => A.Element(a + "ChargeCode").Value + ":" + A.Element(a + "Amount").Value)),
        //                       CURRENCY = Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => Avl.Element(a + "SSRCode").Value.Equals("INFT")).First().Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Av => Av.Element(a + "ChargeCode").Value.Equals("INFT")).First().Element(a + "CurrencyCode").Value.Trim(),

        //                       ITINREF = Journey.index + 1,
        //                       TRIPTYPE = Booking.Element(a + "Journeys").Elements(a + "Journey").Count() == 1 ? "O" : (Booking.Element(a + "Journeys").Elements(a + "Journey").Count() == 2 ? "R" : "M"),
        //                       AIRLINECATEGORY = "LCC",

        //                       APIBASEFARE = Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => Avl.Element(a + "SSRCode").Value.Equals("INFT")).First().Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Av => Av.Element(a + "ChargeCode").Value.Equals("INFT")).First().Element(a + "Amount").Value,
        //                       APIGROSSFARE = Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => Avl.Element(a + "SSRCode").Value.Equals("INFT")).First().Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Select(Avl => Convert.ToDouble(Avl.Element(a + "Amount").Value)).Sum(),
        //                       APIYQFARE = "0",
        //                       APICURRENCY = Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => Avl.Element(a + "SSRCode").Value.Equals("INFT")).First().Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Av => Av.Element(a + "ChargeCode").Value.Equals("INFT")).First().Element(a + "CurrencyCode").Value,
        //                       APITAXBREAKUP = "TAX:0",//string.Join("/", Pax.Val.Element(a + "PassengerFees").Elements(a + "PassengerFee").Where(Avl => Avl.Element(a + "SSRCode").Value.Equals("INFT")).First().Element(a + "ServiceCharges").Elements(a + "BookingServiceCharge").Where(Av => !Av.Element(a + "ChargeCode").Value.Equals("INFT")).Select(A => A.Element(a + "ChargeCode").Value + ":" + A.Element(a + "Amount").Value)),

        //                       ROE_VALUE = "1",
        //                       SPECIALFARE = "N",
        //                       COMMISSION = "0",
        //                       SERVICECHARGE = "0",
        //                       MARKUP = "0",
        //                       INCENTIVE = "0",
        //                       SUPPCOMMISSION = "0",
        //                       SUPPINCENTIVE = "0",
        //                       SUPPMARKUP = "0",

        //                       BOOK_SUPID = "",
        //                       TKT_SUPID = "",
        //                       FAREID = Booking.Element(a + "Journeys").Elements(a + "Journey").Count() == 1 ? "1" : (Booking.Element(a + "Journeys").Elements(a + "Journey").Count() == 2 ? "2" : "3"),
        //                       EXCESSBAGGAGE = "",
        //                   };
        //DataTable dtInfantResponse = new DataTable();
        //dtInfantResponse = ConvertToDataTable(InfantResult);

        //if (dtInfantResponse != null && dtInfantResponse.Rows.Count != 0)
        //{
        //    if (dtInfantResponse.Rows[0]["TRIPTYPE"].ToString().Equals("O"))
        //        for (int k = 0; k < dtInfantResponse.Rows.Count; k++)
        //        {
        //            dtInfantResponse.Rows[k]["SEQNO"] = (PaxCount + (k + 1));
        //            dtInfantResponse.Rows[k]["TICKETNO"] = dtInfantResponse.Rows[k]["AIRLINEPNR"].ToString() + "-" + (PaxCount + (k + 1));
        //        }
        //    else
        //    {
        //        DataTable dtOnward = dtInfantResponse.Clone();
        //        DataTable dtReturn = dtInfantResponse.Clone();
        //        foreach (DataRow drow in dtInfantResponse.Rows)
        //        {
        //            if (drow["ITINREF"].ToString().Equals("1"))
        //                dtOnward.Rows.Add(drow.ItemArray);
        //            else
        //                dtReturn.Rows.Add(drow.ItemArray);
        //        }
        //        dtInfantResponse = new DataTable();
        //        dtInfantResponse = dtOnward.Clone();
        //        for (int k = 0; k < dtOnward.Rows.Count; k++)
        //        {
        //            dtOnward.Rows[k]["SEQNO"] = (PaxCount + (k + 1));
        //            dtOnward.Rows[k]["TICKETNO"] = dtOnward.Rows[k]["AIRLINEPNR"].ToString() + "-" + (PaxCount + (k + 1));
        //            dtInfantResponse.Rows.Add(dtOnward.Rows[k].ItemArray);
        //        }
        //        for (int k = 0; k < dtReturn.Rows.Count; k++)
        //        {
        //            dtReturn.Rows[k]["SEQNO"] = (PaxCount + (k + 1));
        //            dtReturn.Rows[k]["TICKETNO"] = dtReturn.Rows[k]["AIRLINEPNR"].ToString() + "-" + (PaxCount + (k + 1));
        //            dtInfantResponse.Rows.Add(dtReturn.Rows[k].ItemArray);
        //        }
        //    }
        //    foreach (DataRow drow in dtInfantResponse.Rows)
        //        dtResponse.Rows.Add(drow.ItemArray);
        //}
        #endregion

    }
}
