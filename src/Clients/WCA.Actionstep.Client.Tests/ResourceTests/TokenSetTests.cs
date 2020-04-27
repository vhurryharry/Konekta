using Newtonsoft.Json.Linq;
using NodaTime;
using NodaTime.Testing;
using System;
using System.IdentityModel.Tokens.Jwt;
using WCA.Actionstep.Client.Resources;
using Xunit;

namespace WCA.Actionstep.Client.Tests
{
    public class TokenTests
    {
        [Fact]
        public void CanParseTokenResponseWithIdToken()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");

            Assert.Equal("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ", token.AccessToken);
            Assert.Equal("bearer", token.TokenType);
            Assert.Equal(3600, token.ExpiresIn);
            Assert.Equal(new Uri(@"https://ap-southeast-2.actionstepstaging.com/api/"), token.ApiEndpoint);
            Assert.Equal("trial181078920", token.OrgKey);
            Assert.Equal(new JwtSecurityToken("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs").ToString(), token.IdToken.ToString());
            Assert.Equal("OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm", token.RefreshToken);

            Assert.Equal(Instant.FromUtc(2019, 10, 3, 12, 12, 0), token.AccessTokenExpiresAt);
            Assert.Equal(Instant.FromUtc(2019, 10, 24, 11, 12, 0), token.RefreshTokenExpiresAt);

            Assert.Equal("userId", token.UserId);
            Assert.Equal("id", token.Id);
        }

        [Fact]
        public void CanParseTokenResponseWithoutIdToken()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");

