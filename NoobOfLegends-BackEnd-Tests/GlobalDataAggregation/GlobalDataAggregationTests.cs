using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoobOfLegends.APIs.RiotApi;
using NoobOfLegends_BackEnd.Models.GlobalAggregation;

namespace NoobOfLegends_BackEnd_Tests.GlobalDataAggregation
{
    [TestClass]
    public class GlobalDataAggregationTests
    {

        public GlobalDataAggregationTests()
        {

        }

        [TestMethod]
        public async Task AggregateGlobalDataTest()
        {
            RiotRankedQueue queue = RiotRankedQueue.RankedSolo5v5;
            int numTopPlayers = 1;
            int numMatches = 1;

            GlobalAggregation aggregator = new GlobalAggregation();
            await aggregator.AggregateGlobalData(queue, numTopPlayers, numMatches);
        }
    }
}
