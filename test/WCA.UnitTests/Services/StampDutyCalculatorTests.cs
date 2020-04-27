using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WCA.Core.Services;
using WCA.Domain.Models;
using Xunit;
using Xunit.Abstractions;

namespace WCA.UnitTests.Services
{
    public class StampDutyCalculatorTests
    {
        private readonly ITestOutputHelper _output;

        public StampDutyCalculatorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public static IEnumerable<object[]> GetCorrectStampDutyCalculatorData
        {
            get
            {
                // Test data created using http://stampdutycalc.com.au/
                // and http://www.realestate.com.au/calculators/stamp-duty-calculator/

                string dataPath = "StampDutyTestData.csv";
                using (TextReader tr = File.OpenText(dataPath))
                using (CsvReader csvReader = new CsvReader(tr))
                {
                    while (csvReader.Read())
                    {
                        // Expected fees
                        var feeLineItems = new List<FinancialResultLineItem>();
                        AddResultLineItem(feeLineItems, "Mortgage Fee", csvReader, "ExpectedMortgageFee", true);
                        AddResultLineItem(feeLineItems, "Transfer Fee", csvReader, "ExpectedTransferFee", true);
                        decimal transferDutyPayable = csvReader.GetField<decimal>("ExpectedStampDutyPayable");
                        feeLineItems.Add(new FinancialResultLineItem("Transfer Duty Payable", transferDutyPayable));

                        // Buyer sale information
                        string[] allBuyersIntendedPropertyUse = csvReader.GetField<string>("IntendedPropertyUse").Split(';');
                        string[] allBuyersIsFirstHomeBuyer = csvReader.GetField<string>("IsFirstHomeBuyer").Split(';');
                        string[] allBuyersIsForeignBuyer = csvReader.GetField<string>("IsForeignBuyer").Split(';');
                        string[] allBuyersShares = csvReader.GetField<string>("Shares").Split(';');
                        var allBuyerInfo = new List<PropertyBuyer>();

                        // Buyer Result information
                        string[] allBuyersExpectedStampDutyPerBuyer = csvReader.GetField<string>("ExpectedStampDutyPerBuyer").Split(';'); ;
                        string[] allBuyersExpectedForeignerDuty = csvReader.GetField<string>("ExpectedForeignerDuty").Split(';'); ;
                        string[] allBuyersExpectedFirstHomeGrant = csvReader.GetField<string>("ExpectedFirstHomeGrant").Split(';'); ;

                        var stampDutySummaryCategoryLineItems = new List<FinancialResultLineItem>();
                        var concessionLineItems = new List<FinancialResultLineItem>();

                        // Information dependent on number of buyers
                        int numberOfBuyers = csvReader.GetField<int>("NumberOfBuyers");
                        for (int i = 0; i < numberOfBuyers; i++)
                        {
                            int buyerNumber = i + 1;

                            // Sale information
                            allBuyerInfo.Add(new PropertyBuyer(buyerNumber,
                                (IntendedPropertyUse)Enum.Parse(typeof(IntendedPropertyUse), allBuyersIntendedPropertyUse[i]),
                                bool.Parse(allBuyersIsFirstHomeBuyer[i]),
                                bool.Parse(allBuyersIsForeignBuyer[i]),
                                new Fraction(allBuyersShares[i])
                            ));

                            // Result information
                            AddResultLineItem(feeLineItems, $"Buyer {buyerNumber} - Foreign Buyers Duty", allBuyersExpectedForeignerDuty[i], false);
                            AddResultLineItem(stampDutySummaryCategoryLineItems, $"Buyer {buyerNumber}", allBuyersExpectedStampDutyPerBuyer[i], false);
                            AddResultLineItem(concessionLineItems, $"Buyer {buyerNumber} - First Home Owners Grant", allBuyersExpectedFirstHomeGrant[i], false);
                        }

                        // Get information not dependent on number of buyers
                        decimal purchasePrice = csvReader.GetField<decimal>("PurchasePrice");
                        State state = csvReader.GetField<State>("State");
                        PropertyType propertyType = csvReader.GetField<PropertyType>("PropertyType");

                        // Wrap everything up to be returned
                        PropertySaleInformation saleInfo = new PropertySaleInformation(purchasePrice, state, propertyType, allBuyerInfo.ToArray());
                        FinancialResultCategory feeCategory = new FinancialResultCategory("Fees", feeLineItems.ToArray());
                        FinancialResultCategory stampDutySummaryCategory = new FinancialResultCategory("Stamp Duty Summary", stampDutySummaryCategoryLineItems.ToArray());
                        FinancialResultCategory concessionCategory = new FinancialResultCategory("Concessions", concessionLineItems.ToArray());

                        FinancialResults expectedResults;
                        if (stampDutySummaryCategoryLineItems.Count > 0 && stampDutySummaryCategory.Total > 0)
                        {
                            expectedResults = new FinancialResults(new[] { feeCategory, stampDutySummaryCategory, concessionCategory });
                        }
                        else
                        {
                            expectedResults = new FinancialResults(new[] { feeCategory, concessionCategory });
                        }

                        decimal expectedTotalFees = csvReader.GetField<decimal>("ExpectedTotalFees");
                        decimal expectedTotalConcessions = csvReader.GetField<decimal>("ExpectedTotalConcessions");
                        int scenarioId = csvReader.GetField<int>("ScenarioID");
                        yield return new object[] { saleInfo, expectedResults, expectedTotalFees, transferDutyPayable, expectedTotalConcessions, scenarioId };
                    }
                }
            }
        }


