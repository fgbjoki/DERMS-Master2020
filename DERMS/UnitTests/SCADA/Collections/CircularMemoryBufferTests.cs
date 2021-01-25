using FieldProcessor.TCPCommunicationHandler.Collection;
using NUnit.Framework;

namespace UnitTests.SCADA.Collections
{
    public class CircularMemoryBufferTests
    {
        private readonly int bufferSize = 6;

        public CircularMemoryBufferTests()
        {

        }

        [TestCaseSource("CircularMemoryBuffer_PutValidTestCases")]
        public void CircularMemoryBuffer_PutGetValidTest(byte[] data)
        {
            int expectedSize = bufferSize - data.Length;
            CircularMemoryBuffer buffer = new CircularMemoryBuffer(bufferSize);

            buffer.Put(data);
            Assert.AreEqual(data.Length, buffer.Size);

            byte[] put = buffer.Get(data.Length);
            Assert.AreEqual(0, buffer.Size);
            CollectionAssert.AreEqual(data, put);
        }

        [Test]
        public void CircularMemoryBuffer_PutValidTestSwapPointersTest()
        {
            byte[] expectedFirstPut = new byte[] { 1, 2, 3, 4 };
            byte[] expectedSecondPut = new byte[] { 5, 6, 7, 8, 9 };

            int expectedSize = 0;
            CircularMemoryBuffer buffer = new CircularMemoryBuffer(bufferSize);

            buffer.Put(expectedFirstPut);
            byte[] firstPut = buffer.Get(expectedFirstPut.Length);
            buffer.Put(expectedSecondPut);
            byte[] secondPut = buffer.Get(expectedSecondPut.Length);

            Assert.AreEqual(expectedSize, buffer.Size);
            CollectionAssert.AreEqual(expectedFirstPut, firstPut);
            CollectionAssert.AreEqual(expectedSecondPut, secondPut);
        }

        private static object[] CircularMemoryBuffer_PutValidTestCases =
        {
            new byte[4] { 1, 2, 3, 4 },
            new byte[6] { 1, 2, 3, 4, 5, 6 } ,
            new byte[1] {1} ,
            new byte[0] ,
        };
    }
}
