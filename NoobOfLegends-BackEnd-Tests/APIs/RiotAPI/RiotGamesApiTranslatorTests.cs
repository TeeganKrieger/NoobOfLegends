using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoobOfLegends.APIs.RiotApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoobOfLegends_BackEnd.Models.API.RiotAPI.Tests
{
    [TestClass]
    public class RiotGamesApiTranslatorTests
    {
        

        private RiotPUUID testPuuid = new RiotPUUID("BNTI8qJ7csWvi63clRZbqcmARhG1Z9Fp1NiasqiMqb7HjaXnK9gmzjriypYIcDi63OWfcZ-e0E9-bQ");
        private RiotSummoner testSummoner = new RiotSummoner("Vm7J9ZNp4zW8ykNGbmHyhH9AfZniGYHlVGMOonWj8iJr2BA");
        private RiotSummoner testSummonerRanked = new RiotSummoner("eQQiTu0eY-Cqb87NBlOTALXTs0Ggl8lj-sBD1-rbjbalWs0");
        private string testMatchId = "NA1_4155692370";

        public RiotGamesApiTranslatorTests()
        {
            
        }

        [TestMethod]
        public async Task GetPUUIDTest()
        {
            RiotGamesApiTranslator translator = new RiotGamesApiTranslator();
            translator.SetPlatform(RiotApiPlatform.NA1);

            RiotPUUID puuid = await translator.GetPUUID("Saltymate8#NA1");
            Assert.AreEqual(puuid, testPuuid);
        }

        [TestMethod]
        public async Task GetPUUIDTest1()
        {
            RiotGamesApiTranslator translator = new RiotGamesApiTranslator();
            translator.SetPlatform(RiotApiPlatform.NA1);

            RiotPUUID puuid = await translator.GetPUUID("Saltymate8", "NA1");
            Assert.AreEqual(puuid, testPuuid);
        }

        [TestMethod]
        public async Task GetSummonerTest()
        {
            RiotGamesApiTranslator translator = new RiotGamesApiTranslator();
            translator.SetPlatform(RiotApiPlatform.NA1);

            RiotSummoner summoner = await translator.GetSummoner(testPuuid);
            Assert.IsNotNull(summoner);
            Assert.AreEqual(summoner.id, testSummoner.id);
        }

        [TestMethod]
        public async Task GetMatchHistoryTest()
        {
            RiotGamesApiTranslator translator = new RiotGamesApiTranslator();
            translator.SetPlatform(RiotApiPlatform.NA1);

            string[] matches = await translator.GetMatchHistory(testPuuid);

            Assert.IsNotNull(matches);
            Assert.IsTrue(matches.Length > 0);
        }

        [TestMethod]
        public async Task GetMatchTest()
        {
            RiotGamesApiTranslator translator = new RiotGamesApiTranslator();
            translator.SetPlatform(RiotApiPlatform.NA1);

            RiotMatch match = await translator.GetMatch(testMatchId);
            Assert.IsNotNull(match);
        }

        [TestMethod]
        public async Task GetMatchTimelineTest()
        {
            RiotGamesApiTranslator translator = new RiotGamesApiTranslator();
            translator.SetPlatform(RiotApiPlatform.NA1);

            RiotMatchTimeline matchTimeline = await translator.GetMatchTimeline(testMatchId);
            Assert.IsNotNull(matchTimeline);
        }

        [TestMethod]
        public async Task GetSummonerRankedDataTest()
        {
            RiotGamesApiTranslator translator = new RiotGamesApiTranslator();
            translator.SetPlatform(RiotApiPlatform.NA1);

            RiotRankedResult[] rankedResults = await translator.GetSummonerRankedData(testSummonerRanked);
            Assert.IsNotNull(rankedResults);
            Assert.IsTrue(rankedResults.Length > 0);
        }


        [TestMethod]
        public async Task GetListOfSummonersInRankTest()
        {
            RiotGamesApiTranslator translator = new RiotGamesApiTranslator();
            translator.SetPlatform(RiotApiPlatform.NA1);

            foreach (RiotRankedQueue queue in Enum.GetValues(typeof(RiotRankedQueue)))
            {
                foreach (RiotRankedTier tier in Enum.GetValues(typeof(RiotRankedTier)))
                {
                    foreach (RiotRankedDivision division in Enum.GetValues(typeof(RiotRankedDivision)))
                    {
                        RiotRankedResult[] rankedResults = await translator.GetListOfSummonersInRank(queue, tier, division);
                        Assert.IsNotNull(rankedResults, $"Failed at {queue} {tier} {division}");
                        Assert.IsTrue(rankedResults.Length > 0);
                        await Task.Delay(500); //Delay 500 ms to avoid rate limits causing a test fail
                    }
                }
            }
        }
    }
}