        private static void AddResultLineItem(List<FinancialResultLineItem> lineItems, string title, string decimalValue, bool addIfZero = false)
        {
            decimal value = 0M;
            if (decimal.TryParse(decimalValue, out value))
            {
                if (value > 0 || addIfZero)
                {
                    lineItems.Add(new FinancialResultLineItem(title, value));
                }
            }
            else
            {
                if (addIfZero)
                {
                    lineItems.Add(new FinancialResultLineItem(title, value));
                }
            }
        }

        private static void AddResultLineItem(List<FinancialResultLineItem> lineItems, string title, CsvReader csvReader, string csvHeading, bool addIfZero = false)
        {
            decimal value = csvReader.GetField<decimal>(csvHeading);
            if (value > 0 || addIfZero)
            {
                lineItems.Add(new FinancialResultLineItem(title, value));
            }
        }

        [Fact]
        public void FinancialLineItemEquality()
        {
            FinancialResultLineItem item1 = new FinancialResultLineItem("Title", 10);
            FinancialResultLineItem item2 = new FinancialResultLineItem("Title", 10);

            Assert.Equal(item1, item2);
        }

        [Fact]
        public void FinancialResultCategoryEquality()
        {
            var items1 = new[] {
                new FinancialResultLineItem("Title1", 10),
                new FinancialResultLineItem("Title2", 10)
            };

            var items2 = new[] {
                new FinancialResultLineItem("Title1", 10),
                new FinancialResultLineItem("Title2", 10)
            };

            FinancialResultCategory category1 = new FinancialResultCategory("catTitle", items1);
            FinancialResultCategory category2 = new FinancialResultCategory("catTitle", items2);

            Assert.Equal(category1, category2);
        }

        [Fact]
        public void FinancialResultsEquality()
        {
            var items1 = new[] {
                new FinancialResultLineItem("Title1", 10),
                new FinancialResultLineItem("Title2", 10)
            };

            var items2 = new[] {
                new FinancialResultLineItem("Title1", 10),
                new FinancialResultLineItem("Title2", 10)
            };

            FinancialResultCategory category1 = new FinancialResultCategory("catTitle", items1);
            FinancialResultCategory category2 = new FinancialResultCategory("catTitle", items2);

            FinancialResults results1 = new FinancialResults(new[] { category1 });
            FinancialResults results2 = new FinancialResults(new[] { category2 });

            Assert.Equal(results1, results2);
        }

        [Theory]
        [MemberData(nameof(GetCorrectStampDutyCalculatorData))]
        public void StampDutyCalculatorCorrectFees(PropertySaleInformation saleInfo, FinancialResults expectedResults,
            decimal expectedTotalFees, decimal transferDutyPayable, decimal expectedTotalConcessions, int scenarioId)
        {
            var calc = new StampDutyService();

            FinancialResults results = calc.Calculate(saleInfo);

            _output.WriteLine($"Testing scenario {scenarioId}");

            try
            {
                FinancialResultCategory stampDutySummaryCategory = results.Categories.Where(c => c.Title == "Stamp Duty Summary").SingleOrDefault();
                if (stampDutySummaryCategory != null)
                {
                    Assert.Equal(transferDutyPayable, stampDutySummaryCategory.Total);
                }

                Assert.Equal(expectedTotalFees, results.Categories.Where(c => c.Title == "Fees").Single().Total);
                Assert.Equal(expectedTotalConcessions, results.Categories.Where(c => c.Title == "Concessions").Single().Total);
                Assert.Equal(expectedResults, results);
            }
            catch
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                var serialiser = Newtonsoft.Json.JsonSerializer.Create(settings);

                using (TextWriter writer = new StringWriter())
                {
                    serialiser.Serialize(writer, expectedResults);
                    _output.WriteLine("Expected financial results:");
                    _output.WriteLine(writer.ToString());
                }

                using (TextWriter writer = new StringWriter())
                {
                    serialiser.Serialize(writer, results);
                    _output.WriteLine("Actual financial result:");
                    _output.WriteLine(writer.ToString());
                }
                throw;
            }
        }
    }
}