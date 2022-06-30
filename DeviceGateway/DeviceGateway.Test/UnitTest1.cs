using System;
using Xunit;
using DeviceGateway;
using DeviceGateway.InboundChannels;
using DeviceGateway.OutboundChannels;
using Moq;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace DeviceGateway.Test
{
    public class UnitTest1  // name after method/ name after class
    {

        private Mock<IInboundChannel> Inbound;
        private Mock<IOutboundChannel> Outbound;

        public UnitTest1()
        {
            Inbound = new Mock<IInboundChannel>();
            Outbound = new Mock<IOutboundChannel>();
        }

        [Fact]
        public void TestMethod1()  // exactly one testcase
        {
            // ARRANGE
            List<JObject> returnData = new List<JObject>();
            // TODO: create expected data

            Inbound.Setup(x => x.getInboundData()).Returns(returnData);

            // get list inboud
            List<IInboundChannel> iChannels = new List<IInboundChannel> { Inbound.Object };

            // get list outbound
            List<IOutboundChannel> oChannels = new List<IOutboundChannel> { Outbound.Object };

            var gateway = new DeviceGateway(iChannels, oChannels, "TestGateway", 10);

            // ACT
            gateway.Synchronize(default(CancellationToken));  // maybe use source here

            // ASSERT
            Assert.Equal(5, 4);
        }
    }
}
