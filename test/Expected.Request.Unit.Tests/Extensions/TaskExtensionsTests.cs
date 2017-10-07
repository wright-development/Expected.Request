using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Expected.Request.Unit.Tests.Extensions
{
    public class TaskExtensionsTests
    {
        [Fact]
        public async Task should_complete_on_done()
        {
            var expectMock = new Mock<IExpectRequest>();

            await Task.FromResult(expectMock.Object)
            .Done();

            expectMock.Verify(x => x.Done());
        }
    }
}