            Assert.Equal("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ", token.AccessToken);
            Assert.Equal("bearer", token.TokenType);
            Assert.Equal(3600, token.ExpiresIn);
            Assert.Equal(new Uri(@"https://ap-southeast-2.actionstepstaging.com/api/"), token.ApiEndpoint);
            Assert.Equal("trial181078920", token.OrgKey);
            Assert.Null(token.IdToken);
            Assert.Equal("OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm", token.RefreshToken);
        }

        [Fact]
        public void CanParseTokenResponseWithNullUserId()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), null, "id");

            Assert.Equal("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ", token.AccessToken);
            Assert.Equal("bearer", token.TokenType);
            Assert.Equal(3600, token.ExpiresIn);
            Assert.Equal(new Uri(@"https://ap-southeast-2.actionstepstaging.com/api/"), token.ApiEndpoint);
            Assert.Equal("trial181078920", token.OrgKey);
            Assert.Equal(new JwtSecurityToken("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs").ToString(), token.IdToken.ToString());
            Assert.Equal("OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm", token.RefreshToken);

            Assert.Equal(Instant.FromUtc(2019, 10, 3, 12, 12, 0), token.AccessTokenExpiresAt);
            Assert.Equal(Instant.FromUtc(2019, 10, 24, 11, 12, 0), token.RefreshTokenExpiresAt);

            Assert.Null(token.UserId);
            Assert.Equal("id", token.Id);
        }

        [Fact]
        public void CanParseTokenResponseWithEmptyUserId()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "", "id");

            Assert.Equal("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ", token.AccessToken);
            Assert.Equal("bearer", token.TokenType);
            Assert.Equal(3600, token.ExpiresIn);
            Assert.Equal(new Uri(@"https://ap-southeast-2.actionstepstaging.com/api/"), token.ApiEndpoint);
            Assert.Equal("trial181078920", token.OrgKey);
            Assert.Equal(new JwtSecurityToken("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs").ToString(), token.IdToken.ToString());
            Assert.Equal("OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm", token.RefreshToken);

            Assert.Equal(Instant.FromUtc(2019, 10, 3, 12, 12, 0), token.AccessTokenExpiresAt);
            Assert.Equal(Instant.FromUtc(2019, 10, 24, 11, 12, 0), token.RefreshTokenExpiresAt);

            Assert.Equal(string.Empty, token.UserId);
            Assert.Equal("id", token.Id);
        }

        [Fact]
        public void AccessTokenExistsAndHasntExpiredReturnsTrueForValidUnexpiredTokenWithTwoMinBuffer()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 1, 0, 0, 0), "userId", "id");

            Assert.True(token.AccessTokenAppearsValid(FakeClock.FromUtc(2019, 10, 1, 0, 30, 0), Duration.FromMinutes(2)));
        }

        [Fact]
        public void AccessTokenExistsAndHasntExpiredReturnsFalseForValidTokenWithinBuffer()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 1, 0, 0, 0), "userId", "id");

            Assert.False(token.AccessTokenAppearsValid(FakeClock.FromUtc(2019, 10, 1, 0, 59, 0), Duration.FromMinutes(2)));
        }

        [Fact]
        public void AccessTokenExistsAndHasntExpiredReturnsFalseForValidTokenAfterExpiry()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 1, 0, 0, 0), "userId", "id");

            Assert.False(token.AccessTokenAppearsValid(FakeClock.FromUtc(2019, 10, 1, 2, 0, 0), Duration.FromMinutes(2)));
        }

        [Fact]
        public void RefreshTokenExistsAndHasntExpiredReturnsTrueForValidUnexpiredToken()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 1, 0, 0, 0), "userId", "id");

            Assert.True(token.RefreshTokenAppearsValid(FakeClock.FromUtc(2019, 10, 1, 0, 30, 0)));
        }

        [Fact]
        public void RefreshTokenExistsAndHasntExpiredReturnsFalseForValidTokenAfterExpiry()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 1, 0, 0, 0), "userId", "id");

            // Refresh tokens are valid for 21 days.
            Assert.False(token.RefreshTokenAppearsValid(FakeClock.FromUtc(2019, 11, 1, 0, 0, 0)));
        }

        [Fact]
        public void EmptyAccessTokenThrowsInvalidTokenResponseException()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var ex = Assert.Throws<InvalidTokenResponseException>(() =>
            {
                var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");
            });

            Assert.Equal("The 'access_token' property was missing or empty in the token response.", ex.Message);
        }


        [Fact]
        public void NonJwtIdTokenThrowsInvalidTokenResponseException()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"INVALID-JWT-STRING\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var ex = Assert.Throws<InvalidTokenResponseException>(() =>
            {
                var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");
            });

            Assert.Equal("The 'id_token' property could not be parsed as a valid JWT token. The value received was 'INVALID-JWT-STRING'.", ex.Message);
        }

        [Fact]
        public void InvalidApiEndpointThrowsInvalidTokenResponseException()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var ex = Assert.Throws<InvalidTokenResponseException>(() =>
            {
                var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");
            });

            Assert.Equal("The 'api_endpoint' property could not be parsed as a valid absolute Uri. The value received was '/api/'.", ex.Message);
        }

        [Fact]
        public void MissingApiEndpointThrowsInvalidTokenResponseException()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var ex = Assert.Throws<InvalidTokenResponseException>(() =>
            {
                var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");
            });

            Assert.Equal("The 'api_endpoint' property was missing or empty in the token response.", ex.Message);
        }

        [Fact]
        public void ZeroLengthApiEndpointThrowsInvalidTokenResponseException()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var ex = Assert.Throws<InvalidTokenResponseException>(() =>
            {
                var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");
            });

            Assert.Equal("The 'api_endpoint' property was missing or empty in the token response.", ex.Message);
        }

        [Fact]
        public void MissingOrgKeyThrowsInvalidTokenResponseException()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"OTk4OTZiZDJlMjY0ZDNjMDlhZDA3MzhlYmE1OTNiMGI0Yjk2ZmU2MzUyZDRm\"" +
                "}");

            var ex = Assert.Throws<InvalidTokenResponseException>(() =>
            {
                var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");
            });

            Assert.Equal("The 'orgkey' property was missing or empty in the token response.", ex.Message);
        }

        [Fact]
        public void ZeroLengthOrgKeyThrowsInvalidTokenResponseException()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"\"" +
                "}");

            var ex = Assert.Throws<InvalidTokenResponseException>(() =>
            {
                var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");
            });

            Assert.Equal("The 'orgkey' property was missing or empty in the token response.", ex.Message);
        }

        [Fact]
        public void MissingRefreshTokenThrowsInvalidTokenResponseException()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"" +
                "}");

            var ex = Assert.Throws<InvalidTokenResponseException>(() =>
            {
                var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");
            });

            Assert.Equal("The 'refresh_token' property was missing or empty in the token response.", ex.Message);
        }

        [Fact]
        public void ZeroLengthRefreshTokenThrowsInvalidTokenResponseException()
        {
            // This is okay in Git because the access_token and access_token are no longer valid.
            var validTokenResponse = JObject.Parse(
                "{" +
                    "\"access_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhIjoiTjJWbVpUUmhNVFU1T0dKbVpEVTVaamRsTURSbE16TTFNREk0Tm1WaU1XUTJOV0ZsTVRRMU9ESTBZV0UzIiwiYiI6InRyaWFsMTgxMDc4OTIwIiwiYyI6IjEwLjYwLjMuNTQiLCJnIjoiVDIxNzI2LTIxNjc4LTEiLCJlIjoiRiIsImYiOiJBdXN0cmFsaWFcL0FkZWxhaWRlIiwiaCI6ImIiLCJkIjoiIiwibmJmIjoxNTcwMTAwMDk5LCJleHAiOjE1NzAxMDM3MDl9.bZtwbucWDq7qLmtGuXNQlhyLQfbX-R-E4dEkYTFLnCBLhD7A2hIv2Gl1qU3Yd_IWH5fbLYlIcwdeTiCbY7WGAGK3oA6FnU_kW5Ld2Qdm-jzbQ2kXcwPOw7PsGxeqZmbNNK5uHCiHkMnnKANupjWoey8bnVFv9GssDcfBJ2JCj4YuPOo7O1Nm70BVqEXZazqdiv26Euki6DEq5ByjrZE8iuC4uLPMZM_K3IYhDQio1TV-0_-w-0NDOr4BgPDthWTAT65z2Y1lHg8oSGfUJze1yWSuzpFdyc603j6ob2qUTcJwSKRKIoXCyGABNMXW6P5fYRjQA2e6yhyiSiMykSHolQ\"," +
                    "\"token_type\":\"bearer\"," +
                    "\"expires_in\":3600," +
                    "\"api_endpoint\":\"https:\\/\\/ap-southeast-2.actionstepstaging.com\\/api\\/\"," +
                    "\"orgkey\":\"trial181078920\"," +
                    "\"id_token\":\"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs\"," +
                    "\"refresh_token\":\"\"" +
                "}");

            var ex = Assert.Throws<InvalidTokenResponseException>(() =>
            {
                var token = new TokenSet(validTokenResponse, Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id");
            });

            Assert.Equal("The 'refresh_token' property was missing or empty in the token response.", ex.Message);
        }

        [Fact]
        public void ConstructorSetsPropertiesWithValidParameters()
        {
            var token = new TokenSet("accessToken", "tokenType", 1, new Uri("http://valid.endpoint/"), "orgKey", "refreshToken", Instant.FromUtc(2019, 10, 3, 11, 12), "userId", "id", new JwtSecurityToken("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs"));
            Assert.Equal("accessToken", token.AccessToken);
            Assert.Equal("tokenType", token.TokenType);
            Assert.Equal(1, token.ExpiresIn);
            Assert.Equal(new Uri("http://valid.endpoint/"), token.ApiEndpoint);
            Assert.Equal("orgKey", token.OrgKey);
            Assert.Equal("refreshToken", token.RefreshToken);
            Assert.Equal("userId", token.UserId);
            Assert.Equal(Instant.FromUtc(2019, 10, 3, 11, 12), token.ReceivedAt);
            Assert.Equal("id", token.Id);
            Assert.Equal(new JwtSecurityToken("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJhcGkuYWN0aW9uc3RlcHN0YWdpbmcuY29tIiwiYXpwIjoiNTkzMEJUUmNsb3VkM0JENzU3MjkwMDI2QkJFQjc4ODVEQzA3QzFCMDU2OEMiLCJzdWIiOiJleUp6Ykd3aU9pSXhPVEk1TkNJc0luTnNJam9pTVRBNU9UVWlmUSIsImF1ZCI6IjU5MzBCVFJjbG91ZDNCRDc1NzI5MDAyNkJCRUI3ODg1REMwN0MxQjA1NjhDIiwiZXhwIjoxNTcwMTAwNDEwLCJpYXQiOjE1NzAxMDAxMTAsIngtb3Jna2V5IjoidHJpYWwxODEwNzg5MjAiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvQWRlbGFpZGUiLCJuYW1lIjoiRGFuaWVsIFNtb24iLCJnaXZlbl9uYW1lIjoiRGFuaWVsIiwiZmFtaWx5X25hbWUiOiJTbW9uIiwiZW1haWwiOiJkYW5pZWxAd29ya2Nsb3VkLmNvbS5hdSIsImVtYWlsX3ZlcmlmaWVkIjoidHJ1ZSJ9.x5VtgfQ0Ix7X9QDkOOMMRKIaCBF1vgDyI2veS5DIbQs").ToString(), token.IdToken.ToString());
        }

        [Fact]
        public void ConstructorThrowsOnNullAccessToken()
        {
            var ex = Assert.Throws<ArgumentException>(() => { var token = new TokenSet(null, "tokenType", 1, new Uri("http://valid.endpoint/"), "orgKey", "refreshToken", Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("The parameter was null or empty. (Parameter 'accessToken')", ex.Message);
        }

        [Fact]
        public void ConstructorThrowsOnEmptyAccessToken()
        {
            var ex = Assert.Throws<ArgumentException>(() => { var token = new TokenSet("", "tokenType", 1, new Uri("http://valid.endpoint/"), "orgKey", "refreshToken", Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("The parameter was null or empty. (Parameter 'accessToken')", ex.Message);
        }

        [Fact]
        public void ConstructorThrowsOnNullTokenType()
        {
            var ex = Assert.Throws<ArgumentException>(() => { var token = new TokenSet("accessToken", null, 1, new Uri("http://valid.endpoint/"), "orgKey", "refreshToken", Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("The parameter was null or empty. (Parameter 'tokenType')", ex.Message);
        }

        [Fact]
        public void ConstructorThrowsOnEmptyTokenType()
        {
            var ex = Assert.Throws<ArgumentException>(() => { var token = new TokenSet("accessToken", "", 1, new Uri("http://valid.endpoint/"), "orgKey", "refreshToken", Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("The parameter was null or empty. (Parameter 'tokenType')", ex.Message);
        }

        [Fact]
        public void ConstructorThrowsIfExpiresInLessThanOne()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => { var token = new TokenSet("accessToken", "tokenType", 0, new Uri("http://valid.endpoint/"), "orgKey", "refreshToken", Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("The parameter must be greater than 0 (Parameter 'expiresIn')", ex.Message);
        }

        [Fact]
        public void ConstructorThrowsOnNullApiEndpoint()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => { var token = new TokenSet("accessToken", "tokenType", 1, null, "orgKey", "refreshToken", Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("Value cannot be null. (Parameter 'apiEndpoint')", ex.Message);
        }

        [Fact]
        public void ConstructorThrowsOnInvalidApiEndpoint()
        {
            var ex = Assert.Throws<ArgumentException>(() => { var token = new TokenSet("accessToken", "tokenType", 1, new Uri("/not/an/absolute/uri", UriKind.Relative), "orgKey", "refreshToken", Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("Must be an absolute uri. (Parameter 'apiEndpoint')", ex.Message);
        }

        [Fact]
        public void ConstructorThrowsOnNullOrgKey()
        {
            var ex = Assert.Throws<ArgumentException>(() => { var token = new TokenSet("accessToken", "tokenType", 1, new Uri("http://valid.endpoint/"), null, "refreshToken", Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("The parameter was null or empty. (Parameter 'orgKey')", ex.Message);
        }

        [Fact]
        public void ConstructorThrowsOnEmptyOrgKey()
        {
            var ex = Assert.Throws<ArgumentException>(() => { var token = new TokenSet("accessToken", "tokenType", 1, new Uri("http://valid.endpoint/"), "", "refreshToken", Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("The parameter was null or empty. (Parameter 'orgKey')", ex.Message);
        }

        [Fact]
        public void ConstructorThrowsOnNullRefreshToken()
        {
            var ex = Assert.Throws<ArgumentException>(() => { var token = new TokenSet("accessToken", "tokenType", 1, new Uri("http://valid.endpoint/"), "orgKey", null, Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("The parameter was null or empty. (Parameter 'refreshToken')", ex.Message);
        }

        [Fact]
        public void ConstructorThrowsOnEmptyRefreshToken()
        {
            var ex = Assert.Throws<ArgumentException>(() => { var token = new TokenSet("accessToken", "tokenType", 1, new Uri("http://valid.endpoint/"), "orgKey", "", Instant.FromUtc(2019, 10, 3, 11, 12), "userId"); });
            Assert.Equal("The parameter was null or empty. (Parameter 'refreshToken')", ex.Message);
        }
    }
}