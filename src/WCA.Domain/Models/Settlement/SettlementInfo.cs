using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace WCA.Domain.Models.Settlement
{
    public class SettlementInfo
    {
        public MatterDetails MatterDetails { get; set; }
#pragma warning disable CA1819 // Properties should not return arrays: Front-end code will need fixing after these are resolved, or we just treat as a DTO and continue to ignore this warning.
        public Dictionary<string, dynamic>[] Adjustments { get; set; }
        public Dictionary<string, dynamic>[] Fees { get; set; }
        public Dictionary<string, dynamic>[] AdditionalRequirements { get; set; }
        public Dictionary<string, dynamic>[] Payees { get; set; }
        public Dictionary<string, dynamic>[] OurRequirements { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
#pragma warning disable CA2227 // Collection properties should be read only: Front-end code will need fixing after these are resolved, or we just treat as a DTO and continue to ignore this warning.
        public Dictionary<string, dynamic> WaterUsage { get; set; }
        public Dictionary<string, dynamic> AdditionalInfo { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        public SettlementInfo(
            MatterDetails matterDetails,
            Dictionary<string, dynamic>[] adjustments,
            Dictionary<string, dynamic>[] fees,
            Dictionary<string, dynamic>[] additionalRequirements,
            Dictionary<string, dynamic>[] payees,
            Dictionary<string, dynamic>[] ourRequirements,
            Dictionary<string, dynamic> waterUsage,
            Dictionary<string, dynamic> additionalInfo)
        {
            var adjustmentsLength = (adjustments is null) ? 0 : adjustments.Length;
            Adjustments = new Dictionary<string, dynamic>[adjustmentsLength];

            var feesLength = (fees is null) ? 0 : fees.Length;
            Fees = new Dictionary<string, dynamic>[feesLength];

            var additionalRequirementsLength = (additionalRequirements is null) ? 0 : additionalRequirements.Length;
            AdditionalRequirements = new Dictionary<string, dynamic>[additionalRequirementsLength];

            var payeesLength = (payees is null) ? 0 : payees.Length;
            Payees = new Dictionary<string, dynamic>[payeesLength];

            var ourRequirementsLength = (ourRequirements is null) ? 0 : ourRequirements.Length;
            OurRequirements = new Dictionary<string, dynamic>[ourRequirementsLength];

            Array.Copy(adjustments, Adjustments, adjustmentsLength);
            Array.Copy(fees, Fees, feesLength);
            Array.Copy(additionalRequirements, AdditionalRequirements, additionalRequirementsLength);
            Array.Copy(payees, Payees, payeesLength);
            Array.Copy(ourRequirements, OurRequirements, ourRequirementsLength);

            MatterDetails = matterDetails;
            WaterUsage = waterUsage;
            AdditionalInfo = additionalInfo;
        }

        public SettlementInfo()
        {
            Adjustments = Array.Empty<Dictionary<string, dynamic>>();
            Fees = Array.Empty<Dictionary<string, dynamic>>();
            AdditionalRequirements = Array.Empty<Dictionary<string, dynamic>>();
            Payees = Array.Empty<Dictionary<string, dynamic>>();
            OurRequirements = Array.Empty<Dictionary<string, dynamic>>();
            MatterDetails = new MatterDetails();
            WaterUsage = new Dictionary<string, dynamic>();
            AdditionalInfo = new Dictionary<string, dynamic>();
        }

        public override string ToString()
        {
            string dataString = JsonConvert.SerializeObject(this);
            return dataString;
        }

        public static SettlementInfo FromString(string dataString)
        {
            SettlementInfo settlementData = JsonConvert.DeserializeObject<SettlementInfo>(dataString);
            return settlementData;
        }

        private static string GenerateResult(Dictionary<string, dynamic> Item, bool isAdjustment)
        {
            var result = $@"<tr>
                                <td></td><td></td>
                                <td align='right'>--------------------</td>
                            </tr>
                            <tr>
                                <td></td>
                                {(isAdjustment ? $@"<td align='right'><b>{Item["credit"].ToString("N2")}</b></td><td align='right'><b>{Item["debit"].ToString("N2")}</b></td>" :
                                $@"<td align='right'><b>{Item["debit"].ToString("N2")}</b></td><td align='right'><b>{Item["credit"].ToString("N2")}</b></td>")}
                            </tr>";

            return result;
        }

        private string GenerateMatterInfoContent()
        {
            string result = "";

            switch (MatterDetails.State)
            {
                case "QLD":
                    result = $@"
                        <table id='header' width='100%'>
                            <tr>
                                <td colspan='4' align='center'><h3>{AdditionalInfo["title"]}</h3></td>
                            </tr>
                        </table>

                        <table width='100%'>
                            <tr>
                                <td width='40%'><b>Matter:</b></td>
                                <td>{MatterDetails.Matter} ({MatterDetails.MatterRef})</td>
                            </tr>
                            <tr>
                                <td><b>Property:</b></td>
                                <td>{MatterDetails.Property}</td>
                            </tr>
                            <tr>
                                <td><b>Adjustment Date:</b>  </td>
                                <td>{MatterDetails.AdjustmentDate.ToUniversalTime().ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}</td>
                            </tr>
                            <tr>
                                <td><b>Settlement Date:</b>  </td>
                                <td>{MatterDetails.SettlementDate.ToUniversalTime().ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}</td>
                            </tr>
                            <tr>
                                <td><b>Settlement Place:</b></td>
                                <td>{MatterDetails.SettlementPlace}</td>
                            </tr>
                            <tr>
                                <td><b>Settlement Time:</b>  </td>
                                <td>{MatterDetails.SettlementTime}</td>
                            </tr>
                        </table>";
                    break;

                case "NSW":
                    result = $@"
                        <table id='header' width='100%'>
                            <tr>
                                <td colspan='4' align='center'><h3>{AdditionalInfo["title"]}</h3></td>
                            </tr>
                        </table>

                        <table width='100%'>
                            <tr>
                                <td align='center'><b>{MatterDetails.Matter} ({MatterDetails.MatterRef})</b></td>
                            </tr>
                            <tr>
                                <td align='center'><b>PROPERTY: {MatterDetails.Property}</b></td>
                            </tr>
                            <tr>
                                <td><br></td>
                            </tr>
                            <tr>
                                <td align='center'>Settlement: {MatterDetails.SettlementDate.ToUniversalTime().ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}</td>
                            </tr>
                            <tr>
                                <td align='center'>Adjustment as at: {MatterDetails.AdjustmentDate.ToUniversalTime().ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}</td>
                            </tr>
                        </table>";
                    break;

                case "VIC":
                    result = $@"<table width='100%'>

                            <tr>
                                <td align='center'><b>{MatterDetails.Matter.ToUpper(CultureInfo.InvariantCulture)} ({MatterDetails.MatterRef})</b></td>
                            </tr>
                            <tr>
                                <td align='center'><b>PROPERTY: {MatterDetails.Property.ToUpper(CultureInfo.InvariantCulture)}</b></td>
                            </tr>
                            <tr>
                                <td align='center' class='badge'>
                                    <div><b>STATEMENT OF ADJUSTMENTS</b></div>
                                </td>
                            </tr>
                            <tr>
                                <td align='center'><b>DATE OF ADJUSTMENT: {MatterDetails.AdjustmentDate.ToUniversalTime().ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}</b></td>
                            </tr>
                            <tr>
                                <td align='center'><b>DATE OF SETTLEMENT: {MatterDetails.SettlementDate.ToUniversalTime().ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}</b></td>
                            </tr>
                        </table>";
                    break;

                default:
                    result = $@"

                        <table id='header' width='100%'>
                            <tr>
                                <td colspan='4' align='center'><h3>{AdditionalInfo["title"]}</h3></td>
                            </tr>
                        </table>

                        <table width='100%'>
                            <tr>
                                <td width='40%'><b>Matter:</b></td>
                                <td>{MatterDetails.Matter} ({MatterDetails.MatterRef})</td>
                            </tr>
                            <tr>
                                <td><b>Property:</b></td>
                                <td>{MatterDetails.Property}</td>
                            </tr>
                            <tr>
                                <td><b>Adjustment Date:</b>  </td>
                                <td>{MatterDetails.AdjustmentDate.ToUniversalTime().ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}</td>
                            </tr>
                            <tr>
                                <td><b>Settlement Date:</b>  </td>
                                <td>{MatterDetails.SettlementDate.ToUniversalTime().ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}</td>
                            </tr>
                            <tr>
                                <td><b>Settlement Place:</b></td>
                                <td>{MatterDetails.SettlementPlace}</td>
                            </tr>
                            <tr>
                                <td><b>Settlement Time:</b>  </td>
                                <td>{MatterDetails.SettlementTime}</td>
                            </tr>
                        </table>";
                    break;
            }


            return result;
        }

        public string GeneratePDFContent(string actionstepOrgName)
        {
            bool isAdjustment = AdditionalInfo["isAdjustment"] == true;
            var matterInfoContent = GenerateMatterInfoContent();

            var pdfContent = @"<head>

                                    <link rel='stylesheet' type='text/css' href='http://fonts.googleapis.com/css?family=Calibri'>
                                    <style>
                                        body {
                                            font-family: 'Calibri';
                                            font-size: 20px;
                                            padding: 20px;
                                        }
                                        td {
                                            font-size: 20px;
                                        }
                                        h3 {
                                            font-size: 25px;
                                        }
                                        .footer {
                                            font-size: 15px;
                                            padding: 10px;
                                        }

                                        .badge {
                                            width: 100%;
                                            padding-top: 20px;
                                            padding-bottom: 20px;
                                        }

                                        .badge div {
                                            height: 100%;
                                            padding: 3px;
                                            background-color: rgb(239, 239, 239);
                                            border: 1px solid black;
                                        }

                                    </style>
                                </head>";

            pdfContent += $@"<body>

                            {matterInfoContent}

                            <table width='100%'>
                                <tr>
                                   <td colspan='5'><hr></td>
                                </tr>
                                <tr>
                                   <td></td>
                                   <td align='right'><b>$ DEBIT</b></td>
                                   <td align='right'><b>$ CREDIT</b></td>
                                </tr>";

            foreach (Dictionary<string, dynamic> Adjustment in Adjustments)
            {
                var result = GenerateResult(Adjustment, isAdjustment);
                DateTime from, to;

                switch (Adjustment["title"])
                {
                    case "Contract Price":
                        if (MatterDetails.State != "VIC")
                        {
                            pdfContent += $@"<tr>
                                <td><b>Contract Price</b></td>
                                {(isAdjustment ? "<td></td>" : "")}
                                <td align='right'>{Adjustment["price"].ToString("N2")}</td>
                            </tr>";

                            if (Adjustment["deposit"] != 0)
                            {
                                pdfContent += $@"<tr>
                                    <td><b>Deposit</b></td>
                                    {(isAdjustment ? "" : "<td></td>")}
                                    <td align='right'>{Adjustment["deposit"].ToString("N2")}</td>
                                </tr>";
                            }
                        }
                        break;

                    case "Release Fee":
                        pdfContent += $@"<tr>
                                <td><b>Sellers Release Fee</b></td>
                            </tr>
                            <tr>
                                <td>{((MatterDetails.State == "VIC" || MatterDetails.State == "NSW") ? "Vendor allows " : "")}{ Adjustment["each"].ToString("N2")}{((MatterDetails.State == "VIC" || MatterDetails.State == "NSW") ? " x Discharge of Mortgage" : "")} @ ${Adjustment["mortgages"].ToString("N2")}</td>
                                {(isAdjustment ? "" : "<td></td>")}
                                <td align='right'>{Adjustment["result"].ToString("N2")}</td>
                            </tr>";

                        break;

                    case "Water Usage":
                        pdfContent += $@"<tr>
                                   <td>
                                      <b>Water Usage</b>
                                      <div>(see calculation following)</div>
                                </td>
                                {(isAdjustment ? "" : "<td></td>")}
                                <td align='right'><div>{Adjustment["result"].ToString("N2")}</div></td>
                               </tr>";

                        break;

                    case "Penalty Interest":
                        from = Adjustment["from"];
                        to = Adjustment["to"];
                        pdfContent += $@" <tr>
                                <td>
                                    <b>Penalty Interest</b>
                                    <div>from {from.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)} to {to.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)} - {Adjustment["days"]} days @ {Adjustment["rate"].ToString("N2")}%</div>
                                </td>
                                {(isAdjustment ? "<td></td>" : "")}
                                <td align='right'>{Adjustment["result"].ToString("N2")}</td>
                            </tr>";

                        break;

                    case "Other Adjustment":
                        pdfContent += $@"<tr>
                                <td>
                                    <b>{Adjustment["description"]}</b>
                                </td>
                                {((Adjustment["status"] == "Plus") == isAdjustment ? "<td></td>" : "")}
                                <td align='right'>{Adjustment["amount"].ToString("N2")}</td>
                            </tr>";

                        break;

                    default:
                        from = Adjustment["from"];
                        to = Adjustment["to"];
                        pdfContent += $@" <tr>
                                <td>
                                    <b>{Adjustment["title"]}</b>";

                        if (MatterDetails.State == "VIC" || MatterDetails.State == "NSW")
                        {
                            var today = DateTime.Today;
                            var julyFirst = new DateTime(today.Year, 7, 1);
                            DateTime adjustment = MatterDetails.AdjustmentDate;

                            pdfContent += $@"<br>
                                For period {from.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)} to {to.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)} - {Adjustment["days"]} days
                                <br>
                                ${Adjustment["amount"]} {(Adjustment["status"] == "unpaid" ? "Unpaid" : "Paid")}
                                <br>
                                {(Adjustment["status"] == "unpaid" ? $@"Vendor allows {((TimeSpan)(from - adjustment)).Days} days"
                                : $@"Purchaser allows {((TimeSpan)(to - adjustment)).Days} days")}
                                <br>
                                For period {adjustment.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)} to {to.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}";
                        }
                        else
                        {
                            pdfContent += $@"<div>${Adjustment["amount"]} for the period {from.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)} to {to.ToString("dd MMM yyyy", CultureInfo.InvariantCulture)}
                                    <br>Proportion being {Adjustment["adjustDays"]} / {Adjustment["days"]} days</div>";
                        }

                        pdfContent += $@"</td>
                                {((Adjustment["status"] == "Plus") == isAdjustment ? "<td></td>" : "")}
                                <td align='right'>{Adjustment["result"].ToString("N2")}</td>
                            </tr>";

                        break;
                }

                if (MatterDetails.State == "QLD")
                    pdfContent += result;

            }

            if (MatterDetails.State == "SA")
            {
                foreach (Dictionary<string, dynamic> Fee in Fees)
                {
                    if (Fee["showOnAdjustment"] == true)
                    {
                        pdfContent += $@"<tr>
                                <td><b>{Fee["description"]}</b></td>
                                {((Fee["status"] == "Plus") == isAdjustment ? "<td></td>" : "")}
                                <td align='right'>{Fee["amount"].ToString("N2")}</td>
                            </tr>";
                    }
                }
            }

            double contractDebit = AdditionalInfo["contractDebit"], contractCredit = AdditionalInfo["contractCredit"];
            if (MatterDetails.State == "VIC")
            {
                contractDebit -= Adjustments[0]["price"];
                contractCredit -= Adjustments[0]["deposit"];
            }

            pdfContent += $@"
                    <tr><td></td></tr>
                    <tr>
                       <td></td><td></td>
                       <td align='right'>--------------------</td>
                    </tr>
                    <tr>
                       <td {(MatterDetails.State == "VIC" ? "" : "align='right'")}><b>{(MatterDetails.State == "NSW" ? "AMOUNT DUE ON SETTLEMENT" : (MatterDetails.State == "VIC" ? "PURCHASER TO PAY VENDOR" : "CONTRACT BALANCE"))}</b></td>
                       {(isAdjustment ? $@"<td align='right'><b>{AdditionalInfo["contractCredit"].ToString("N2")}</b></td><td align='right'><b>{AdditionalInfo["contractDebit"].ToString("N2")}</b></td>"
                       : $@"<td align='right'><b>{contractDebit.ToString("N2", CultureInfo.InvariantCulture)}</b></td><td align='right'><b>{contractCredit.ToString("N2", CultureInfo.InvariantCulture)}</b></td>")}
                    </tr>
                    <tr><td></td></tr>";

            if (MatterDetails.State == "VIC")
            {
                var balance = Adjustments[0]["price"] - Adjustments[0]["deposit"];
                var plusAdjustments = contractDebit - contractCredit;
                var totalBalance = balance + plusAdjustments;

                pdfContent += $@"
                            <tr>
                                <td colspan='3' align='center' class='badge'>
                                    <div><b>SETTLEMENT STATEMENT</b></div>
                                </td>
                            </tr>

                            <tr>
                                <td>Purchase Price:</td>
                                <td></td>
                                <td align='right'>{Adjustments[0]["price"].ToString("N2")}</td>
                            </tr>

                            <tr>
                                <td>Less Deposit Paid:</td>
                                <td></td>
                                <td align='right'>{Adjustments[0]["deposit"].ToString("N2")}</td>
                            </tr>

                            <tr>
                               <td></td><td></td>
                               <td align='right'>--------------------</td>
                            </tr>

                            <tr>
                                <td>Balance:</td>
                                <td></td>
                                <td align='right'>{balance.ToString("N2")}</td>
                            </tr>

                            <tr>
                                <td>Plus Adjustments:</td>
                                <td></td>
                                <td align='right'>{plusAdjustments.ToString("N2", CultureInfo.InvariantCulture)}</td>
                            </tr>

                            <tr>
                               <td></td><td></td>
                               <td align='right'>--------------------</td>
                            </tr>

                            <tr>
                                <td><b>BALANCE DUE TO VENDOR:</b></td>
                                <td></td>
                                <td align='right'><b>${totalBalance.ToString("N2", CultureInfo.InvariantCulture)}</b></td>
                            </tr>
                        ";
            }

            if (MatterDetails.State == "SA" && !isAdjustment)
            {
                pdfContent += $@"
                    <tr><td></td></tr>
                    <tr>
                       <td align='left'><b>FEES</b></td>
                    </tr>";

                foreach (Dictionary<string, dynamic> Fee in Fees)
                {
                    if (Fee["showOnAdjustment"] == false)
                    {
                        pdfContent += $@"<tr>
                                <td><b>{Fee["description"]}</b></td>
                                {((Fee["status"] == "Plus") == isAdjustment ? "<td></td>" : "")}
                                <td align='right'>{Fee["amount"].ToString("N2")}</td>
                            </tr>";
                    }
                }

                pdfContent += $@"
                    <tr><td></td></tr>
                    <tr>
                       <td></td><td></td>
                       <td align='right'>--------------------</td>
                   </tr>
                   <tr>
                       <td><b>TOTAL</b></td>

                       {(isAdjustment ? $@"<td align='right'><b>{AdditionalInfo["feeCredit"].ToString("N2")}</b></td><td align='right'><b>{AdditionalInfo["feeDebit"].ToString("N2")}</b></td>"
                       : $@"<td align='right'><b>{AdditionalInfo["feeDebit"].ToString("N2")}</b></td><td align='right'><b>{AdditionalInfo["feeCredit"].ToString("N2")}</b></td>")}
                   </tr>";

            }

            if (AdditionalRequirements.Length > 0 && MatterDetails.State != "VIC" && AdditionalInfo["showAdditionalRequirements"])
            {
                pdfContent += $@"<tr><td></td></tr>
                        <tr><td></td></tr>
                        <tr><td></td></tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td><b>ADDITIONAL REQUIREMENTS AT SETTLEMENT</b></td>
                        </tr>
                        <tr>
                            <td><b>Balance at Settlement</b></td>
                            <td align='right'>
                                <b>{AdditionalInfo["contractBalance"].ToString("N2")}</b>
                            </td>
                        </tr>
                        <tr><td></td></tr>";

                foreach (Dictionary<string, dynamic> addReq in AdditionalRequirements)
                {
                    pdfContent += $@"<tr>
                                <td>
                                    <div> {addReq["description"]}</div>
                                </td>
                                {((addReq["status"] == "Plus") == isAdjustment ? "<td></td>" : "")}
                                <td align='right'>{addReq["amount"].ToString("N2")}</td>
                            </tr>";
                }

                pdfContent += $@"
                    <tr><td></td></tr>
                    <tr>
                       <td></td><td></td>
                       <td align='right'>--------------------</td>
                    </tr>
                    <tr>
                        <td><b>TOTAL</b></td>
                        {(isAdjustment ? $@"<td align='right'><b>{AdditionalInfo["additionalCredit"].ToString("N2")}</b></td><td align='right'><b>{AdditionalInfo["additionalDebit"].ToString("N2")}</b></td>"
                        : $@"<td align='right'><b>{AdditionalInfo["additionalDebit"].ToString("N2")}</b></td><td align='right'><b>{AdditionalInfo["additionalCredit"].ToString("N2")}</b></td>")}
                    </tr>

                    <tr><td><br></td></tr>
                ";

            }

            if (Payees.Length > 0)
            {
                if (MatterDetails.State == "VIC")
                {
                    pdfContent += @"
                        <tr>
                            <td colspan='3' align='center' class='badge'>
                                <div><b>SETTLEMENT CHEQUES</b></div>
                            </td>
                        </tr>";
                }
                else
                {
                    pdfContent += $@"
                        <tr>
                           <td align='left'><b>PAYEE</b></td>
                        </tr>";
                }

                var payeeCounter = 1;

                foreach (Dictionary<string, dynamic> payee in Payees)
                {
                    pdfContent += $@"<tr>
                           <td>
                               <div>{payeeCounter}. {payee["description"]}</div>
                           </td>
                           <td></td>
                           <td align='right'>{payee["amount"].ToString("N2")}</td>
                        </tr>";

                    payeeCounter++;
                }

                pdfContent += $@"<tr>
                           <td></td>
                           <td align='right'></td>
                        </tr>
                        <tr>
                           <td></td><td></td>
                           <td align='right'>--------------------</td>
                        </tr>
                        <tr>
                           <td><b>{(MatterDetails.State == "VIC" ? "TOTAL CHEQUES:" : "TOTAL")}</b></td>
                           {(isAdjustment ? $@"<td align='right'><b>${AdditionalInfo["payeeCredit"].ToString("N2")}</b></td><td align='right'><b>${AdditionalInfo["payeeDebit"].ToString("N2")}</b></td>"
                           : (MatterDetails.State == "VIC" ? $@"<td></td><td align='right'><b>${AdditionalInfo["payeeTotal"].ToString("N2")}</b></td>"
                           : $@"<td align='right'><b>${AdditionalInfo["payeeDebit"].ToString("N2")}</b></td><td align='right'><b>${AdditionalInfo["payeeCredit"].ToString("N2")}</b></td>"))}
                        </tr>
                        <tr>
                           <td></td><td></td>
                           <td align='right'>(unallocated: ${AdditionalInfo["unallocated"].ToString("N2")})</td>
                        </tr>";
            }

            if (OurRequirements.Length > 0)
            {
                pdfContent += $@"<tr>
                           <td colspan='5'></td>
                        </tr>
                        <tr>
                           <td align='left'><b>OUR REQUIREMENTS AT SETTLEMENT</b></td>
                        </tr>";

                foreach (Dictionary<string, dynamic> ourReq in OurRequirements)
                {
                    pdfContent += $@"<tr>
                           <td>
                              <div>{ourReq["detail"]}</div>
                           </td>
                        </tr>";
                }
            }

            if (AdditionalInfo["hasWaterUsage"] == true)
            {

                pdfContent += $@"<br><div id='view_water_calc'>
                    <table width='100%'>
                        <tr>
                            <td align='left'><b>WATER USAGE CALCULATION</b><br/></td>
                            <td></td>
                        </tr>
                    <tr>
                        <td><div id='date_water_paid_to'>Date water paid to: {WaterUsage["paidDate"].ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}</div></td>
                        <td><div id='amount_paid_to'>Reading : {WaterUsage["paidReadingAmount"].ToString("N3")}kL</div></td>
                    </tr>
                    <tr>
                        <td><div id='date_water_reading'>Date of search reading: {WaterUsage["searchDate"].ToString("dd MMMM yyyy", CultureInfo.InvariantCulture)}</div></td>
                        <td><div id='amount_reading'>Reading : {WaterUsage["searchReadingAmount"].ToString("N3")}kL</div></td>
                    </tr>
                    <tr>
                        <td align='center'><div id='daily_usage'><hr><b>Average daily usage</b></div></td>
                        <td align='center'><div id='per_kl'><hr><b><i>Charge per kL</i></b></div></td>
                    </tr>
                    <tr><td align='center'>";

                var usageDetails = WaterUsage["averageKlCount"];

                if (WaterUsage["method"] != "daily-average")
                {
                    pdfContent += $@"<div id='days_bet_reading'>Days between readings: {WaterUsage["numberOfDays"].ToString("0")}</div>";
                    usageDetails = $@"{WaterUsage["searchReadingAmount"].ToString("N3")}kL - {WaterUsage["paidReadingAmount"].ToString("N3")}kL = {WaterUsage["diffAmountReading"].ToString("N3")}kL / {WaterUsage["numberOfDays"].ToString("0")} = <b>{WaterUsage["dailyUsage"].ToString("N3")}kl</b>";
                }

                pdfContent += $@"</td><td align='center'><div id='tier1'>${WaterUsage["tier1Charge"].ToString("N3")} for first {WaterUsage["tier1KlCount"].ToString("N3")}kL</div></td>
                   </tr>
                   <tr>
                      <td align='center'><div id='usage_details'>{usageDetails}</b></div></td>
                      <td align='center'><div id='tier2'>${WaterUsage["tier2Charge"].ToString("N3")} for the next {WaterUsage["tier2KlCount"].ToString("N3")}kL</div></td>
                   </tr>
                   <tr>
                      <td align='center'><div id='days_to_settlement'>Days from date paid to settlement = {WaterUsage["partDays"].ToString("0")}</div></td>
                      <td align='center'><div id='balance'>${WaterUsage["balanceCharge"].ToString("N3")} for the balance</div></td>
                   </tr>
                   <tr>
                      <td align='center'><div id='daily_and_days'>{WaterUsage["dailyUsage"].ToString("N3")}kL x {WaterUsage["partDays"].ToString("0")}days = <b>{WaterUsage["dailyAndDays"].ToString("N3")}kL</b></div></td>
                      <td align='center'><div id='bulk_water'>Bulk water ${WaterUsage["bulkCharge"].ToString("N3")}</div></td>
                   </tr>";

                if (WaterUsage["tier1FeeIncrease"] != 0 || WaterUsage["tier2FeeIncrease"] != 0 ||
                        WaterUsage["balanceFeeIncrease"] != 0 || WaterUsage["bulkFeeIncrease"] != 0)
                {
                    pdfContent += $@"
                                <tr><td></td></tr>
                                <tr>
                                    <td align='center'><div id='calc_july1'>Days from date paid to 30 June = {WaterUsage["numberOfDaysToJune"].ToString("0")}<br>
                                        Days from 1 July to settlement = {WaterUsage["numberOfDaysFromJune"].ToString("0")}<br></div></td>
                                    <td align='center'><div id='data_july1'><b><i>Charge per kL from 1 July</i></b><br>
                                            ${WaterUsage["tier1FeeIncrease"].ToString("N3")} for first {WaterUsage["tier1KlCount"].ToString("N3")} kL<br>
                                            ${WaterUsage["tier2FeeIncrease"].ToString("N3")} for next {WaterUsage["tier2KlCount"].ToString("N3")}  kL<br>
                                            ${WaterUsage["balanceFeeIncrease"].ToString("N3")}  for the balance<br> Bulk water ${WaterUsage["bulkFeeIncrease"].ToString("N3")}</div></td>
                                </tr>";

                    pdfContent += $@"<tr>
                              <td align='center'><div id='round'><i>(All kL results are rounded to whole litres ie 3 decimal places)</i><hr></div></td>
                              <td align='center'><div id='kl'><br><hr></div></td>
                            </tr>
                            </table>
                            <table width='100%'>
                               <tr>
                                  <td><div id='water_adj'><b><i>Water Adjustment</i></b></div></td>
                               </tr>
                               <tr>
                                  <td><div id='tier1_name'>Tier 1</div></td>
                                  <td align='center'><div id='tier1_calc'>{WaterUsage["tier1KlCount"].ToString("N3")} kL x {WaterUsage["numberOfDaysToJune"].ToString("0")}/{WaterUsage["partDays"].ToString("0")} days x ${WaterUsage["tier1Charge"].ToString("N3")}</div></td>
                                  <td align='right'><div id='tier1_calc'>${WaterUsage["tier1CalcResult"].ToString("N3")}</div></td>
                               </tr>
                               <tr>
                                  <td><div id='tier2_name'>Tier 2</div></td>
                                  <td align='center'><div id='tier2_calc'>{WaterUsage["tier2KlCount"].ToString("N3")} kL x {WaterUsage["numberOfDaysToJune"].ToString("0")}/{WaterUsage["partDays"].ToString("0")} days x ${WaterUsage["tier2Charge"].ToString("N3")}</div></td>
                                  <td align='right'><div id='tier1_calc'>${WaterUsage["tier2CalcResult"].ToString("N3")}</div></td>
                               </tr>
                               <tr>
                                  <td><div id='balance_name'>Balances</div></td>
                                  <td align='center'><div id='balance_calc'>{WaterUsage["balanceCalc"].ToString("N3")} kL x {WaterUsage["numberOfDaysToJune"].ToString("0")}/{WaterUsage["partDays"].ToString("0")} days x ${WaterUsage["balanceCharge"].ToString("N3")}</div></td>
                                  <td align='right'><div id='tier1_calc'>${WaterUsage["balanceCalcResult"].ToString("N3")}</div></td>
                               </tr>
                               <tr>
                                  <td><div id='bulk_name'>Bulk water</div></td>
                                  <td align='center'><div id='bulk_calc'>{WaterUsage["dailyAndDays"].ToString("N3")} kL x {WaterUsage["numberOfDaysToJune"].ToString("0")}/{WaterUsage["partDays"].ToString("0")} days x ${WaterUsage["bulkCharge"].ToString("N3")}</div></td>
                                  <td align='right'><div id='tier1_calc'>${WaterUsage["bulkCalcResult"].ToString("N3")}</div></td>
                               </tr>";

                    pdfContent += $@"<tr>
                                 <td><div id='july1_c'><b>From 1 July</b></div></td>
                            </tr>
                            <tr>
                                <td><div id='july1_name'>Tier 1<br>
                                        Tier 2<br>
                                        Balance<br>
                                        Bulk water</div></td>
                                <td align='center'>
                                    <div id='july1_calc'>{WaterUsage["tier1KlCount"].ToString("N3")}kL x {WaterUsage["numberOfDaysFromJune"].ToString("0")}/{WaterUsage["partDays"].ToString("0")} days x ${WaterUsage["tier1FeeIncrease"].ToString("N3")}<br>
                                    {WaterUsage["tier2KlCount"].ToString("N3")}kL x {WaterUsage["numberOfDaysFromJune"].ToString("0")}/{WaterUsage["partDays"].ToString("0")} days x ${WaterUsage["tier2FeeIncrease"].ToString("N3")}<br>
                                    {WaterUsage["balanceCalc"].ToString("N3")}kL x {WaterUsage["numberOfDaysFromJune"].ToString("0")}/{WaterUsage["partDays"].ToString("0")} days x ${WaterUsage["balanceFeeIncrease"].ToString("N3")}<br>
                                    {WaterUsage["dailyAndDays"].ToString("N3")}kL x {WaterUsage["numberOfDaysFromJune"].ToString("0")}/{WaterUsage["partDays"].ToString("0")} days x ${WaterUsage["bulkFeeIncrease"].ToString("N3")}<br>
                                    </div>
                                </td>
                                <td align='right'>
                                    <div id='july1_result'>
                                        {WaterUsage["tier1Result"].ToString("N3")}<br>
                                        {WaterUsage["tier2Result"].ToString("N3")}<br>
                                        {WaterUsage["balanceResult"].ToString("N3")}<br>
                                        {WaterUsage["bulkResult"].ToString("N3")}<br>
                                    </div>
                                </td>
                            </tr>";

                    pdfContent += $@"<tr>
                                      <td><div></div></td>
                                      <td><div></div></td>
                                      <td align='right'><div id='water_usage_calc'>{WaterUsage["finalWaterUsageResult"].ToString("N2")}</div></td>
                                   </tr>
                                </table>
                            </div>";
                }
                else
                {
                    pdfContent += $@"<tr>
                                  <td align='center'><div id='round'><i>(All kL results are rounded to whole litres ie 3 decimal places)</i><hr></div></td>
                                  <td align='center'><div id='kl'><br><hr></div></td>
                               </tr>
                            </table>
                            <table width='100%'>
                               <tr>
                                  <td><div id='water_adj'><b><i>Water Adjustment</i></b></div></td>
                               </tr>
                               <tr>
                                  <td><div id='tier1_name'>Tier 1</div></td>
                                  <td align='center'><div id='tier1_calc'>{WaterUsage["tier1KlCount"].ToString("N3")}kL x ${WaterUsage["tier1Charge"].ToString("N3")}</div></td>
                                  <td align='right'><div id='tier1_result'>${WaterUsage["tier1Result"].ToString("N3")}</div></td>
                               </tr>
                               <tr>
                                  <td><div id='tier2_name'>Tier 2</div></td>
                                  <td align='center'><div id='tier2_calc'>{WaterUsage["tier2KlCount"].ToString("N3")}kL x ${WaterUsage["tier2Charge"].ToString("N3")}</div></td>
                                  <td align='right'><div id='tier2_result'>{WaterUsage["tier2Result"].ToString("N3")}</div></td>
                               </tr>
                               <tr>
                                  <td><div id='balance_name'>Balance</div></td>
                                  <td align='center'><div id='balance_calc'>{WaterUsage["balanceCalc"].ToString("N3")}kL x ${WaterUsage["balanceCharge"].ToString("N3")}</div></td>
                                  <td align='right'><div id='balance_result'>{WaterUsage["balanceResult"].ToString("N3")}</div></td>
                               </tr>
                               <tr>
                                  <td><div id='bulk_name'>Bulk water</div></td>
                                  <td align='center'><div id='bulk_calc'>{WaterUsage["dailyAndDays"].ToString("N3")}kL x ${WaterUsage["bulkCharge"].ToString("N3")}</div></td>
                                  <td align='right'><div id='bulk_result'>{WaterUsage["bulkResult"].ToString("N3")}</div></td>
                               </tr>";

                    pdfContent += $@"<tr>
                                      <td><div></div></td>
                                      <td><div></div></td>
                                      <td align='right'><div id='water_usage_calc'>{WaterUsage["finalWaterUsageResult"].ToString("N3")}</div></td>
                                   </tr>
                                </table>
                             </div>";
                }
            }

            pdfContent += @"<tr>
                           <td colspan='5'><hr></td>
                        </tr>
                    </table>";

            var currentTime = DateTime.UtcNow;
            pdfContent += $@"
                            <p class='footer'>
                                Prepared By: {actionstepOrgName} <br>
                                Date & Time: {currentTime.ToString("dd-MM-yyyy HH:mm tt", CultureInfo.InvariantCulture)}
                            </p>";

            pdfContent += "</body>";

            Console.WriteLine("{0}", pdfContent);

            return pdfContent;
        }
    }
}
